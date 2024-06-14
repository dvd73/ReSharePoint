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
    public class ListInstanceCache : SPXmlEntityCache<ListInstanceXmlEntity>
    {
        #region Properties
        protected override string XmlSchemaContainerXPath => "Elements";

        protected override string XmlEntityXPath => "Elements/ListInstance";

        protected override string CacheDirectoryName => "ListInstances";

        protected override IEqualityComparer<ListInstanceXmlEntity> ItemsEqualityComparer => new ListInstanceXmlEntityEqualityComparer();

        public static ListInstanceCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<ListInstanceCache>();
        } 
        #endregion

        public ListInstanceCache(Lifetime lifetime, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, persistentIndexManager)
        {
        }

        #region Methods
        
        #region Overrided methods

        protected override SPPsiCacheItems<ListInstanceXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<ListInstanceXmlEntity> jetHashSet = null;
            IFile file = sourceFile.GetDominantPsiFile<XmlLanguage>();

            if (file != null)
            {
                IXmlFile xmlFile = file as IXmlFile;
                IXmlTag validatedTag = xmlFile.GetNestedTags<IXmlTag>(XmlSchemaContainerXPath).FirstOrDefault();

                if (validatedTag != null)
                {
                    foreach (IXmlTag xmlTag in xmlFile.GetNestedTags<IXmlTag>(XmlEntityXPath))
                    {
                        if (jetHashSet == null)
                            jetHashSet = new JetHashSet<ListInstanceXmlEntity>(ItemsEqualityComparer);

                        var treeOffset = xmlTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                            jetHashSet.Add(new ListInstanceXmlEntity(xmlTag)
                        {
                            Offset = treeOffset.Offset,
                            SourceFileFullPath = sourceFile.GetLocation().Directory.FullPath,
                            SourceFileName = sourceFile.Name
                        });
                    }
                }
            }

            if (jetHashSet == null)
                return null;
            else
                return new SPPsiCacheItems<ListInstanceXmlEntity>(jetHashSet.ToArray());
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class ListInstanceXmlEntity : SPXmlEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public override string GetPropertyValue(string attributeName)
        {
            switch (attributeName)
            {
                case "Url":
                    return Url;
                case "Title":
                    return Title;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }

        public ListInstanceXmlEntity(UnsafeReader reader)
            : base(reader)
        {
            Title = reader.ReadString();
            Url = reader.ReadString();
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Title);
            writer.Write(Url);
        }

        public ListInstanceXmlEntity(IXmlTag xmlTag)
        {
            Title = xmlTag.AttributeExists("Title") ? xmlTag.GetAttribute("Title").UnquotedValue.Trim() : String.Empty;
            Url = xmlTag.AttributeExists("Url") ? xmlTag.GetAttribute("Url").UnquotedValue.Trim() : String.Empty;
        }
    }

    public class ListInstanceXmlEntityEqualityComparer : IEqualityComparer<ListInstanceXmlEntity>
    {
        public bool Equals(ListInstanceXmlEntity x, ListInstanceXmlEntity y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Title.Trim(), y.Title.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.Url.Trim(), y.Url.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ListInstanceXmlEntity obj)
        {
            return (obj.Title.Trim() + obj.Url.Trim()).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
