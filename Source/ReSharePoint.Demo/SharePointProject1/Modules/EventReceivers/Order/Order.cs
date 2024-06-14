using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using CamlexNET;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Novartis.StatOrd.Modules.EventReceivers
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
           
        }
        
        public override void ItemDeleting(SPItemEventProperties properties)
        {
           
            
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            
        }

        public override void ItemDeleted(SPItemEventProperties properties)
        {
        }

        private void UpdateLinkedFields(SPItemEventProperties properties)
        {
            
        }

    }
}
