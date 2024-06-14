using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using CamlexNET;
using Microsoft.SharePoint;
using MOSS.Common.Utilities;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.Modules.EventReceivers
{
    [Guid("963A9FF7-7B05-4BD8-A0EC-D171B259864E")]
    public class OrderItemEventReceiver : SPItemEventReceiver
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
            string order_number = StringHelper.CheckForNull(properties.ListItem[Consts.OrderItemsListFields.OrderNumber]);
            Enums.OrderStatus order_status = CommonHelper.GetOrderStatus(properties.Web, order_number);
            if (order_status != Enums.OrderStatus.Draft)
            {
                properties.ErrorMessage = String.Format(Consts.WARNING1, CommonHelper.GetOrderStatusTitle(properties.Web, order_status));
                properties.Cancel = true;
            }
            else
                MOSSPropertyBagHelper.Instance.SetStringValue(properties.SiteId, Consts.ORDER_NEED_TO_BE_RECALCULATED, order_number);
        }

        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;
            HandleAddedUpdated(properties);
            string title = StringHelper.CheckForNull(properties.ListItem[SPBuiltInFieldId.Title]);
            string order_number = StringHelper.CheckForNull(properties.ListItem[Consts.OrderItemsListFields.OrderNumber]);
            if (String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(order_number))
            {
                properties.ListItem[SPBuiltInFieldId.Title] = String.Format("<{0}> - <{1}>", new SPFieldLookupValue(order_number).LookupValue, properties.ListItemId);
                properties.ListItem.SystemUpdate();
            }
            this.EventFiringEnabled = true;
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;
            HandleAddedUpdated(properties);
            this.EventFiringEnabled = true;
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            using (SPWeb web = properties.OpenWeb())
            {
                string order_number = MOSSPropertyBagHelper.Instance.GetStringValue(web, Consts.ORDER_NEED_TO_BE_RECALCULATED);
                UpdateOrderSum(properties.Web, order_number);
                MOSSPropertyBagHelper.Instance.DeleteStringValue(properties.SiteId, Consts.ORDER_NEED_TO_BE_RECALCULATED);
            }
        }

        private void HandleAddingUpdating(SPItemEventProperties properties)
        {
            string order_number = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.OrderItemsListFields.OrderNumber));
            Enums.OrderStatus order_status = CommonHelper.GetOrderStatus(properties.Web, order_number);
            if (order_status != Enums.OrderStatus.Draft)
            {
                properties.ErrorMessage = String.Format(Consts.WARNING1, CommonHelper.GetOrderStatusTitle(properties.Web, order_status));
                properties.Cancel = true;
            }
            else
            {
                StoreItemAvailabilityResult r = CheckStoreItemAvailability(properties);
                if (r.Result)
                {
                    UpdateOrderItemLinkedFields(properties);                    
                }
                else
                {
                    properties.ErrorMessage = String.Format(Consts.WARNING5, r.Title + " - " + r.StoreNumber, r.OrderedNumber);
                    properties.Cancel = true;
                }
            }
        }

        private void HandleAddedUpdated(SPItemEventProperties properties)
        {
            string order_number = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.OrderItemsListFields.OrderNumber));
            UpdateOrderSum(properties.Web, order_number);            
        }

        private void UpdateOrderItemLinkedFields(SPItemEventProperties properties)
        {
            string draft_status_name = CommonHelper.GetOrderStatusTitle(properties.Web, Enums.OrderStatus.Draft);
            string order_number = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.OrderItemsListFields.OrderNumber));
            string item_article = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.OrderItemsListFields.Article));
            bool orderIsDraft = false;

            // fill order related fields
            if (!String.IsNullOrEmpty(order_number))
            {
                int orderId = new SPFieldLookupValue(order_number).LookupId;
                properties.Web.TryUsingList(Consts.ListUrl.ORDERS, (list) =>
                    {
                        SPListItem item = list.GetItemById(orderId);
                        if (item != null)
                        {
                            string order_status = StringHelper.CheckForNull(item[item.Fields.GetFieldByInternalName(Consts.OrdersListFields.Status).Id]);
                            if (CommonHelper.OrderIsDraft(order_status))
                            {
                                properties.AfterProperties[Consts.OrderItemsListFields.OrderType] = item[item.Fields.GetFieldByInternalName(Consts.OrdersListFields.OrderType).Id];
                                properties.AfterProperties[Consts.OrderItemsListFields.OrderStatus] = item[item.Fields.GetFieldByInternalName(Consts.OrdersListFields.Status).Id];

                                orderIsDraft = true;
                            }
                        }
                    });
            }

            // fill store item related fields
            if (orderIsDraft && !String.IsNullOrEmpty(item_article))
            {
                int articleId = new SPFieldLookupValue(item_article).LookupId;
                properties.Web.TryUsingList(Consts.ListUrl.STOREITEMS, (list) =>
                {
                    SPListItem item = list.GetItemById(articleId);
                    if (item != null)
                    {
                        properties.AfterProperties[Consts.OrderItemsListFields.Title] = item[item.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Title).Id];
                        if (item.ContentTypeId.IsChildOf(Consts.CT_STOREITEM))
                        {
                            properties.AfterProperties[Consts.OrderItemsListFields.Category] = item[item.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Category).Id];
                            properties.AfterProperties[Consts.OrderItemsListFields.MeasureUnit] = item[item.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.MeasureUnit).Id];
                        }
                        else
                        {
                            properties.AfterProperties[Consts.OrderItemsListFields.MeasureUnit] = item[item.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.MeasureUnitNotStd).Id];
                        }
                        CultureInfo ci_eng = new CultureInfo(1033);
                        SPFieldCurrency cf = (SPFieldCurrency)item.Fields.GetFieldByInternalName(Consts.OrderItemsListFields.Price);
                        Double d_price = CommonHelper.GetCurrencyFieldValue(item.GetFormattedValue(Consts.OrderItemsListFields.Price), Consts.OrderItemsListFields.Price, cf.CurrencyLocaleId);
                        if (d_price > 0)
                            properties.AfterProperties[Consts.OrderItemsListFields.Price] = d_price.ToString(ci_eng.NumberFormat);                        
                        else                        
                            properties.AfterProperties[Consts.OrderItemsListFields.Price] = item[item.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Price).Id];                        
                    }
                });
            }
        }

        private void UpdateOrderSum(SPWeb web, string order_number)
        {
            if (!String.IsNullOrEmpty(order_number))
            {
                int orderId = new SPFieldLookupValue(order_number).LookupId;
                Double sum = CommonHelper.GetOrderSum(web, orderId);

                web.TryUsingList(Consts.ListUrl.ORDERS, (list) =>
                {
                    SPListItem item = list.GetItemById(orderId);
                    if (item != null)
                    {
                        item[item.Fields.GetFieldByInternalName(Consts.OrdersListFields.Price).Id] = sum;
                        item.SystemUpdate();
                    }
                });
            }
        }

        private StoreItemAvailabilityResult CheckStoreItemAvailability(SPItemEventProperties properties)
        {
            StoreItemAvailabilityResult result = new StoreItemAvailabilityResult();
            string item_article = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.OrderItemsListFields.Article));
            string orderedNumber = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.OrderItemsListFields.Number));
            if (!String.IsNullOrEmpty(orderedNumber) && !String.IsNullOrEmpty(item_article))
            {
                int i_orderedNumber = 0;
                Int32.TryParse(orderedNumber, out i_orderedNumber);
                result.OrderedNumber = i_orderedNumber;

                if (i_orderedNumber > 0)
                {
                    int articleId = new SPFieldLookupValue(item_article).LookupId;
                    properties.Web.TryUsingList(Consts.ListUrl.STOREITEMS, (list) =>
                    {
                        SPListItem storeItem = list.GetItemById(articleId);
                        if (storeItem != null && storeItem.ContentTypeId.IsChildOf(Consts.CT_STOREITEM))
                        {
                            string article_tite = StringHelper.CheckForNull(storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Title).Id]);
                            object o_storenumber = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Number).Id];
                            string s_storenumber = StringHelper.CheckForNull(o_storenumber);
                            int i_storenumber = 0;
                            Int32.TryParse(s_storenumber, out i_storenumber);

                            result.Title = article_tite;
                            result.StoreNumber = i_storenumber;
                            result.Result = i_orderedNumber <= i_storenumber;
                        }
                    });
                }
            }
            return result;
        }        
    }
}
