using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Application;
using JetBrains.Application.changes;
using JetBrains.Application.Threading;
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
       IDEProjectType.SPSandbox  )]
    public class ContentTypeCache : SPXmlEntityCache<ContentTypeXmlEntity>
    {
        #region Properties
        protected override string XmlSchemaContainerXPath => "Elements";

        protected override string XmlEntityXPath => "Elements/ContentType";

        protected override string CacheDirectoryName => "ContentTypes";

        protected override IEqualityComparer<ContentTypeXmlEntity> ItemsEqualityComparer => new ContentTypeXmlEntityEqualityComparer();

        public static ContentTypeCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<ContentTypeCache>();
        } 
        #endregion

        public ContentTypeCache(Lifetime lifetime, IShellLocks locks, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, locks, persistentIndexManager)
        {
        }

        #region Methods

        #region Overrided methods

        protected override SPPsiCacheItems<ContentTypeXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<ContentTypeXmlEntity> jetHashSet = null;
            IFile file = sourceFile.GetDominantPsiFile<XmlLanguage>();

            if (file is IXmlFile xmlFile)
            {
                IXmlTag validatedTag = xmlFile.GetNestedTags<IXmlTag>(XmlSchemaContainerXPath).FirstOrDefault();

                if (validatedTag != null)
                {
                    foreach (IXmlTag xmlTag in xmlFile.GetNestedTags<IXmlTag>(XmlEntityXPath))
                    {
                        if (jetHashSet == null)
                            jetHashSet = new JetHashSet<ContentTypeXmlEntity>(ItemsEqualityComparer);
                        
                        var treeOffset = xmlTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                            jetHashSet.Add(new ContentTypeXmlEntity(xmlTag, sourceFile)
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
                return new SPPsiCacheItems<ContentTypeXmlEntity>(jetHashSet.ToArray());
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class ContentTypeXmlEntity : SPXmlEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public ICollection<string> FieldLinks { get; set; }
        public string ProjectName { get; set; }

        public ContentTypeXmlEntity(UnsafeReader reader)
            : base(reader)
        {
            Id = reader.ReadString();
            Name = reader.ReadString();
            Group = reader.ReadString();
            FieldLinks = reader.ReadArray(UnsafeReader.StringDelegate);
            ProjectName = reader.ReadString();
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Id);
            writer.Write(Name);
            writer.Write(Group);
            writer.Write(UnsafeWriter.StringDelegate, FieldLinks);
            writer.Write(ProjectName);
        }

        public ContentTypeXmlEntity(IXmlTag xmlTag, IPsiSourceFile sourceFile)
        {
            var project = sourceFile.GetProject();
            Id = xmlTag.AttributeExists("ID") ? xmlTag.GetAttribute("ID").UnquotedValue.Trim() : String.Empty;
            Name = xmlTag.AttributeExists("Name") ? xmlTag.GetAttribute("Name").UnquotedValue.Trim() : String.Empty;
            Group = xmlTag.AttributeExists("Group") ? xmlTag.GetAttribute("Group").UnquotedValue.Trim() : String.Empty;
            FieldLinks = GetFieldLinks(xmlTag);
            if (project != null) ProjectName = String.IsNullOrEmpty(project.Name) ? project.Presentation : project.Name;
        }

        public override string GetPropertyValue(string attributeName)
        {
            switch (attributeName)
            {
                case "ID":
                    return Id;
                case "Name":
                    return Name;
                case "Group":
                    return Group;
                case "ProjectName":
                    return ProjectName;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }

        private ICollection<string> GetFieldLinks(IXmlTag xmlTag)
        {
            return
                xmlTag.GetNestedTags<IXmlTag>("FieldRefs/FieldRef")
                    .Where(em => em.AttributeExists("ID"))
                    .Select(em => em.GetAttribute("ID").UnquotedValue).ToArray();
        }
    }

    public class ContentTypeXmlEntityEqualityComparer : IEqualityComparer<ContentTypeXmlEntity>
    {
        public bool Equals(ContentTypeXmlEntity x, ContentTypeXmlEntity y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Id.Trim(), y.Id.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.Name.Trim(), y.Name.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ContentTypeXmlEntity obj)
        {
            return (obj.Id.Trim() + obj.Name.Trim()).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
