using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public class FileCache : SPXmlEntityCache<FileXmlEntity>
    {
        #region Properties
        protected override string XmlSchemaContainerXPath => "Elements";

        protected override string XmlEntityXPath => "Elements/Module/File";

        protected override string CacheDirectoryName => "Files";

        protected override IEqualityComparer<FileXmlEntity> ItemsEqualityComparer => new FileXmlEntityEqualityComparer();

        public static FileCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<FileCache>();
        } 
        #endregion

        public FileCache(Lifetime lifetime, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, persistentIndexManager)
        {
        }

        #region Methods
        
        #region Overrided methods

        protected override SPPsiCacheItems<FileXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<FileXmlEntity> jetHashSet = null;
            IFile file = sourceFile.GetDominantPsiFile<XmlLanguage>();

            if (file is IXmlFile xmlFile)
            {
                IXmlTag validatedTag = xmlFile.GetNestedTags<IXmlTag>(XmlSchemaContainerXPath).FirstOrDefault();

                if (validatedTag != null)
                {
                    foreach (IXmlTag xmlTag in xmlFile.GetNestedTags<IXmlTag>(XmlEntityXPath))
                    {
                        if (jetHashSet == null)
                            jetHashSet = new JetHashSet<FileXmlEntity>(ItemsEqualityComparer);

                        var treeOffset = xmlTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                            jetHashSet.Add(new FileXmlEntity(xmlTag, sourceFile)
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
                return new SPPsiCacheItems<FileXmlEntity>(jetHashSet.ToArray());
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class FileXmlEntity : SPXmlEntity
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ContentTypeId { get; set; }
        public string ProjectName { get; set; }

        public FileXmlEntity(UnsafeReader reader)
            : base(reader)
        {
            Title = reader.ReadString();
            Url = reader.ReadString();
            FileName = reader.ReadString();
            ContentType = reader.ReadString();
            ContentTypeId = reader.ReadString();
            ProjectName = reader.ReadString();
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Title);
            writer.Write(Url);
            writer.Write(FileName);
            writer.Write(ContentType);
            writer.Write(ContentTypeId);
            writer.Write(ProjectName);
        }

        public FileXmlEntity(IXmlTag xmlTag, IPsiSourceFile sourceFile)
        {
            var project = sourceFile.GetProject();
            Url = xmlTag.AttributeExists("Url") ? xmlTag.GetAttribute("Url").UnquotedValue.Trim() : String.Empty;
            if (xmlTag.AttributeExists("Name"))
                Url = xmlTag.GetAttribute("Name").UnquotedValue.Trim();

            FileName = !String.IsNullOrEmpty(Url) ? GetFileName(Url) : String.Empty;
            Title = String.Empty;
            ContentType = String.Empty;
            ContentTypeId = String.Empty;

            var propertyTags = xmlTag.GetNestedTags<IXmlTag>("Property");
            if (propertyTags != null && propertyTags.Count > 0)
            {
                var titleTag = propertyTags.FirstOrDefault(t => t.CheckAttributeValue("Name", new[] {"Title"}) && t.AttributeExists("Value"));
                if (titleTag != null)
                    Title = titleTag.GetAttribute("Value").UnquotedValue.Trim();

                var contentTypeTag = propertyTags.FirstOrDefault(t => t.CheckAttributeValue("Name", new[] { "ContentType" }) && t.AttributeExists("Value"));
                if (contentTypeTag != null)
                    ContentType = contentTypeTag.GetAttribute("Value").UnquotedValue.Trim();

                var contentTypeIdTag = propertyTags.FirstOrDefault(t => t.CheckAttributeValue("Name", new[] { "ContentTypeId" }) && t.AttributeExists("Value"));
                if (contentTypeIdTag != null)
                    ContentTypeId = contentTypeIdTag.GetAttribute("Value").UnquotedValue.Trim();
            }
            if (project != null) ProjectName = String.IsNullOrEmpty(project.Name) ? project.Presentation : project.Name;
        }

        public override string GetPropertyValue(string attributeName)
        {
            switch (attributeName)
            {
                case "Title":
                    return Title;
                case "Url":
                    return Url;
                case "FileName":
                    return FileName;
                case "ContentType":
                    return ContentType;
                case "ContentTypeId":
                    return ContentTypeId;
                case "ProjectName":
                    return ProjectName;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }

        private static string GetFileName(string p)
        {
            Regex regex = new Regex(@"(\w+)(\.\w+)+(?!.*(\w+)(\.\w+)+)");
            return regex.Match(p).Value;
        }
    }

    public class FileXmlEntityEqualityComparer : IEqualityComparer<FileXmlEntity>
    {
        public bool Equals(FileXmlEntity x, FileXmlEntity y)
        {
            return x.Offset.Equals(y.Offset) &&
                   String.Equals(x.Title.Trim(), y.Title.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(FileXmlEntity obj)
        {
            return (obj.Title.Trim()).GetHashCode() ^ obj.Offset.GetHashCode();
        }
    }
}
