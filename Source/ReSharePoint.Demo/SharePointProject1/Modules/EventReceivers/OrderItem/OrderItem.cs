using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;

namespace Novartis.StatOrd.Modules.EventReceivers
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
        }

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
            this.EventFiringEnabled = false;
            HandleAddedUpdated(properties);
            this.EventFiringEnabled = true;
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
        }

        private void HandleAddingUpdating(SPItemEventProperties properties)
        {
        }

        private void HandleAddedUpdated(SPItemEventProperties properties)
        {
        }
    }
}
