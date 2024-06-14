using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using SharePoint.Common.Utilities.Extensions;
using SharePoint.Common.Utilities;
using CamlexNET;
using System.Collections.Generic;
using System.Linq.Expressions;
using MOSS.Common.Utilities;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.Modules.EventReceivers
{
    [Guid("356EF509-F875-433B-B72A-DD4A31691FFE")]
    public class StoreItemEventReceiver : SPItemEventReceiver
    {

        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;
            HandleAddingUpdating(properties);
            this.EventFiringEnabled = true;
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;
            HandleAddingUpdating(properties);
            this.EventFiringEnabled = true;
        }

        /// <summary>
        /// An item is being deleted.
        /// </summary>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            // check article uase in orders
            properties.Web.TryUsingList(Consts.ListUrl.ORDERITEMS, (list) =>
            {
                string s_articleId = properties.ListItemId.ToString();
                SPQuery query = new SPQuery();
                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                expressions.Add(x => x[Consts.OrderItemsListFields.Article] == (DataTypes.LookupId)s_articleId);
                query.Query = Camlex.Query().WhereAny(expressions).ToString();
                SPListItemCollection listItems = list.GetItems(query);

                if (listItems.Count > 0)
                {
                    properties.ErrorMessage = String.Format(Consts.WARNING2, listItems.Count);
                    properties.Cancel = true;
                }
            });
        }

        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;
            if (IsStandardArticle(properties))
            {
                CheckItemThresholds(properties);
            }
            StartWorkflows(properties);
            this.EventFiringEnabled = true;
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
        }

        private void HandleAddingUpdating(SPItemEventProperties properties)
        {
            if (properties.EventType == SPEventReceiverType.ItemAdding)
                properties.AfterProperties[Consts.StoreItemsListFields.Number] = 0;

            if (IsStandardArticle(properties))
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    CheckIncome(properties);
                    SetIndicator(properties);   
                });
            }
        }
        
        private void CheckIncome(SPItemEventProperties properties)
        {
            string s_income = StringHelper.CheckForNull(properties.AfterProperties[Consts.StoreItemsListFields.Income]);
            int i_income = 0;            

            if (Int32.TryParse(s_income, out i_income) && i_income > 0)
            {
                string s_number = StringHelper.CheckForNull(properties.AfterProperties[Consts.StoreItemsListFields.Number]);
                int i_number = 0;
                Int32.TryParse(s_number, out i_number);

                if (i_income < 0) i_income = i_income * -1;
                i_number += i_income;

                properties.AfterProperties[Consts.StoreItemsListFields.Number] = i_number;
                properties.AfterProperties[Consts.StoreItemsListFields.Income] = 0;
            }            
        }

        private void SetIndicator(SPItemEventProperties properties)
        {
            Thresholds thresholds = GetThresholds(properties);
            SPFieldUrlValue indicatorField = new SPFieldUrlValue();            

            //зеленый не показываем
            //if (t.Threshold < t.Number)
            //{
            //    indicatorField.Description = Consts.ITEMS_ENOUGHT;
            //    indicatorField.Url = "/_layouts/images/kpipeppers-0.gif";                
            //}

            //красный
            if (thresholds.ReserveBalance >= thresholds.Number)
            {
                indicatorField.Description = Consts.ITEMS_NOTENOUGHT;
                indicatorField.Url = properties.Web.SharePointUrlToRelativeUrl("~sitecollection/lists/siteimages/red_box.png");
            }

            //желтый
            if (thresholds.ReserveBalance < thresholds.Number && thresholds.Number <= thresholds.Threshold)
            {
                indicatorField.Description = Consts.ITEMS_NOTMORE;
                indicatorField.Url = properties.Web.SharePointUrlToRelativeUrl("~sitecollection/lists/siteimages/yellow_box.png");
            }

            if (!String.IsNullOrEmpty(indicatorField.Url))
                properties.AfterProperties[Consts.StoreItemsListFields.Indicator] = indicatorField;
            else
                properties.AfterProperties[Consts.StoreItemsListFields.Indicator] = null;
        }

        private void CheckItemThresholds(SPItemEventProperties properties)
        {
            Thresholds t = GetThresholds(properties);

            if (t.Number <= t.Threshold)
            {
                //ответственное за управление складом лицо должно получить оповещение электронной почты о том, что на складе подходят к концу текущие канцелярские принадлежности.
                //CommonHelper.SendArticleNotification("5", "Проверте наличие товара", properties);
            }

            if (t.Number <= t.ReserveBalance)
            {
                //ответственное за управление складом лицо должно получить оповещение электронной почты о том, что на складе осталось критическое количество текущих канцелярских принадлежностей.
                //CommonHelper.SendArticleNotification("5", "Проверте наличие товара", properties);
            }
        }

        private Thresholds GetThresholds(SPItemEventProperties properties)
        {
            Thresholds result = new Thresholds();

            string s_itemThreshold = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.StoreItemsListFields.WarningThreshold));
            int i_itemThreshold = 0;
            Int32.TryParse(s_itemThreshold, out i_itemThreshold);

            string s_itemReserveBalance = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.StoreItemsListFields.ReserveBalance));
            int i_itemReserveBalance = 0;
            Int32.TryParse(s_itemReserveBalance, out i_itemReserveBalance);

            string s_storenumber = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.StoreItemsListFields.Number));
            int i_storenumber = 0;
            Int32.TryParse(s_storenumber, out i_storenumber);

            result.Number = i_storenumber;
            result.ReserveBalance = i_itemReserveBalance;
            result.Threshold = i_itemThreshold;

            return result;
        }

        private bool IsStandardArticle(SPItemEventProperties properties)
        {
            string ct = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, "ContentTypeId"));
            SPContentType ct_storeitem = properties.Web.ContentTypes[Consts.CT_STOREITEM];
            return String.IsNullOrEmpty(ct) || new SPContentTypeId(ct).IsChildOf(ct_storeitem.Id);
        }

        private void StartWorkflows(SPItemEventProperties properties)
        {
            int itemId = properties.ListItemId;
            string listName = properties.ListTitle;
            if (itemId <= 0 || String.IsNullOrEmpty(listName))
            {
                return;
            }

            using (SPSite ElevatedSite = SecurityHelper.GetElevatedSite(properties.SiteId))
            {
                using (SPWeb ElevatedWeb = ElevatedSite.OpenWeb(properties.RelativeWebUrl))
                {
                    SPList list = ElevatedWeb.Lists[listName];
                    SPListItem item = list.GetItemById(itemId);

                    if (item.Folder != null)
                    {
                        return;
                    }

                    string loginName;
                    if (properties.EventType == SPEventReceiverType.ItemAdded)
                    {
                        loginName = item.GetFieldValueUserLogin(SPBuiltInFieldId.Author);
                    }
                    else
                    {
                        loginName = item.GetFieldValueUserLogin(SPBuiltInFieldId.Editor);
                    }

                    CommonHelper.StartWorkFlow(ElevatedSite, list, item, loginName);
                }
            }
        }

        struct Thresholds
        {
            public int Threshold;
            public int ReserveBalance;
            public int Number;
        }
    }
}
