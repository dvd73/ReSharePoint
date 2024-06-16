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
       IDEProjectType.SPSandbox )]
    public class FieldCache : SPXmlEntityCache<FieldXmlEntity>
    {
        #region Properties
        protected override string XmlSchemaContainerXPath => "Elements";

        protected override string XmlEntityXPath => "Elements/Field";

        protected override string CacheDirectoryName => "Fields";

        protected override IEqualityComparer<FieldXmlEntity> ItemsEqualityComparer => new FieldXmlEntityEqualityComparer();

        public static FieldCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<FieldCache>();
        } 
        #endregion

        public FieldCache(Lifetime lifetime, IShellLocks locks, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, locks, persistentIndexManager)
        {
        }

        #region Methods

        #region Overrided methods

        protected override SPPsiCacheItems<FieldXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<FieldXmlEntity> jetHashSet = null;
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
                            jetHashSet = new JetHashSet<FieldXmlEntity>(ItemsEqualityComparer);

                        var treeOffset = xmlTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                            jetHashSet.Add(new FieldXmlEntity(xmlTag, sourceFile)
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
                return new SPPsiCacheItems<FieldXmlEntity>(jetHashSet.ToArray());
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class FieldXmlEntity : SPXmlEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StaticName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public string ProjectName { get; set; }

        public FieldXmlEntity(UnsafeReader reader)
            : base(reader)
        {
            Id = reader.ReadString();
            Name = reader.ReadString();
            DisplayName = reader.ReadString();
            Description = reader.ReadString();
            Type = reader.ReadString();
            Group = reader.ReadString();
            ProjectName = reader.ReadString();
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Id);
            writer.Write(Name);
            writer.Write(DisplayName);
            writer.Write(Description);
            writer.Write(Type);
            writer.Write(Group);
            writer.Write(ProjectName);
        }

        public FieldXmlEntity(IXmlTag xmlTag, IPsiSourceFile sourceFile)
        {
            var project = sourceFile.GetProject();

            Id = xmlTag.AttributeExists("ID") ? xmlTag.GetAttribute("ID").UnquotedValue.Trim() : String.Empty;
            Name = xmlTag.AttributeExists("Name") ? xmlTag.GetAttribute("Name").UnquotedValue.Trim() : String.Empty;
            StaticName = xmlTag.AttributeExists("StaticName")
                ? xmlTag.GetAttribute("StaticName").UnquotedValue.Trim()
                : String.Empty;
            DisplayName = xmlTag.AttributeExists("DisplayName")
                ? xmlTag.GetAttribute("DisplayName").UnquotedValue.Trim()
                : String.Empty;
            Description = xmlTag.AttributeExists("Description")
                ? xmlTag.GetAttribute("Description").UnquotedValue.Trim()
                : String.Empty;
            Type = xmlTag.AttributeExists("Type") ? xmlTag.GetAttribute("Type").UnquotedValue.Trim() : String.Empty;
            Group = xmlTag.AttributeExists("Group") ? xmlTag.GetAttribute("Group").UnquotedValue.Trim() : String.Empty;
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
                case "StaticName":
                    return StaticName;
                case "DisplayName":
                    return DisplayName;
                case "Type":
                    return Type;
                case "Group":
                    return Group;
                case "ProjectName":
                    return ProjectName;
                case "Description":
                    return Description;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }
        
    }

    public class FieldXmlEntityEqualityComparer : IEqualityComparer<FieldXmlEntity>
    {
        public bool Equals(FieldXmlEntity x, FieldXmlEntity y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Id.Trim(), y.Id.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.Name.Trim(), y.Name.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.StaticName.Trim(), y.StaticName.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.DisplayName.Trim(), y.DisplayName.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(FieldXmlEntity obj)
        {
            return (obj.Id.Trim() + obj.Name.Trim() + obj.StaticName + obj.DisplayName).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
