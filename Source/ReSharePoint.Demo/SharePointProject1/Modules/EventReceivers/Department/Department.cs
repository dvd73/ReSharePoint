using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using CamlexNET;
using Microsoft.SharePoint;

namespace Novartis.StatOrd.Modules.EventReceivers
{
    [Guid("4040504A-029C-4FA2-911B-6342DCCD40DB")]
    public class DepartmentEventReceiver : SPItemEventReceiver
    {
       /// <summary>
       /// An item was added.
       /// </summary>
       public override void ItemAdded(SPItemEventProperties properties)
       {
       }

       /// <summary>
       /// An item was updated.
       /// </summary>
       public override void ItemUpdated(SPItemEventProperties properties)
       {
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
           
       }

       
       
    }
}
