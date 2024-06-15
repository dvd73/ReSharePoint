using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Application;
using JetBrains.Application.changes;
using JetBrains.Application.PersistentMap;
using JetBrains.Application.Progress;
using JetBrains.Application.Threading;
using JetBrains.DataFlow;
using JetBrains.DocumentManagers.impl;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Web.WebConfig;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Serialization;
using JetBrains.Util;
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
    public class FeatureCache : SPXmlEntityCache<FeatureXmlEntity>
    {
        #region Properties
        public override string Version => "2.1";

        protected override string XmlSchemaContainerXPath => "feature";

        protected override string XmlEntityXPath => "feature";

        protected override string CacheDirectoryName => "Features";

        protected override string XmlSchemaName => "http://schemas.microsoft.com/VisualStudio/2008/SharePointTools/FeatureModel";

        protected override IEqualityComparer<FeatureXmlEntity> ItemsEqualityComparer => new FeatureXmlEntityEqualityComparer();

        public static FeatureCache GetInstance(ISolution solution)
        {
            return solution.GetComponent<FeatureCache>();
        } 
        #endregion

        private readonly IPersistentIndexManager _persistentIndexManager;

        public FeatureCache(Lifetime lifetime, IShellLocks locks, IPersistentIndexManager persistentIndexManager)
            : base(lifetime, locks, persistentIndexManager)
        {
            _persistentIndexManager = persistentIndexManager;
        }

        #region Methods
        
        #region Overrided methods

        public override object Load(IProgressIndicator progress, bool enablePersistence)        {
            var featureFiles = _persistentIndexManager.GetAllSourceFiles()
                .Where(i => i.GetExtensionWithDot() == ".feature");
            
            foreach (var featureFile in featureFiles)
            {
                if (!Map.Cache.ContainsKeyInCache(featureFile))
                {
                    MarkAsDirty(featureFile);
                }
            }

            return base.Load(progress, enablePersistence);
        }
        
        protected override SPPsiCacheItems<FeatureXmlEntity> BuildData(IPsiSourceFile sourceFile)
        {
            JetHashSet<FeatureXmlEntity> jetHashSet = null;
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
                            jetHashSet = new JetHashSet<FeatureXmlEntity>(ItemsEqualityComparer);

                        var treeOffset = xmlTag.GetTreeStartOffset();
                        if (treeOffset.IsValid())
                            jetHashSet.Add(new FeatureXmlEntity(xmlTag, sourceFile)
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
                return new SPPsiCacheItems<FeatureXmlEntity>(jetHashSet.ToArray());
        }

        public override void OnDocumentChange(IPsiSourceFile sourceFile, ProjectFileDocumentCopyChange change)
        {
            if (change.OldLength != change.NewLength)
                base.OnDocumentChange(sourceFile, change);
        }
        #endregion

        public IEnumerable<FeatureXmlEntity> GetReceivers(SPFeatureScope scope)
        {
            return Items.Where(item => item.Scope == scope);
        }

        #endregion
    }

    [Serializable()]
    public class FeatureXmlEntity : SPXmlEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SPFeatureScope Scope { get; set; }
        public bool AlwaysForceInstall { get; set; }
        public string ImageUrl { get; set; }
        public string ReceiverAssembly { get; set; }
        public string ReceiverClass { get; set; }
        public string ProjectName { get; set; }
        public ICollection<string> ReceiverClassReferences { get; set; }
        public ICollection<Guid> ProjectItems { get; set; }

        public FeatureXmlEntity(UnsafeReader reader) : base(reader)
        {
            Id = reader.ReadGuid();
            Title = reader.ReadString();
            Description = reader.ReadString();
            Scope = (SPFeatureScope) reader.ReadInt16();
            AlwaysForceInstall = reader.ReadBool();
            ImageUrl = reader.ReadString();
            ReceiverAssembly = reader.ReadString();
            ReceiverClass = reader.ReadString();
            ProjectName = reader.ReadString();
            ReceiverClassReferences = reader.ReadArray(UnsafeReader.StringDelegate);
            ProjectItems = reader.ReadArray(UnsafeReader.GuidDelegate);
        }

        public override void Write(UnsafeWriter writer)
        {
            base.Write(writer);

            writer.Write(Id);
            writer.Write(Title);
            writer.Write(Description);
            writer.Write((short)Scope);
            writer.Write(AlwaysForceInstall);
            writer.Write(ImageUrl);
            writer.Write(ReceiverAssembly);
            writer.Write(ReceiverClass);
            writer.Write(ProjectName);
            writer.Write(UnsafeWriter.StringDelegate, ReceiverClassReferences);
            writer.Write(UnsafeWriter.GuidDelegate, ProjectItems);
        }

        public FeatureXmlEntity(IXmlTag xmlTag, IPsiSourceFile sourceFile)
        {
            var project = sourceFile.GetProject();

            Id = xmlTag.AttributeExists("featureId")
                ? new Guid(xmlTag.GetAttribute("featureId").UnquotedValue.Trim())
                : Guid.Empty;
            Title = xmlTag.AttributeExists("title") ? xmlTag.GetAttribute("title").UnquotedValue.Trim() : String.Empty;
            Description = xmlTag.AttributeExists("description") ? xmlTag.GetAttribute("description").UnquotedValue.Trim() : String.Empty;
            Scope =
                ResolveScope(xmlTag.AttributeExists("scope")
                    ? xmlTag.GetAttribute("scope").UnquotedValue.Trim()
                    : String.Empty);
            AlwaysForceInstall = xmlTag.AttributeExists("alwaysForceInstall")
                ? Convert.ToBoolean(xmlTag.GetAttribute("alwaysForceInstall").UnquotedValue.Trim())
                : false;
            ImageUrl = xmlTag.AttributeExists("imageUrl")
                ? xmlTag.GetAttribute("imageUrl").UnquotedValue.Trim()
                : String.Empty;
            ReceiverAssembly = ResolveReceiverAssembly(project,
                xmlTag.AttributeExists("receiverAssembly")
                    ? xmlTag.GetAttribute("receiverAssembly").UnquotedValue.Trim()
                    : String.Empty);
            ReceiverClass = xmlTag.AttributeExists("receiverClass")
                ? xmlTag.GetAttribute("receiverClass").UnquotedValue.Trim()
                : String.Empty;
            ReceiverClassDeclaration = ResolveReceiverClass(project,
                xmlTag.AttributeExists("receiverClass")
                    ? xmlTag.GetAttribute("receiverClass").UnquotedValue.Trim()
                    : String.Empty);
            ProjectItems = GetProjectItems(xmlTag);

            if (project != null) ProjectName = String.IsNullOrEmpty(project.Name) ? project.Presentation : project.Name;
        }

        protected IClassDeclaration ReceiverClassDeclaration 
        {
            set
            {
                if (value != null)
                {
                    ReceiverClass = value.CLRName;
                    ReceiverClassReferences = value.ThisAndDescendants<IReferenceExpression>().ToEnumerable()
                        .Where(referenceExpression => referenceExpression.ReferenceExpressionTarget() is IMethod)
                        .Select(referenceExpression => referenceExpression.ReadableName())
                        .Where(s => !String.IsNullOrEmpty(s) && !s.Contains("Microsoft.") && !s.Contains("System."))
                        .Distinct()
                        .ToArray();
                }
            } 
        }

        public override string GetPropertyValue(string attributeName)
        {
            switch (attributeName)
            {
                case "Id":
                    return Id.ToString();
                case "Title":
                    return Title;
                case "Scope":
                    return Scope.ToString();
                case "AlwaysForceInstall":
                    return Convert.ToInt32(AlwaysForceInstall).ToString();
                case "ImageUrl":
                    return ImageUrl;
                case "ReceiverAssembly":
                    return ReceiverAssembly;
                case "ReceiverClass":
                    return ReceiverClass;
                case "ProjectName":
                    return ProjectName;
                case "description":
                    return Description;
                default:
                    throw new ArgumentOutOfRangeException("attributeName");
            }
        }

        private SPFeatureScope ResolveScope(string s)
        {
            switch (s.ToLower().Trim())
            {
                case "farm":
                    return SPFeatureScope.Farm;
                case "webapplication":
                    return SPFeatureScope.WebApplication;
                case "site":
                    return SPFeatureScope.Site;
                default:
                    return SPFeatureScope.Web;
            }
        }

        private IClassDeclaration ResolveReceiverClass(IProject project, string s)
        {
            IClassDeclaration result = null;

            if (!String.IsNullOrEmpty(s) && project != null)
            {
                const string regexp = @"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}";
                if (Regex.IsMatch(s, regexp))
                {
                    string guid = Regex.Match(s, regexp).Value;
                    foreach (IProjectFile projectFile in project.GetAllProjectFiles())
                    {
                        if (projectFile.IsValid() && projectFile.LanguageType.Is<CSharpProjectFileType>())
                        {
                            var sourceFile = projectFile.ToSourceFile();
                            if (sourceFile != null && !sourceFile.Properties.IsGeneratedFile)
                            {
                                foreach (ICSharpFile csharpFile in sourceFile.GetPsiFiles<CSharpLanguage>())
                                {
                                    if (csharpFile != null)
                                    {
                                        foreach (IClassDeclaration classDeclaration in csharpFile.ThisAndDescendants().OfType<IClassDeclaration>())
                                        {
                                            if (
                                                classDeclaration.Attributes.Any(
                                                    attribute =>
                                                        attribute.Name.ShortName == "Guid" &&
                                                        attribute.Arguments.Any(
                                                            argument =>
                                                                argument.Value != null &&
                                                                argument.Value.IsConstantValue() &&
                                                                argument.Value.ConstantValue.Value != null &&
                                                                String.Equals(
                                                                    argument.Value.ConstantValue.Value.ToString(), guid,
                                                                    StringComparison.OrdinalIgnoreCase))))
                                            {
                                                result = classDeclaration;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private string ResolveReceiverAssembly(IProject project, string s)
        {
            string result;

            if (project != null && !String.IsNullOrEmpty(s) && String.Equals(s, "$SharePoint.Project.AssemblyFullName$", StringComparison.OrdinalIgnoreCase))
            {
                result = project.GetOutputAssemblyFullName();
            }
            else
            {
                result = s;
            }

            return result;
        }

        private ICollection<Guid> GetProjectItems(IXmlTag xmlTag)
        {
            return
                xmlTag.GetNestedTags<IXmlTag>("projectItems/projectItemReference")
                    .Where(em => em.AttributeExists("itemId"))
                    .Select(em => Guid.Parse(em.GetAttribute("itemId").UnquotedValue)).ToArray();
        }

        public override string ToString()
        {
            return Title;
        }
    }

    public class FeatureXmlEntityEqualityComparer : IEqualityComparer<FeatureXmlEntity>
    {
        public bool Equals(FeatureXmlEntity x, FeatureXmlEntity y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(FeatureXmlEntity obj)
        {
            return (obj.Id).GetHashCode();
        }
    }
}
