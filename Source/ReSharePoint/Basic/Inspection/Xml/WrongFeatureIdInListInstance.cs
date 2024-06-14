using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
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

[assembly: RegisterConfigurableSeverity(WrongFeatureIdInListInstanceHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  WrongFeatureIdInListInstanceHighlighting.CheckId + ": " + WrongFeatureIdInListInstanceHighlighting.Message,
  "It is important to remember to add the right FeatureId attribute to the ListInstance element, if the TemplateType attribute points to a list template which is not in the same feature as the one you are creating the list instance in. If you forget or set it wrong (copy&paste), you will get some very vague error messages.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox)]
    public class WrongFeatureIdInListInstance : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListInstance")
            {
                var solution = element.GetSolution();
                ProblemAttribute = element.GetAttribute("FeatureId");

                if (ProblemAttribute != null && !String.IsNullOrEmpty(ProblemAttribute.UnquotedValue) &&
                    Guid.TryParse(ProblemAttribute.UnquotedValue, out var templateFeatureId))
                {
                    string builtInFeatureId = TypeInfo.GetBuiltInFeatureName(templateFeatureId);
                    if (String.IsNullOrEmpty(builtInFeatureId))
                    {
                        FeatureXmlEntity featureEntity = FeatureCache.GetInstance(solution)
                            .Items.FirstOrDefault(
                                f => f.Id.Equals(templateFeatureId));

                        result = featureEntity == null;

                        if (featureEntity != null && element.AttributeExists("TemplateType"))
                        {
                            IXmlAttribute templateType = element.GetAttribute("TemplateType");
                            if (!String.IsNullOrEmpty(templateType.UnquotedValue) &&
                                Int32.TryParse(templateType.UnquotedValue, out var templateId))
                            {
                                var listTemplate = ListTemplateCache.GetInstance(solution)
                                    .Items.FirstOrDefault(lt => Int32.Parse(lt.Type) == templateId);

                                if (listTemplate != null && !String.IsNullOrEmpty(listTemplate.SourceFileFullPath))
                                {
                                    var sourceFilePath = listTemplate.SourceFileFullPath;
                                    var project = element.GetProject();
                                    var solutionComponent =
                                        solution.GetComponent<SharePointProjectItemsSolutionProvider>();
                                    IEnumerable<SharePointProjectItem> spProjectItems =
                                        solutionComponent.GetCacheContent(project);
                                    var projectItem =
                                        spProjectItems.SingleOrDefault(
                                            pi =>
                                                pi.ItemType == SharePointProjectItemType.ListDefinition &&
                                                pi.ElementManifest == listTemplate.SourceFileName &&
                                                pi.Path == sourceFilePath);
                                    if (projectItem != null)
                                    {
                                        FeatureXmlEntity feature = FeatureCache.GetInstance(solution)
                                            .Items.FirstOrDefault(
                                                f => f.ProjectItems.Any(pi => pi.Equals(projectItem.Id)));

                                        result = feature != null && featureEntity.Id != feature.Id;
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
            return new WrongFeatureIdInListInstanceHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class WrongFeatureIdInListInstanceHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.WrongFeatureIdInListInstance;
        public const string Message = "Wrong FeatureId for list instance";

        public WrongFeatureIdInListInstanceHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
