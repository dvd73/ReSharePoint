using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using SharePoint.Common.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq.Expressions;
using CamlexNET;
using SharePoint.Common.Utilities;
using MOSS.Common.Utilities;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.Modules.EventReceivers
{
    [Guid("4040504A-029C-4FA2-911B-6342DCCD40DB")]
    public class DepartmentEventReceiver : SPItemEventReceiver
    {
       /// <summary>
       /// An item was added.
       /// </summary>
       public override void ItemAdded(SPItemEventProperties properties)
       {
           CheckSecurityGroup(properties);
       }

       /// <summary>
       /// An item was updated.
       /// </summary>
       public override void ItemUpdated(SPItemEventProperties properties)
       {
           CheckSecurityGroup(properties);
       }

       /// <summary>
       /// An item was deleted.
       /// </summary>
       public override void ItemDeleted(SPItemEventProperties properties)
       {
           base.ItemDeleted(properties);
       }


       public override void ItemDeleting(SPItemEventProperties properties)
       {
           // check linked orders
           properties.Web.TryUsingList(Consts.ListUrl.ORDERS, (list) =>
           {
               string s_departmentId = properties.ListItemId.ToString();
               SPQuery query = new SPQuery();
               var expressions = new List<Expression<Func<SPListItem, bool>>>();
               expressions.Add(x => x[Consts.OrdersListFields.Department] == (DataTypes.LookupId)s_departmentId);
               query.Query = Camlex.Query().WhereAny(expressions).ToString();
               SPListItemCollection listItems = list.GetItems(query);

               if (listItems.Count > 0)
               {
                   properties.ErrorMessage = String.Format(Consts.WARNING3, listItems.Count);
                   properties.Cancel = true;
               }
           });  
       }

       private void CheckSecurityGroup(SPItemEventProperties properties)
       {
           // надо добавить руководителя департамента в группу безопасности
           SPGroup group = PermissionHelper.Instance.GetGroupByName(properties.Web, new PortalGroups(properties.Web)[Enums.SecurityGroup.DepartmentManager].Name);
           string dep_manager = StringHelper.CheckForNull(properties.ListItem[Consts.DepartmentsListFields.Manager]);

           if (!String.IsNullOrEmpty(dep_manager) && group != null)
           {
               using (SPSite site = SecurityHelper.GetElevatedSite(properties.SiteId))
               {
                   using (SPWeb web = site.OpenWeb(properties.RelativeWebUrl))
                   {
                        SPFieldUserValue dm = new SPFieldUserValue(web, dep_manager);
                        if (!CommonHelper.CheckIfUserIsAGroupMember(properties.SiteId, properties.RelativeWebUrl, dm.LookupId, group.Name))
                        {
                            web.SiteGroups[group.Name].AddUser(dm.User);
                        }
                    }
               }
           }
       }
       
    }
}
