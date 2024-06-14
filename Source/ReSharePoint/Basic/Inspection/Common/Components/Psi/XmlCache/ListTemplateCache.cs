using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Serialization;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache
{
    [PsiComponent]
    [Applicability(
       IDEProjectType.SPFarmSolution  |
       IDEProjectType.SPSandbox )]
    public class ListTemplateCache : SPXmlEntityCache<ListTemplateXmlEntity>
    {
        #region Properties
        protected override string XmlSchemaContainerXPath => "Elements";

        protected override string XmlEntityXPath => "Elements/ListTemplate";

        protected override string CacheDirectoryName => "ListTemplates";

        protected override IEqualityComparer<ListTemplateXmlEntity> ItemsEqualityComparer => new ListTemplateXmlEntityEqualityComparer();

        public static ListTemplateCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<ListTemplateCache>();
        } 
        #endregion

        public ListTemplateCache(Lifetime lifetime, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, persistentIndexManager)
        {
        }

        #region Methods

        #region Overrided methods

        protected override SPPsiCacheItems<ListTemplateXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<ListTemplateXmlEntity> jetHashSet = null;
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
                            jetHashSet = new JetHashSet<ListTemplateXmlEntity>(ItemsEqualityComparer);

                        var treeOffset = xmlTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                        {
                            jetHashSet.Add(new ListTemplateXmlEntity(xmlTag, sourceFile)
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
                return new SPPsiCacheItems<ListTemplateXmlEntity>(jetHashSet.ToArray());
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class ListTemplateXmlEntity : SPXmlEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ProjectName { get; set; }

        public ListTemplateXmlEntity(UnsafeReader reader)
            : base(reader)
        {
            Name = reader.ReadString();
            DisplayName = reader.ReadString();
            Description = reader.ReadString();
            Type = reader.ReadString();
            ProjectName = reader.ReadString();
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Name);
            writer.Write(DisplayName);
            writer.Write(Description);
            writer.Write(Type);
            writer.Write(ProjectName);
        }

        public ListTemplateXmlEntity(IXmlTag xmlTag, IPsiSourceFile sourceFile)
        {
            var project = sourceFile.GetProject();

            Name = xmlTag.AttributeExists("Name") ? xmlTag.GetAttribute("Name").UnquotedValue.Trim() : String.Empty;
            DisplayName = xmlTag.AttributeExists("DisplayName")
                ? xmlTag.GetAttribute("DisplayName").UnquotedValue.Trim()
                : String.Empty;
            Description = xmlTag.AttributeExists("Description")
                ? xmlTag.GetAttribute("Description").UnquotedValue.Trim()
                : String.Empty;
            Type = xmlTag.AttributeExists("Type") ? xmlTag.GetAttribute("Type").UnquotedValue.Trim() : String.Empty;
            if (project != null) ProjectName = String.IsNullOrEmpty(project.Name) ? project.Presentation : project.Name;
        }

        public override string GetPropertyValue(string attributeName)
        {
            switch (attributeName)
            {
                case "DisplayName":
                    return DisplayName;
                case "Name":
                    return Name;
                case "Description":
                    return Description;
                case "Type":
                    return Type;
                case "ProjectName":
                    return ProjectName;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }
    }

    public class ListTemplateXmlEntityEqualityComparer : IEqualityComparer<ListTemplateXmlEntity>
    {
        public bool Equals(ListTemplateXmlEntity x, ListTemplateXmlEntity y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Name.Trim(), y.Name.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.DisplayName.Trim(), y.DisplayName.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.Type.Trim(), y.Type.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ListTemplateXmlEntity obj)
        {
            return (obj.Name.Trim() + obj.DisplayName.Trim() + obj.Type.Trim()).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
