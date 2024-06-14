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
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC015401Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015401Highlighting.CheckId + ": " + SPC015401Highlighting.Message,
  "ListInstance can only be deployed with Features of scope 'Web' or 'Site'.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DoNotDefineListInstanceInFeatureWithWrongScope : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListInstance")
            {
                var solution = element.GetSolution();
                var project = element.GetProject();
                var sourceFile = element.GetSourceFile();

                if (sourceFile != null)
                {
                    var sourceFilePath = sourceFile.GetLocation().Directory.FullPath;
                    SharePointProjectItemsSolutionProvider solutionComponent =
                        solution.GetComponent<SharePointProjectItemsSolutionProvider>();
                    IEnumerable<SharePointProjectItem> spProjectItems = solutionComponent.GetCacheContent(project);
                    var projectItem =
                        spProjectItems.SingleOrDefault(
                            pi =>
                                pi.ItemType == SharePointProjectItemType.ListInstance &&
                                pi.ElementManifest == sourceFile.Name && pi.Path == sourceFilePath);

                    if (projectItem != null)
                    {
                        FeatureXmlEntity feature = FeatureCache.GetInstance(solution)
                            .Items.FirstOrDefault(
                                f => f.ProjectItems.Any(pi => pi.Equals(projectItem.Id)));

                        if (feature != null)
                            result = feature.Scope == SPFeatureScope.WebApplication ||
                                     feature.Scope == SPFeatureScope.Farm;
                    }
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015401Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015401Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.SPC015401;
        public const string Message = "Do not define ListInstance in Feature with scope 'WebApplication' or 'Farm'";

        public SPC015401Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
