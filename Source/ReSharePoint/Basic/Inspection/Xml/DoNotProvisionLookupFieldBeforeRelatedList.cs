using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Basic.Inspection.Common.Components.Solution.ProjectFileCache;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(DoNotProvisionLookupFieldBeforeRelatedListHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotProvisionLookupFieldBeforeRelatedListHighlighting.CheckId + ": " + DoNotProvisionLookupFieldBeforeRelatedListHighlighting.Message,
  "Consider to put lookup fields in the same feature among with related lists.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotProvisionLookupFieldBeforeRelatedList : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition() && element.AttributeExists("List"))
            {
                ProblemAttribute = element.GetAttribute("List");
                if (!String.IsNullOrEmpty(ProblemAttribute.UnquotedValue) && ProblemAttribute.UnquotedValue != "Self")
                {
                    var solution = element.GetSolution();
                    var project = element.GetProject();
                    var sourceFile = element.GetSourceFile();
                    if (sourceFile != null)
                    {
                        string sourceFilePath = sourceFile.GetLocation().Directory.FullPath;
                        SharePointProjectItemsSolutionProvider solutionComponent =
                            solution.GetComponent<SharePointProjectItemsSolutionProvider>();
                        IEnumerable<SharePointProjectItem> spProjectItems = solutionComponent.GetCacheContent(project);
                        SharePointProjectItem projectItem =
                            spProjectItems.SingleOrDefault(
                                pi =>
                                    pi.ItemType == SharePointProjectItemType.Field &&
                                    pi.ElementManifest == sourceFile.Name && pi.Path == sourceFilePath);

                        if (projectItem != null)
                        {
                            FeatureXmlEntity fieldFeature = FeatureCache.GetInstance(solution)
                                .Items.FirstOrDefault(
                                    f => f.ProjectItems.Any(pi => pi.Equals(projectItem.Id)));

                            if (fieldFeature != null)
                            {
                                string relatedListUrl = ProblemAttribute.UnquotedValue;
                                var listInstance =
                                    ListInstanceCache.GetInstance(solution)
                                        .Items.FirstOrDefault(li => li.Url == relatedListUrl);

                                if (listInstance != null && !String.IsNullOrEmpty(listInstance.SourceFileFullPath))
                                {
                                    sourceFilePath = listInstance.SourceFileFullPath;
                                    spProjectItems = solutionComponent.GetCacheContent(project);
                                    projectItem =
                                        spProjectItems.SingleOrDefault(
                                            pi =>
                                                pi.ItemType == SharePointProjectItemType.ListInstance &&
                                                pi.ElementManifest == listInstance.SourceFileName &&
                                                pi.Path == sourceFilePath);
                                    if (projectItem != null)
                                    {
                                        FeatureXmlEntity feature = FeatureCache.GetInstance(solution)
                                            .Items.FirstOrDefault(
                                                f => f.ProjectItems.Any(pi => pi.Equals(projectItem.Id)));

                                        result = feature != null && fieldFeature.Id != feature.Id;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DoNotProvisionLookupFieldBeforeRelatedListHighlighting(ProblemAttribute); 
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotProvisionLookupFieldBeforeRelatedListHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DoNotProvisionLookupFieldBeforeRelatedList;
        public const string Message = "Do not provision lookup field outside related list";

        public DoNotProvisionLookupFieldBeforeRelatedListHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
