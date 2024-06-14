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
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(NotProvisionedEntityHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  NotProvisionedEntityHighlighting.CheckId + ": " + NotProvisionedEntityHighlighting.Message,
  "Any xml manifest is not included into feature considered not provisioned.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class NotProvisionedEntity : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            string s = element.Header.ContainerName;
            if (s == "Elements" ||
                s == "Project" ||
                s == "webParts")
            {
                var solution = element.GetSolution();
                var project = element.GetProject();
                var sourceFile = element.GetSourceFile();
                if (sourceFile != null)
                {
                    string sourceFilePath = sourceFile.GetLocation().Directory.FullPath;
                    SharePointProjectItemsSolutionProvider projectItemProvider =
                        solution.GetComponent<SharePointProjectItemsSolutionProvider>();
                    IEnumerable<SharePointProjectItem> spProjectItems = projectItemProvider.GetCacheContent(project);
                    SharePointProjectItem projectItem =
                        spProjectItems.FirstOrDefault(
                            pi =>
                                pi.ElementManifest == sourceFile.Name && pi.Path == sourceFilePath);

                    if (projectItem != null)
                    {
                        result = !FeatureCache.GetInstance(solution)
                            .Items.Any(
                                f => f.ProjectItems.Any(pi => pi.Equals(projectItem.Id)));
                    }
                }
            }
            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new NotProvisionedEntityHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class NotProvisionedEntityHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.General.NotProvisionedEntity;
        public const string Message = "Not provisioned entity";

        public NotProvisionedEntityHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
