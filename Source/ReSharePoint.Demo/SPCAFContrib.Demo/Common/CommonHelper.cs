﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CamlexNET;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using MOSS.Common.Utilities;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;
using MOSS.Common.Controls;
using System.Web.UI;
using System.Data;
using System.Collections;
using Microsoft.SharePoint.Workflow;

namespace SPCAFContrib.Demo.Common
{
    public class CommonHelper
    {
        public static string GetOrderStatusTitle(SPWeb web, SPCAFContrib.Demo.Common.Enums.OrderStatus status)
        {
            string result = String.Empty;

            web.TryUsingList(Consts.ListUrl.STATUSES, (list) =>
            {
                SPListItem item = list.GetItemById((int)status);
                if (item != null)
                {
                    result = StringHelper.CheckForNull(item[item.Fields.GetFieldByInternalName(Consts.StatusesListFields.Name).Id]);
                }
            });

            return result;
        }

        public static Enums.OrderStatus GetOrderStatus(SPWeb web, string order_number)
        {
            Enums.OrderStatus result = Enums.OrderStatus.Draft;

            if (!String.IsNullOrEmpty(order_number))
            {
                int orderId = new SPFieldLookupValue(order_number).LookupId;
                web.TryUsingList(Consts.ListUrl.ORDERS, (list) =>
                {
                    SPListItem item = list.GetItemById(orderId);
                    if (item != null)
                    {
                        string order_status = StringHelper.CheckForNull(item[item.Fields.GetFieldByInternalName(Consts.OrdersListFields.Status).Id]);
                        if (!String.IsNullOrEmpty(order_status))
                        {
                            result = (Enums.OrderStatus)new SPFieldLookupValue(order_status).LookupId;
                        }
                    }
                });
            }

            return result;
        }

        public static bool SameStatuses(string order_status1, string order_status2)
        {
            bool result = true;
            if (!String.IsNullOrEmpty(order_status1) && !String.IsNullOrEmpty(order_status2))
            {
                SPFieldLookupValue v1 = new SPFieldLookupValue(order_status1);
                SPFieldLookupValue v2 = new SPFieldLookupValue(order_status2);
                result = v1.LookupId == v2.LookupId;
            }
            return result;
        }

        public static bool OrderSentToProcess(string order_status_old, string order_status_new)
        {
            return OrderIsDraft(order_status_old) &&
                (OrderSentToApproval(order_status_new) || (OrderIsAssembling(order_status_new)));
        }

        public static bool OrderIsDraft(string order_status)
        {
            bool result = true;
            if (!String.IsNullOrEmpty(order_status))
            {
                SPFieldLookupValue v = new SPFieldLookupValue(order_status);
                result = (int)Enums.OrderStatus.Draft == v.LookupId;
            }
            return result;
        }

