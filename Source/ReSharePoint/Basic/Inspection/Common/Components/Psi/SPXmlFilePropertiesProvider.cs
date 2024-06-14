using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Impl;
using JetBrains.ReSharper.Psi.Web.WebConfig;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.Util;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.Components.Psi
{
    /*
     * A file in a project has properties associated with that control how ReSharper processes it. 
     * For caches, the interesting values are ShouldBuildPsi, ProvidesCodeModel and IsICacheParticipant. 
     * They’re very similar, but are for different purposes:
     *    ShouldBuildPsi - indicates that the file should be parsed
     *    ProvidesCodeModel - usually the same as ShouldBuildPsi, but indicates if the parsed data is used in the project's code model. 
     *                        A good example here is external sources. We build the PSI, so we can navigate, etc, but there’s no code model, as it’s not part of the project
     *    IsICacheParticipant - indicates that the file should be handled by ICache implementations, but doesn’t indicate if that means a PSI will be built, or if it contributes to the code model.
     * Xml files return false for ProvidesCodeModel, so no xml file is passed to any ICache.
     */
    [PsiSharedComponent]
    [Applicability(
       IDEProjectType.SPFarmSolution  |
       IDEProjectType.SPSandbox )]
    public class SPXmlFilePropertiesProvider : IPsiSourceFilePropertiesProvider
    {
        public IPsiSourceFileProperties GetPsiProperties(IPsiSourceFileProperties prevProperties, IProject project,
            IProjectFile projectFile, IPsiSourceFile sourceFile)
        {
            if (CheclXmlFile(sourceFile) && project.IsApplicableFor(this, sourceFile.PsiModule.TargetFrameworkId))
            {
                return new SPXmlFileProperties(prevProperties, projectFile, sourceFile);
            }
            else
            {
                return prevProperties;
            }
        }

        private static bool CheclXmlFile(IPsiSourceFile sourceFile)
        {
            string[] validXmlExtensions = {".feature"};

            return sourceFile.PrimaryPsiLanguage.Is<XmlLanguage>() || validXmlExtensions.Any(validXmlExtension => sourceFile.GetExtensionWithDot() == validXmlExtension);
        }

        public double Order => 1000;
    }
    
    public class SPXmlFileProperties : DefaultPsiProjectFileProperties
    {
        private readonly IPsiSourceFileProperties _sourceProperties;

        public override bool ProvidesCodeModel => true;

        public override bool ShouldBuildPsi => _sourceProperties == null || _sourceProperties.ShouldBuildPsi;

        public override bool IsNonUserFile => _sourceProperties != null && _sourceProperties.IsNonUserFile;

        public override bool IsICacheParticipant => _sourceProperties == null || _sourceProperties.IsICacheParticipant;

        public override bool IsGeneratedFile
        {
            get
            {
                if (SourceFile.GetExtensionWithDot() == ".feature")
                    return true;
                
                return _sourceProperties != null && _sourceProperties.IsGeneratedFile;
            }
        }

        public override ICollection<PreProcessingDirective> GetDefines()
        {
            return _sourceProperties == null ? EmptyList<PreProcessingDirective>.InstanceList :_sourceProperties.GetDefines();
        }

        public override IEnumerable<string> GetPreImportedNamespaces()
        {
            return _sourceProperties == null ? EmptyList<string>.InstanceList : _sourceProperties.GetPreImportedNamespaces();
        }

        public override string GetDefaultNamespace()
        {
            return _sourceProperties == null ? String.Empty : _sourceProperties.GetDefaultNamespace();
        }

        public override string ToString()
        {
            return _sourceProperties == null ? String.Empty : _sourceProperties.ToString();
        }

        public SPXmlFileProperties(IPsiSourceFileProperties sourceProperties, IProjectFile projectFile, IPsiSourceFile sourceFile)
            : base(projectFile, sourceFile)
        {
            _sourceProperties = sourceProperties;
        }
    }
}
