using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;
using CamlexNET;
using System.Collections.Generic;
using System.Linq.Expressions;
using MOSS.Common.Utilities;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.Modules.EventReceivers
{
    [Guid("72B11732-4459-4678-9F24-F01BCD0123BB")]
    public class OrdersEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            UpdateLinkedFields(properties);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            StoreItemsAvailabilityResult r = CheckStoreItemsAvailability(properties);
            if (r.Result)
            {                
                UpdateLinkedFields(properties);
                UpdateStoreItemStatistic(properties);
                AddLogForExistingOrder(properties);
            }
            else
            {
                string err_message = String.Empty;
                foreach (StoreItemAvailabilityResult itemResult in r.ItemResults)
                {
                    if (!itemResult.Result)
                        err_message += String.Format(Consts.WARNING5, itemResult.Title + " - " + itemResult.StoreNumber, itemResult.OrderedNumber) + Environment.NewLine;
                }
                properties.ErrorMessage = err_message;
                properties.Cancel = true;
            }
        }
        
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            string order_status = StringHelper.CheckForNull(properties.ListItem[Consts.OrdersListFields.Status]);
            if (String.IsNullOrEmpty(order_status) || (int)Enums.OrderStatus.Draft == new SPFieldLookupValue(order_status).LookupId)
            {
                // delete order items
                properties.Web.TryUsingList(Consts.ListUrl.ORDERITEMS, (list) =>
                {
                    string s_orderId = properties.ListItemId.ToString();
                    SPQuery query = new SPQuery();
                    var expressions = new List<Expression<Func<SPListItem, bool>>>();
                    expressions.Add(x => x[Consts.OrderItemsListFields.OrderNumber] == (DataTypes.LookupId)s_orderId);
                    query.Query = Camlex.Query().WhereAny(expressions).ToString();
                    SPListItemCollection listItems = list.GetItems(query);

                    if (listItems.Count > 0)
                    {
                        try
                        {
                            for (int i = listItems.Count - 1; i >= 0; i--)
                            {
                                listItems[i].Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            properties.ErrorMessage = ex.Message;
                            properties.Cancel = true;
                        }                        
                    }
                });  
            }
            else
            {
                properties.ErrorMessage = String.Format(Consts.WARNING4, new SPFieldLookupValue(order_status).LookupValue);
                properties.Cancel = true;
            }
            
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            AddLogForNewOrder(properties);            
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            
        }

        public override void ItemDeleted(SPItemEventProperties properties)
        {
        }

        private void UpdateLinkedFields(SPItemEventProperties properties)
        {
            string draft_status_name = CommonHelper.GetOrderStatusTitle(properties.Web, Enums.OrderStatus.Draft);
            string order_status = StringHelper.CheckForNull(CommonHelper.GetActualSPItemEventProperty(properties, Consts.OrdersListFields.Status));
            if (CommonHelper.OrderIsDraft(order_status))
            {
                string order_department = StringHelper.CheckForNull(properties.AfterProperties[Consts.OrdersListFields.Department]);
                if (!String.IsNullOrEmpty(order_department))
                {
                    int depId = new SPFieldLookupValue(order_department).LookupId;
                    properties.Web.TryUsingList(Consts.ListUrl.DEPARTMENTS, (list) =>
                    {
                        SPListItem item = list.GetItemById(depId);
                        if (item != null)
                        {
                            properties.AfterProperties[Consts.OrdersListFields.BU] = item[item.Fields.GetFieldByInternalName(Consts.DepartmentsListFields.BU).Id];
                            properties.AfterProperties[Consts.OrdersListFields.CostCenter] = item[item.Fields.GetFieldByInternalName(Consts.DepartmentsListFields.CostCenter).Id];
                            properties.AfterProperties[Consts.OrdersListFields.ChiefManager] = item[item.Fields.GetFieldByInternalName(Consts.DepartmentsListFields.Manager).Id];
                            if (String.IsNullOrEmpty(order_status))
                                properties.AfterProperties[Consts.OrdersListFields.Status] = new SPFieldLookupValue((int)Enums.OrderStatus.Draft, draft_status_name);
                        }
                    });
                }
            }
            else if (CommonHelper.OrderClosed(order_status))
            {
                properties.AfterProperties[Consts.OrdersListFields.ClosedDate] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.UtcNow);
            }
            else if (CommonHelper.OrderCanceled(order_status))
            {
                properties.AfterProperties[Consts.OrdersListFields.CanceledDate] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.UtcNow);
            }
            else if (CommonHelper.OrderRejected(order_status))
            {
                properties.AfterProperties[Consts.OrdersListFields.RejectedDate] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.UtcNow);
            }
        }

        private void UpdateStoreItemStatistic(SPItemEventProperties properties)
        {
            /*
                    Склад (StoreItems):
                    Поле «Заказано раз» обновляем в момент отправки заявки в работу для всех позиций  заказа.
                    Поле «Зарезервировано» при отправке заявки в работу, значение данного поля должно увеличиться на величину количества, указанного в заявке для соответствующей канцелярской принадлежности.
                    При закрытии заявки или отмены, значение поля «Зарезервировано» должно быть уменьшено на величину количества, указанного в заявке для соответствующей канцелярской принадлежности.                     
                    Зарезервированные товары должны быть списаны после перевода заявки в статус «Закрыта». 
                    После расформирования заявки, зарезервированные товары должны быть списаны из резерва, т.е. 
                   должен быть произведен возврат зарезервированного объема в остатки по складу.
                    */

            string order_status_old = StringHelper.CheckForNull(properties.ListItem[Consts.OrdersListFields.Status]);
            string order_status_new = StringHelper.CheckForNull(properties.AfterProperties[Consts.OrdersListFields.Status]);
            if (!CommonHelper.SameStatuses(order_status_old, order_status_new))
            {
                if (CommonHelper.OrderSentToProcess(order_status_old, order_status_new))
                {
                    HandleStoreItem(properties.SiteId, properties.RelativeWebUrl, properties.ListItemId, (storeItem, ordered_number) =>
                    {
                        object o_reserved = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Reserved).Id];
                        string s_reserved = StringHelper.CheckForNull(o_reserved);
                        int i_reserved = 0;
                        Int32.TryParse(s_reserved, out i_reserved);
                        storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Reserved).Id] = i_reserved + ordered_number;

                        object o_orderedNumber = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.OrderedNumber).Id];
                        string s_orderedNumber = StringHelper.CheckForNull(o_orderedNumber);
                        int i_orderedNumber = 0;
                        Int32.TryParse(s_orderedNumber, out i_orderedNumber);
                        i_orderedNumber++;
                        storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.OrderedNumber).Id] = i_orderedNumber;

                        storeItem.Update();
                    });                    
                }
                else if (CommonHelper.OrderCanceled(order_status_new) || CommonHelper.OrderRejected(order_status_new) || CommonHelper.OrderClosed(order_status_new))
                {
                    HandleStoreItem(properties.SiteId, properties.RelativeWebUrl, properties.ListItemId, (storeItem, ordered_number) =>
                    {
                        object o_reserved = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Reserved).Id];
                        string s_reserved = StringHelper.CheckForNull(o_reserved);
                        int i_reserved = 0;
                        Int32.TryParse(s_reserved, out i_reserved);
                        storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Reserved).Id] = i_reserved - ordered_number;

                        object o_store_number = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Number).Id];
                        string s_store_number = StringHelper.CheckForNull(o_store_number);
                        int i_store_number = 0;
                        Int32.TryParse(s_store_number, out i_store_number);

                        if (CommonHelper.OrderClosed(order_status_new))
                        {                            
                            storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Number).Id] = i_store_number - ordered_number;
                        }

                        storeItem.Update();
                    }); 
                }                
            }
        }

        private void HandleStoreItem(Guid SiteId, string RelativeWebUrl, int listItemId, Action<SPListItem, int> a)
        {
            int orderId = listItemId;
            using (SPSite ElevatedSite = SecurityHelper.GetElevatedSite(SiteId))
            {
                using (SPWeb ElevatedWeb = ElevatedSite.OpenWeb(RelativeWebUrl))
                {
                    ElevatedWeb.TryUsingList(Consts.ListUrl.ORDERITEMS, (orderItemsList) =>
                    {
                        string s_orderId = orderId.ToString();
                        SPQuery query = new SPQuery();
                        var expressions = new List<Expression<Func<SPListItem, bool>>>();
                        expressions.Add(x => x[Consts.OrderItemsListFields.OrderNumber] == (DataTypes.LookupId)s_orderId);
                        query.ViewFields = Camlex.Query().ViewFields(new string[] { Consts.OrderItemsListFields.Article, Consts.OrderItemsListFields.Number, "PermMask" });
                        query.Query = Camlex.Query().WhereAny(expressions).ToString();
                        SPListItemCollection orderItems = orderItemsList.GetItems(query);
                        ElevatedWeb.TryUsingList(Consts.ListUrl.STOREITEMS, (storeItemsList) =>
                        {
                            foreach (SPListItem orderItem in orderItems)
                            {
                                string reserved_article = StringHelper.CheckForNull(orderItem[Consts.OrderItemsListFields.Article]);
                                string ordered_number = StringHelper.CheckForNull(orderItem[Consts.OrderItemsListFields.Number]);
                                int i_ordered_amount = 0;
                                if (!String.IsNullOrEmpty(reserved_article) && Int32.TryParse(ordered_number, out i_ordered_amount))
                                {
                                    int articleId = new SPFieldLookupValue(reserved_article).LookupId;
                                    SPListItem storeItem = storeItemsList.GetItemById(articleId);
                                    if (storeItem != null)
                                    {
                                        a(storeItem, i_ordered_amount);
                                    }
                                }
                            }
                        });
                    });
                }
            }
        }       

        private StoreItemsAvailabilityResult CheckStoreItemsAvailability(SPItemEventProperties properties)
        {
            StoreItemsAvailabilityResult result = new StoreItemsAvailabilityResult();
            string order_status_old = StringHelper.CheckForNull(properties.ListItem[Consts.OrdersListFields.Status]);
            string order_status_new = StringHelper.CheckForNull(properties.AfterProperties[Consts.OrdersListFields.Status]);
            if (!CommonHelper.SameStatuses(order_status_old, order_status_new))
            {
                if (CommonHelper.OrderSentToProcess(order_status_old, order_status_new))
                {
                    HandleStoreItem(properties.SiteId, properties.RelativeWebUrl, properties.ListItemId, (storeItem, ordered_amount) =>
                    {
                        if (storeItem.ContentTypeId.IsChildOf(Consts.CT_STOREITEM))
                        {
                            string article_tite = StringHelper.CheckForNull(storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Title).Id]);
                            object o_number = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Number).Id];
                            string s_number = StringHelper.CheckForNull(o_number);
                            int i_number = 0;
                            Int32.TryParse(s_number, out i_number);

                            o_number = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.ReserveBalance).Id];
                            s_number = StringHelper.CheckForNull(o_number);
                            int i_reserveBalance = 0;
                            Int32.TryParse(s_number, out i_reserveBalance);

                            o_number = storeItem[storeItem.Fields.GetFieldByInternalName(Consts.StoreItemsListFields.Reserved).Id];
                            s_number = StringHelper.CheckForNull(o_number);
                            int i_reserved = 0;
                            Int32.TryParse(s_number, out i_reserved);

                            result.ItemResults.Add(new StoreItemAvailabilityResult() { Title = article_tite, OrderedNumber = ordered_amount, StoreNumber = i_number - i_reserveBalance - i_reserved });
                        }
                    });   
                }
            }
            return result;
        }

        private void AddLogForNewOrder(SPItemEventProperties properties)
        {
            string order_number = StringHelper.CheckForNull(properties.AfterProperties["Title"]);
            string order_status = StringHelper.CheckForNull(properties.AfterProperties[Consts.OrdersListFields.Status]);

            CommonHelper.AddOrderLog(properties.SiteId, properties.RelativeWebUrl,
            new SPFieldLookupValue(properties.ListItemId, order_number),
            null,
            new SPFieldLookupValue(order_status),
            new SPFieldUserValue(properties.Web, properties.CurrentUserId, properties.UserDisplayName),
            String.Empty);
        }

        private void AddLogForExistingOrder(SPItemEventProperties properties)
        {
            string order_number = StringHelper.CheckForNull(properties.ListItem["Title"]);
            string order_status_old = StringHelper.CheckForNull(properties.ListItem[Consts.OrdersListFields.Status]);
            string order_status_new = StringHelper.CheckForNull(properties.AfterProperties[Consts.OrdersListFields.Status]);
            
            string comment = String.Empty;

            if (!CommonHelper.SameStatuses(order_status_old, order_status_new))
            {
                if (!CommonHelper.OrderSentToApproval(order_status_old) && !CommonHelper.OrderRejected(order_status_new) &&
                    !CommonHelper.OrderCanceled(order_status_new))
                {
                    if (CommonHelper.OrderIsDraft(order_status_old) && !CommonHelper.OrderIsDraft(order_status_new))
                        comment = StringHelper.CheckForNull(properties.AfterProperties[Consts.OrdersListFields.Comments]);
                    CommonHelper.AddOrderLog(properties.SiteId, properties.RelativeWebUrl,
                    new SPFieldLookupValue(properties.ListItemId, order_number),
                    new SPFieldLookupValue(order_status_old),
                    new SPFieldLookupValue(order_status_new),
                    new SPFieldUserValue(properties.Web,properties.CurrentUserId,properties.UserDisplayName),
                    comment);
                }
            }
        }        
    }
}