        public static bool OrderSentToApproval(string order_status)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(order_status))
            {
                SPFieldLookupValue v = new SPFieldLookupValue(order_status);
                result = (int)Enums.OrderStatus.AdministratorApproval == v.LookupId ||
                    (int)Enums.OrderStatus.DepartmentManagerApproval == v.LookupId ||
                    (int)Enums.OrderStatus.CFOApproval == v.LookupId;
            }
            return result;
        }

        public static bool OrderIsAssembling(string order_status)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(order_status))
            {
                SPFieldLookupValue v = new SPFieldLookupValue(order_status);
                result = (int)Enums.OrderStatus.Assembling == v.LookupId;
            }
            return result;
        }

        public static bool OrderCanceled(string order_status)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(order_status))
            {
                SPFieldLookupValue v = new SPFieldLookupValue(order_status);
                result = (int)Enums.OrderStatus.Canceled == v.LookupId;
            }
            return result;
        }

        public static bool OrderRejected(string order_status)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(order_status))
            {
                SPFieldLookupValue v = new SPFieldLookupValue(order_status);
                result = (int)Enums.OrderStatus.Rejected == v.LookupId;
            }
            return result;
        }

        public static bool OrderClosed(string order_status)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(order_status))
            {
                SPFieldLookupValue v = new SPFieldLookupValue(order_status);
                result = (int)Enums.OrderStatus.Closed == v.LookupId;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="confirmed"></param>
        /// <param name="sourceUrl">this.Page.Request.QueryString["Source"]</param>
        public static void DialogFormCloseHandler(HttpContext ctx, bool confirmed, string sourceUrl)
        {
            if (SPContext.Current != null && SPContext.Current.IsPopUI)
            {
                ctx.Response.Clear();
                ctx.Response.Write(confirmed ? MOSS.Common.Code.Consts.COMMIT_POPUP_DIALOG2_SCRIPT : MOSS.Common.Code.Consts.CANCEL_POPUP_DIALOG_SCRIPT);
                ctx.Response.Flush();
                ctx.Response.End();
            }
            else
            {
                if (!String.IsNullOrEmpty(sourceUrl))
                {
                    RedirectToPage(sourceUrl, false, false, ctx);
                }
            }
        }

        /// <summary>
        /// Redirect the the users page
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="useSource"></param>
        public static void RedirectToPage(string pageUrl, bool useSource, bool isDialogMode, HttpContext context)
        {
            SPRedirectFlags flags = SPRedirectFlags.DoNotEncodeUrl | SPRedirectFlags.Static;
            if (useSource)
            {
                flags |= SPRedirectFlags.UseSource;
            }
            if (isDialogMode)
            {
                string urlRedirect;
                SPUtility.DetermineRedirectUrl(pageUrl, flags, context, null, out urlRedirect);
                context.Response.Write("<script type='text/javascript'>window.location.href='" + urlRedirect + "';</script>");
                context.Response.Flush();
                context.Response.End();
            }
            else
            {
                SPUtility.Redirect(pageUrl, flags, context);
            }
        }

        public static string GetFieldUrlValue(object field_value)
        {
            string result = UrlHelper.SharePointUrlToRelativeUrl(Consts.NO_IMAGE_PICTURE_URL);
            string f = StringHelper.CheckForNull(field_value);
            if (!String.IsNullOrEmpty(f))
            {
                result = new SPFieldUrlValue(f).Url;
            }

            return result;
        }

        public static string GetArticleSearchFields()
        {
            string result = String.Empty;
            foreach (string field in Consts.ARTICLE_SEARCH_FIELDS)
            {
                if (!String.IsNullOrEmpty(result)) result += ";";
                result += field;
            }
            return result;
        }

        public static string GetNextOrderIndex(SPWeb web)
        {
            return SiteSettingsHelper.HandleValue<int>(web, Consts.SS_ORDER_COUNT, (item) =>
            {
                object value = item[item.Fields.GetFieldByInternalName("Value").Id];

                if (value == null) value = 0;
                int order_counter = 0;
                int tries = 0;
                if (Int32.TryParse(value.ToString(), out order_counter))
                {
                    do
                    {
                        order_counter++;
                        tries++;
                    }
                    while (CheckOrderExistence(web, order_counter.ToString()) && tries < 50);

                    item[item.Fields.GetFieldByInternalName("Value").Id] = order_counter;
                    item.Update();
                }

                return order_counter;
            }).ToString();
        }

        private static bool CheckOrderExistence(SPWeb web, string order_counter)
        {
            bool result = false;
            web.TryUsingList(Consts.ListUrl.ORDERS, (list) =>
            {
                SPQuery query = new SPQuery();
                query.ViewFields = Camlex.Query().ViewFields(new string[] { "ID", "Title", "PermMask" });
                query.Query = Camlex.Query().Where(o => ((string)o[SPBuiltInFieldId.Title]).Contains(order_counter)).ToString();
                SPListItemCollection listItems = list.GetItems(query);
                result = listItems.Count > 0;
            });
            return result;
        }

        public static bool CheckIfUserIsAGroupMember(Guid SiteId, string WebRelativeUrl, int userId, string groupName)
        {
            bool results = false;
            
            SPWeb context_web = SPContext.Current.Web;

            using (SPSite site = SecurityHelper.GetElevatedSite(SiteId))
            {
                using (SPWeb web = site.OpenWeb(WebRelativeUrl))
                {
                    try
                    {
                        SPUser user = web.SiteGroups[groupName].Users.GetByID(userId);
                        if (user != null)
                            results = true;
                        else
                            results = false;
                    }
                    catch
                    {
                        results = false;
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// CFO
        /// </summary>
        public static bool IsCurrentUserCFO
        {
            get
            {
                try
                {
                    string groupName = SiteSettingsHelper.GetStringValue(SPContext.Current.Site.RootWeb, Consts.SS_GS_CFO, Consts.SS_DEFAULT_GS_CFO);
                    return CheckIfUserIsAGroupMember(SPContext.Current.Site.ID, SPContext.Current.Site.RootWeb.ServerRelativeUrl, SPContext.Current.Web.CurrentUser.ID, groupName);
                }
                catch
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Руководитель отдела/департамента
        /// </summary>
        public static bool IsCurrentUserDepartmentManager
        {
            get
            {
                try
                {
                    string groupName = SiteSettingsHelper.GetStringValue(SPContext.Current.Site.RootWeb, Consts.SS_GS_DEPARTMENT_ADMINISTRATOR, Consts.SS_DEFAULT_GS_DEPARTMENT_ADMINISTRATOR);
                    return CheckIfUserIsAGroupMember(SPContext.Current.Site.ID, SPContext.Current.Site.RootWeb.ServerRelativeUrl, SPContext.Current.Web.CurrentUser.ID, groupName);
                }
                catch
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Сотрудник Reception
        /// </summary>
        public static bool IsCurrentUserReception
        {
            get
            {
                try
                {
                    string groupName = SiteSettingsHelper.GetStringValue(SPContext.Current.Site.RootWeb, Consts.SS_GS_RECEPTION, Consts.SS_DEFAULT_GS_RECEPTION);
                    return CheckIfUserIsAGroupMember(SPContext.Current.Site.ID, SPContext.Current.Site.RootWeb.ServerRelativeUrl, SPContext.Current.Web.CurrentUser.ID, groupName);
                }
                catch
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Системный администратор
        /// </summary>
        public static bool IsCurrentUserSysAdmin
        {
            get
            {
                try
                {
                    string groupName = SiteSettingsHelper.GetStringValue(SPContext.Current.Site.RootWeb, Consts.SS_GS_SYSTEM_ADMINISTRATOR, Consts.SS_DEFAULT_GS_SYSTEM_ADMINISTRATOR);
                    return CheckIfUserIsAGroupMember(SPContext.Current.Site.ID, SPContext.Current.Site.RootWeb.ServerRelativeUrl, SPContext.Current.Web.CurrentUser.ID, groupName);
                }
                catch
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Административный менеджер
        /// </summary>
        public static bool IsCurrentUserAdministrativeManager
        {
            get
            {
                try
                {
                    string groupName = SiteSettingsHelper.GetStringValue(SPContext.Current.Site.RootWeb, Consts.SS_GS_ADMIN_MANAGER, Consts.SS_DEFAULT_GS_ADMIN_MANAGER);
                    return CheckIfUserIsAGroupMember(SPContext.Current.Site.ID, SPContext.Current.Site.RootWeb.ServerRelativeUrl, SPContext.Current.Web.CurrentUser.ID, groupName);
                }
                catch
                {
                    return false;
                }

            }
        }

        public static bool IsOrderStatusOver(SPWeb web, int orderId, int statusId, int countDays)
        {
            bool result = false;

            SPQuery query = new SPQuery();
            query.ViewFields = string.Concat("<FieldRef Name='ID' />", "<FieldRef Name='Demo_OrderItemOrder' />", "<FieldRef Name='Demo_OrderLogDate' />");
            query.ViewFieldsOnly = true;
            query.RowLimit = 1;
            query.Query = @"   <Where>
                                     <And>
                                        <Eq>
                                           <FieldRef Name='Demo_OrderItemOrder' LookupId='True' />
                                           <Value Type='Lookup'>" + orderId.ToString() + @"</Value>
                                        </Eq>
                                        <Eq>
                                           <FieldRef Name='Demo_OrderStatus' LookupId='True' />
                                           <Value Type='Lookup'>" + statusId.ToString() + @"</Value>
                                        </Eq>
                                     </And>
                               </Where><OrderBy><FieldRef Name=""Demo_OrderLogDate"" Ascending=""False"" /></OrderBy>";

            web.TryUsingList(Consts.ListUrl.ORDERLOG, (listOrderLog) =>
            {
                SPListItemCollection orderLogItems = listOrderLog.GetItems(query);
                SPField fldDemo_OrderLogDate = listOrderLog.Fields.GetFieldByInternalName(Consts.OrderLogListFields.EventDate);

                if (orderLogItems.Count > 0 && orderLogItems[0][fldDemo_OrderLogDate.Id] != null)
                {
                    DateTime orderLogDate = DateTimeHelper.CheckForNull(orderLogItems[0][fldDemo_OrderLogDate.Id], DateTime.MinValue);
                    DateTime date = orderLogDate;
                    int count = 0;
                    while (date < DateTime.Now)
                    {
                        date = date.AddDays(1);
                        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                            count++;
                        if (count >= countDays) break;
                    }

                    result = count >= countDays;
                }
            });

            return result;
        }

        public static Double GetCalculatedCurrencyFieldValue(SPListItem item, string field_name)
        {
            SPFieldCalculated cf = (SPFieldCalculated)item.Fields.GetFieldByInternalName(field_name);
            if (cf.OutputType == SPFieldType.Currency)
                return GetCurrencyFieldValue(item.GetFormattedValue(field_name), field_name, cf.CurrencyLocaleId);
            else
                return 0;
        }

        public static Double GetCurrencyFieldValue(string item_value, string field_name, int field_localeId)
        {
            //string s_value = StringHelper.CheckForNull(item[item.Fields.GetFieldByInternalName(field_name).Id]);
            Double d_value = 0;
            CultureInfo ci_field = new CultureInfo(field_localeId);
            CultureInfo ci_eng = new CultureInfo(1033);
            CultureInfo ci_current = CultureInfo.CurrentCulture;

            if (Double.TryParse(item_value, NumberStyles.Any, ci_field.NumberFormat, out d_value))
            {
                return d_value;
            }
            else if (Double.TryParse(item_value, NumberStyles.Any, ci_current.NumberFormat, out d_value))
            {
                return d_value;
            }
            else if (Double.TryParse(item_value, NumberStyles.Any, ci_eng.NumberFormat, out d_value))
            {
                return d_value;
            }
            else
            {
                string digits = Regex.Replace(item_value, "[^0-9.,]", "");
                if (digits[digits.Length - 1] == '.')
                    digits = digits.Remove(digits.Length - 1, 1);
                if (!String.IsNullOrEmpty(digits))
                    return Convert.ToDouble(digits);
                else
                    return 0;
            }
        }

        public static void DeleteOrder(SPWeb web)
        {
            // сперва удаляем товары
            web.TryUsingList(Consts.ListUrl.ORDERITEMS, (list) =>
            {
                string s_orderId = SPContext.Current.ItemId.ToString();
                SPQuery query = new SPQuery();
                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                expressions.Add(x => x[Consts.OrderItemsListFields.OrderNumber] == (DataTypes.LookupId)s_orderId);
                query.Query = Camlex.Query().WhereAny(expressions).ToString();
                SPListItemCollection listItems = list.GetItems(query);

                if (listItems.Count > 0)
                {
                    for (int k = listItems.Count - 1; k >= 0; k--)
                    {
                        listItems.Delete(k);
                    }
                }
            });

            // затем сам заказ
            web.TryUsingList(Consts.ListUrl.ORDERS, (list) =>
            {
                SPQuery query = new SPQuery();
                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                expressions.Add(x => (int)x["ID"] == SPContext.Current.ItemId);
                query.Query = Camlex.Query().WhereAny(expressions).ToString();
                SPListItemCollection listItems = list.GetItems(query);

                if (listItems.Count > 0)
                {
                    for (int k = listItems.Count - 1; k >= 0; k--)
                    {
                        listItems.Delete(k);
                    }
                }
            });
        }

        public static void AddOrderLog(SPFieldLookupValue order_number, SPFieldLookupValue old_status, SPFieldLookupValue new_status, SPFieldUserValue user, string description)
        {
            using (SPSite ElevatedSite = SecurityHelper.GetCurrentSiteAsElevated())
            {
                using (SPWeb ElevatedWeb = ElevatedSite.OpenWeb())
                {
                    AddNewOrderLogRecord(order_number, old_status, new_status, user, description, ElevatedWeb);
                }
            }
        }

        public static void AddOrderLog(Guid SiteId, String RelativeWebUrl, SPFieldLookupValue order_number, SPFieldLookupValue old_status, SPFieldLookupValue new_status, SPFieldUserValue user, string description)
        {
            using (SPSite ElevatedSite = SecurityHelper.GetElevatedSite(SiteId))
            {
                using (SPWeb ElevatedWeb = ElevatedSite.OpenWeb(RelativeWebUrl))
                {
                    AddNewOrderLogRecord(order_number, old_status, new_status, user, description, ElevatedWeb);
                }
            }
        }

        private static void AddNewOrderLogRecord(SPFieldLookupValue order_number, SPFieldLookupValue old_status, SPFieldLookupValue new_status, SPFieldUserValue user, string description, SPWeb ElevatedWeb)
        {
            ElevatedWeb.TryUsingList(Consts.ListUrl.ORDERLOG, (list) =>
            {
                if (order_number != null && new_status != null)
                {
                    string s_orderId = order_number.LookupId.ToString();
                    string s_status = new_status.LookupId.ToString();
                    SPQuery query = new SPQuery();
                    query.RowLimit = 1;
                    var expressions = new List<Expression<Func<SPListItem, bool>>>();
                    expressions.Add(x => x[Consts.OrderLogListFields.OrderNumber] == (DataTypes.LookupId)s_orderId);
                    expressions.Add(x => x[Consts.OrderLogListFields.Status] == (DataTypes.LookupId)s_status);
                    query.Query = Camlex.Query().WhereAll(expressions).ToString();
                    SPListItemCollection listItems = list.GetItems(query);
                    if (listItems.Count == 1)
                    {
                        return;
                    }
                }

                SPListItem new_item = list.AddItem();
                new_item[Consts.OrderLogListFields.OrderNumber] = order_number;
                new_item[Consts.OrderLogListFields.EventDate] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.UtcNow);
                new_item[Consts.OrderLogListFields.Person] = user;
                if (old_status != null)
                    new_item[Consts.OrderLogListFields.PrevStatus] = old_status;
                new_item[Consts.OrderLogListFields.Status] = new_status;
                new_item[Consts.OrderLogListFields.Description] = description;
                new_item.Update();

                if (order_number == null)
                    MOSSLogger.Instance.Trace("Попытка занести в историю заявок запись без указания номера заказа.");
            });
        }

        public static void EnsureSecurityGroup(SPWeb web, string groupName, string groupDescription, SPRoleDefinition role)
        {
            if (!String.IsNullOrEmpty(groupName) && !PermissionHelper.Instance.CheckSiteGroup(web, groupName))
            {
                SPGroup newGroup = PermissionHelper.Instance.AddSiteGroup(web, groupName, groupDescription);
                SPRoleAssignment roleAssignment = new SPRoleAssignment(newGroup);
                roleAssignment.RoleDefinitionBindings.Add(role);
                web.RoleAssignments.Add(roleAssignment);
                web.Update();
            }
        }

        /// <summary>
        /// Какие из указанных товаров принадлежат заказу
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="articleIds"></param>
        /// <returns></returns>
        public static int[] CheckArticlesBelongToOrder(int orderId, IEnumerable<int> articleIds)
        {
            int[] result = new int[] { };

            SPContext.Current.Web.TryUsingList(Consts.ListUrl.ORDERITEMS, (list) =>
            {
                string s_orderId = orderId.ToString();
                SPQuery query = new SPQuery();
                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                foreach (int articleId in articleIds)
                {
                    string s_article_id = articleId.ToString();
                    expressions.Add(x => x[Consts.OrderItemsListFields.Article] == (DataTypes.LookupId)s_article_id);
                }
                query.Query = Camlex.Query().WhereAny(expressions).ToString();
                query.Query = Camlex.Query().WhereAll(query.Query, x => x[Consts.OrderItemsListFields.OrderNumber] == (DataTypes.LookupId)s_orderId).ToString();
                SPListItemCollection orderItems = list.GetItems(query);
                if (orderItems.Count > 0)
                {
                    result = orderItems.OfType<SPListItem>().Select(i => new SPFieldLookupValue(StringHelper.CheckForNull(i[Consts.OrderItemsListFields.Article])).LookupId).ToArray();
                }
            });

            return result;
        }

        /// <summary>
        /// Все товары заказа
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static int[] GetOrderItems(int orderId)
        {
            int[] result = new int[] { };

            SPContext.Current.Web.TryUsingList(Consts.ListUrl.ORDERITEMS, (list) =>
            {
                string s_orderId = orderId.ToString();
                SPQuery query = new SPQuery();
                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                expressions.Add(x => x[Consts.OrderItemsListFields.OrderNumber] == (DataTypes.LookupId)s_orderId);
                query.Query = Camlex.Query().WhereAll(expressions).ToString();
                SPListItemCollection orderItems = list.GetItems(query);
                if (orderItems.Count > 0)
                {
                    result = orderItems.OfType<SPListItem>().Select(i => new SPFieldLookupValue(StringHelper.CheckForNull(i[Consts.OrderItemsListFields.Article])).LookupId).ToArray();
                }
            });

            return result;
        }

        public static object GetActualSPItemEventProperty(SPItemEventProperties properties, string field_name)
        {
            switch (properties.EventType)
            {
                case SPEventReceiverType.ItemUpdating:
                    {
                        object o1 = properties.ListItem[field_name];
                        object o2 = properties.AfterProperties[field_name];

                        if (o2 == null)
                            return o1;
                        else if (o2 != null && o1 != null && !o2.Equals(o1))
                            return o2;
                        else
                            return o1;
                    }
                case SPEventReceiverType.ItemUpdated:
                case SPEventReceiverType.ItemAdded:
                    return properties.ListItem[field_name];
                default:
                    return properties.AfterProperties[field_name];
            }
        }

        public static double GetOrderSum(SPWeb web, int orderId)
        {
            Double result = 0;

            web.TryUsingList(Consts.ListUrl.ORDERITEMS, (list) =>
            {
                string s_orderId = orderId.ToString();
                SPQuery query = new SPQuery();
                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                expressions.Add(x => x[Consts.OrderItemsListFields.OrderNumber] == (DataTypes.LookupId)s_orderId);
                query.ViewFields = Camlex.Query().ViewFields(new string[] { Consts.OrderItemsListFields.Sum });
                query.Query = Camlex.Query().WhereAny(expressions).ToString();
                SPListItemCollection listItems = list.GetItems(query);

                result = listItems.Cast<SPListItem>().Sum(item => CommonHelper.GetCalculatedCurrencyFieldValue(item, Consts.OrderItemsListFields.Sum));
            });

            return result;
        }

        public static int GetDepartmentManager(int depId)
        {
            int result = 0;
            SPContext.Current.Web.TryUsingList(Consts.ListUrl.DEPARTMENTS, (list) =>
            {
                SPQuery query = new SPQuery();
                query.ViewFields = Camlex.Query().ViewFields(new string[] { "ID", Consts.DepartmentsListFields.Manager, "PermMask" });
                query.Query = Camlex.Query().Where(o => (int)o["ID"] == depId).ToString();
                SPListItemCollection listItems = list.GetItems(query);
                if (listItems.Count > 0)
                {
                    result = new SPFieldUserValue(list.ParentWeb, StringHelper.CheckForNull(listItems[0][Consts.DepartmentsListFields.Manager])).LookupId;
                }
            });

            return result;
        }

        public static void SetErrorMsg(string msg, Control ctl)
        {
            IssueInformerUserControl ctlIssueMessage = ctl as IssueInformerUserControl;
            ctlIssueMessage.IssueMessage = msg;
            string last_error = String.IsNullOrEmpty(Logger.Instance.LastException) ? MOSSLogger.Instance.LastException : Logger.Instance.LastException;
            SPUser user = UserHelper.Instance.CurrentUser;
            if (user != null)
                last_error += "<br/> <strong>Current user:</strong> " + user.Name + "(" + user.LoginName + ")";
            ctlIssueMessage.IssueDetails = last_error.Replace(Environment.NewLine, "<br/>");
            ctlIssueMessage.Visible = true;
        }

        public static void FillItemPermissions(DataTable dataSource, IEnumerable items)
        {
            if (dataSource != null)
            {
                if (!dataSource.Columns.Contains("AllowEdit"))
                    dataSource.Columns.Add("AllowEdit", typeof(Boolean));
                if (!dataSource.Columns.Contains("AllowView"))
                    dataSource.Columns.Add("AllowView", typeof(Boolean));

                foreach (SPListItem item in items)
                {
                    int id = Convert.ToInt32(item["ID"]);
                    DataRow[] rows = dataSource.Select(String.Format("ID = {0}", id));
                    if (rows != null && rows.Length == 1)
                    {
                        DataRow row = rows[0];
                        row["AllowEdit"] = item.DoesUserHavePermissions(SPBasePermissions.EditListItems);
                        row["AllowView"] = item.DoesUserHavePermissions(SPBasePermissions.ViewListItems);
                    }
                }
            }
        }

        public static void StartWorkFlow(SPSite site, SPList list, SPListItem item, string loginName)
        {
            if (loginName.ToUpper() == "SHAREPOINT\\SYSTEM")
            {
                SPWorkflowManager workflowManager = site.WorkflowManager;
                SPWorkflowAssociationCollection workflowAssociations = list.WorkflowAssociations;
                foreach (SPWorkflowAssociation workflowAssociation in workflowAssociations)
                {
                    if (workflowAssociation.AutoStartCreate || workflowAssociation.AutoStartChange)
                    {
                        workflowManager.StartWorkflow(item, workflowAssociation, workflowAssociation.AssociationData);
                    }
                }
            }
        }

        public static SPWeb GetSPWeb(string url)
        {
            using (SPSite site = new SPSite(url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    return web;
                }
            }
        }

        public static SPList GetListByListTemplateId(SPWeb web, int listTemplateId)
        {
            SPListTemplate listTemplateCustomer = web.ListTemplates.OfType<SPListTemplate>().Where(template => template.Type_Client == listTemplateId).FirstOrDefault();
            if (listTemplateCustomer == null) return null;
            return web.Lists.Cast<SPList>().Where(lst => lst.BaseTemplate.Equals(listTemplateCustomer.Type)).FirstOrDefault();
        }
    }
}
