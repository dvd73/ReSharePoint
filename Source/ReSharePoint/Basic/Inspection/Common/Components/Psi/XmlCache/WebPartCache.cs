using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.changes;
using JetBrains.DataFlow;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Serialization;
using JetBrains.Util.PersistentMap;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache
{
    [PsiComponent]
    [Applicability(
       IDEProjectType.SPFarmSolution  |
       IDEProjectType.SPSandbox )]
    public class WebPartCache : SPXmlEntityCache<WebPartXmlEntity>
    {
        #region Properties
        protected override string XmlSchemaContainerXPath => "webPart";

        protected override string XmlSchemaName => "http://schemas.microsoft.com/WebPart/v3";

        protected override string XmlEntityXPath => "webParts/webPart";

        protected override string CacheDirectoryName => "WebParts";

        protected override IEqualityComparer<WebPartXmlEntity> ItemsEqualityComparer => new WebPartXmlEntityEqualityComparer();

        public static WebPartCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<WebPartCache>();
        } 
        #endregion

        public WebPartCache(Lifetime lifetime, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, persistentIndexManager)
        {
        }

        #region Methods
        
        #region Overrided methods

        protected override SPPsiCacheItems<WebPartXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<WebPartXmlEntity> jetHashSet = null;
            IFile file = sourceFile.GetDominantPsiFile<XmlLanguage>();

            if (file != null)
            {
                IXmlFile xmlFile = file as IXmlFile;
               
                IXmlTag validatedTag = xmlFile.GetNestedTags<IXmlTag>(XmlSchemaContainerXPath).FirstOrDefault();

                if (validatedTag != null)
                {
                    IXmlTag titleTag =
                        xmlFile.GetNestedTags<IXmlTag>("webParts/webPart/data/properties/property")
                            .FirstOrDefault(t => t.CheckAttributeValue("name", new[] {"Title"}, true));

                    if (titleTag != null)
                    {
                        if (jetHashSet == null)
                            jetHashSet = new JetHashSet<WebPartXmlEntity>(ItemsEqualityComparer);

                        var treeOffset = titleTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                        {
                            jetHashSet.Add(new WebPartXmlEntity(titleTag, xmlFile)
                            {
                                Offset = treeOffset.Offset,
                                SourceFileFullPath = sourceFile.GetLocation().Directory.FullPath,
                                SourceFileName = sourceFile.Name
                            });
                        }
                    }
                }
            }

            if (jetHashSet == null)
                return null;
            else
                return new SPPsiCacheItems<WebPartXmlEntity>(jetHashSet.ToArray());
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class WebPartXmlEntity : SPXmlEntity
    {
        public string Title { get; set; }
        public string Group { get; set; }
        public override string GetPropertyValue(string attributeName)
        {
            switch (attributeName)
            {
                case "Title":
                    return Title;
                case "Group":
                    return Group;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }

        public WebPartXmlEntity(UnsafeReader reader)
            : base(reader)
        {
            Title = reader.ReadString();
            Group = reader.ReadString();
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Title);
            writer.Write(Group);
        }

        public WebPartXmlEntity(IXmlTag titleTag, IXmlFile xmlFile)
        {
            Title = titleTag.InnerText.Trim();
            var groupTag = xmlFile.GetNestedTags<IXmlTag>("webParts/webPart/data/properties/property")
                .FirstOrDefault(t => t.CheckAttributeValue("name", new[] {"Group"}, true));
            if (groupTag != null)
                Group = groupTag.InnerText.Trim();
        }
    }

    public class WebPartXmlEntityEqualityComparer : IEqualityComparer<WebPartXmlEntity>
    {
        public bool Equals(WebPartXmlEntity x, WebPartXmlEntity y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Title.Trim(), y.Title.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(WebPartXmlEntity obj)
        {
            return (obj.Title.Trim()).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
