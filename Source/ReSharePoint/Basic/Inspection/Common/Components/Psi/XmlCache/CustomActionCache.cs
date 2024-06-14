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
    public class CustomActionCache : SPXmlEntityCache<CustomActionXmlEntity>
    {
        #region Properties
        protected override string XmlSchemaContainerXPath => "Elements";

        protected override string XmlEntityXPath => "Elements/CustomAction";

        protected override string CacheDirectoryName => "CustomActions";

        protected override IEqualityComparer<CustomActionXmlEntity> ItemsEqualityComparer => new CustomActionXmlEntityEqualityComparer();

        public static CustomActionCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<CustomActionCache>();
        } 
        #endregion

        public CustomActionCache(Lifetime lifetime, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, persistentIndexManager)
        {
        }

        #region Methods

        #region Overrided methods

        protected override SPPsiCacheItems<CustomActionXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<CustomActionXmlEntity> jetHashSet = null;
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
                            jetHashSet = new JetHashSet<CustomActionXmlEntity>(ItemsEqualityComparer);
                        
                        var treeOffset = xmlTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                            jetHashSet.Add(new CustomActionXmlEntity(xmlTag)
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
                return new SPPsiCacheItems<CustomActionXmlEntity>(jetHashSet.ToArray());
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class CustomActionXmlEntity : SPXmlEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }

        public CustomActionXmlEntity(UnsafeReader reader)
            : base(reader)
        {
            Id = reader.ReadString();
            Title = reader.ReadString();
            Location = reader.ReadString();
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Id);
            writer.Write(Title);
            writer.Write(Location);
        }

        public CustomActionXmlEntity(IXmlTag xmlTag)
        {
            Id = xmlTag.AttributeExists("Id") ? xmlTag.GetAttribute("Id").UnquotedValue.Trim() : String.Empty;
            Title = xmlTag.AttributeExists("Title") ? xmlTag.GetAttribute("Title").UnquotedValue.Trim() : String.Empty;
            Location = xmlTag.AttributeExists("Location")
                ? xmlTag.GetAttribute("Location").UnquotedValue.Trim()
                : String.Empty;
        }

        public override string GetPropertyValue(string attributeName)
        {
            switch (attributeName)
            {
                case "Title":
                    return Title;
                case "Id":
                    return Id;
                case "Location":
                    return Location;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }
    }

    public class CustomActionXmlEntityEqualityComparer : IEqualityComparer<CustomActionXmlEntity>
    {
        public bool Equals(CustomActionXmlEntity x, CustomActionXmlEntity y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Id.Trim(), y.Id.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.Title.Trim(), y.Title.Trim(), StringComparison.OrdinalIgnoreCase) &&
                   String.Equals(x.Location.Trim(), y.Location.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(CustomActionXmlEntity obj)
        {
            return (obj.Id.Trim() + obj.Title.Trim() + obj.Location.Trim()).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
