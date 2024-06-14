using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Caches;
using JetBrains.Util;
using ReSharePoint.Common;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.Components.Solution.ProjectFileCache
{
    [SolutionInstanceComponent]
    public class SharePointProjectItemsSolutionProvider : SPProjectFileItemCollectionDataProvider<SharePointProjectItem>
    {
        public SharePointProjectItemsSolutionProvider(Lifetime lifetime, ISolution solution, IProjectFileDataCache cache)
            : base(lifetime, solution, cache)
        {
        }

        public override ICollection<SharePointProjectItem> BuildData(FileSystemPath projectFileLocation, XmlDocument doc)
        {
            List<SharePointProjectItem> result = new List<SharePointProjectItem>();

            XDocument document = doc.ToXDocument();
            IEnumerable<XElement> sharePointProjectItemNodes =
                document.Descendants().Where(p => p.Name.LocalName == "SharePointProjectItemId");
            foreach (var node in sharePointProjectItemNodes)
            {
                result.Add(new SharePointProjectItem()
                {
                    Id = Guid.Parse(node.Value),
                    Include = Path.Combine(ProjectLocation, node.Parent.Attribute("Include").Value)
                });
            }
            
            return result;
        }

        public override int Version => 4;
    }

    [Serializable()]
    public class SharePointProjectItem : SPProjectFileCacheItem
    {
        public Guid Id { get; set; }
        public string Path { get; private set; }
        public SharePointProjectItemType ItemType { get; set; }
        public string ElementManifest { get; private set; }
        public string SupportedDeploymentScopes { get; private set; }
        public string Target { get; private set; }
        public string Include 
        {
            set
            {
                Path = value.Replace("\\SharePointProjectItem.spdata", String.Empty);

                if (File.Exists(value))
                {
                    using (FileStream stream = File.Open(value, FileMode.Open, FileAccess.Read))
                    {
                        XDocument document = XDocument.Load(stream);
                        XElement element = document.Descendants()
                            .FirstOrDefault(p => p.Name.LocalName == "ProjectItem");
                        if (element != null)
                        {
                            XElement elementManifest = element.Descendants().FirstOrDefault(p =>
                                p.Name.LocalName == "ProjectItemFile" && p.HasAttributes &&
                                p.Attribute("Type").Value == "ElementManifest");
                            if (elementManifest != null)
                                ElementManifest = elementManifest.Attribute("Source").Value;
                            SupportedDeploymentScopes = element.Attribute("SupportedDeploymentScopes").Value;
                            switch (element.Attribute("Type").Value)
                            {
                                case "Microsoft.VisualStudio.SharePoint.ContentType":
                                    ItemType = SharePointProjectItemType.ContentType;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.ListInstance":
                                    ItemType = SharePointProjectItemType.ListInstance;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.ListDefinition":
                                    ItemType = SharePointProjectItemType.ListDefinition;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.Field":
                                    ItemType = SharePointProjectItemType.Field;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.WebPart":
                                    ItemType = SharePointProjectItemType.WebPart;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.EventHandler":
                                    ItemType = SharePointProjectItemType.EventHandler;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.Module":
                                    ItemType = SharePointProjectItemType.Module;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.MappedFolder":
                                    ItemType = SharePointProjectItemType.MappedFolder;
                                    XElement el = element.Descendants().FirstOrDefault(p =>
                                        p.Name.LocalName == "ProjectItemFolder" && p.HasAttributes &&
                                        p.Attribute("Type").Value == "TemplateFile");
                                    Target = el != null ? el.Attribute("Target").Value : String.Empty;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.Workflow":
                                case "Microsoft.VisualStudio.SharePoint.Workflow4":
                                    ItemType = SharePointProjectItemType.Workflow;
                                    break;
                                case "Microsoft.VisualStudio.SharePoint.Workflow4CustomActivity":
                                    ItemType = SharePointProjectItemType.CustomActivity;
                                    break;
                                case "CKS.Dev.SharePoint.CustomAction":
                                    ItemType = SharePointProjectItemType.CustomAction;
                                    break;
                                case "CKS.Dev.SharePoint.CustomActionGroup":
                                    ItemType = SharePointProjectItemType.CustomActionGroup;
                                    break;
                                case "CKS.Dev.SharePoint.Branding":
                                    ItemType = SharePointProjectItemType.Branding;
                                    break;
                                case "CKS.Dev.SharePoint.MasterPage":
                                    ItemType = SharePointProjectItemType.MasterPage;
                                    break;
                                default:
                                    ItemType = SharePointProjectItemType.GenericElement;
                                    break;
                            }
                        }
                    }
                }
            } 
        }
    }
}
