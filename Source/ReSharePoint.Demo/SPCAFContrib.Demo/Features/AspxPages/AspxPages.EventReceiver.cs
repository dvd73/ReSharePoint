using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml;
using CamlexNET;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing;
using MOSS.Common.Utilities;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.Features.AspxPages
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("15e594b9-28bb-4b5c-b9e3-b622bd247541")]
    public class AspxPagesEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            UpdateWebConfigClass.Do(properties.Feature.Parent as SPWeb);
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb web = properties.Feature.Parent as SPWeb;

            // Removes the Feature Web Parts from the Web Part Catalog on the site collection. 

            // Find the Web Part names from the Elements collection
            List<string> pages = new List<string>();
            SPElementDefinitionCollection elementColletion = properties.Definition.GetElementDefinitions(CultureInfo.CurrentCulture);
            foreach (SPElementDefinition element in elementColletion)
            {
                foreach (XmlElement xmlNode in element.XmlDefinition.ChildNodes)
                {
                    if (xmlNode.Name.Equals("File"))
                    {
                        pages.Add(xmlNode.Attributes["Url"].Value);
                    }
                }
            }

            if (pages.Count > 0)
            {
                // Get the Web Part Catalog
                PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
                SPList pagesList = publishingWeb.PagesList;

                SPQuery query = new SPQuery();
                var expressions = new List<Expression<Func<SPListItem, bool>>>();
                foreach (string pageUrl in pages)
                {
                    string p = pageUrl;
                    expressions.Add(x => ((string)x["FileLeafRef"]).Contains(p));
                }
                query.Query = Camlex.Query().WhereAny(expressions).Where(x => (string)x["Status"] == "Completed").ToString(); ; // Camlex.Query().Where(x => ((string)x["FileLeafRef"]).Contains(".aspx")).ToString();
                query.ViewAttributes = "Scope='RecursiveAll'";
                SPListItemCollection listItems = pagesList.GetItems(query);
                web.AllowUnsafeUpdates = true;
                for (int i = listItems.Count - 1; i >= 0; i--)
                    listItems[i].Delete();
                pagesList.Update();
                web.AllowUnsafeUpdates = false;
            }
        
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
