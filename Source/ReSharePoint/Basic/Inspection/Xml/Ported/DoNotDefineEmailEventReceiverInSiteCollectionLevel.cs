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
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC016003Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC016003Highlighting.CheckId + ": " + SPC016003Highlighting.Message,
  "If email event receivers are used within a feature with scope 'Site' the attribute 'Scope' must be set to 'Web' or the event receiver must be deployed via a web scopes feature.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDefineEmailEventReceiverInSiteCollectionLevel : SPXmlTagProblemAnalyzer
    {
        private IEnumerable<IXmlTag> _emailReceivers;

        public override void Run(IXmlTag element, IHighlightingConsumer consumer)
        {
            if (element.GetProject().IsApplicableFor(this, element.GetPsiModule().TargetFrameworkId))
            {
                if (IsInvalid(element) && _emailReceivers != null)
                {
                    foreach (IXmlTag emailReceiver in _emailReceivers)
                    {
                        SPC016003Highlighting errorHighlighting = new SPC016003Highlighting(element);
                        consumer.ConsumeHighlighting(new HighlightingInfo(emailReceiver.GetDocumentRange(), errorHighlighting));
                    }
                }
            }
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "Receivers" && HasEmailReceived(element))
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
                                pi.ItemType == SharePointProjectItemType.EventHandler &&
                                pi.ElementManifest == sourceFile.Name && pi.Path == sourceFilePath);

                    if (projectItem != null)
                    {
                        FeatureXmlEntity feature = FeatureCache.GetInstance(solution)
                            .Items.FirstOrDefault(
                                f => f.ProjectItems.Any(pi => pi.Equals(projectItem.Id)));

                        if (feature != null && feature.Scope == SPFeatureScope.Site)
                            result = !element.CheckAttributeValue("Scope", new[] {"Web"});
                    }
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            throw new NotImplementedException();
        }

        private bool HasEmailReceived(IXmlTag element)
        {
            IList<IXmlTag> types = element.GetNestedTags<IXmlTag>("Receiver/Type");
            _emailReceivers = types.Where(t => t.InnerText == "EmailReceived");
            return _emailReceivers.Any();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC016003Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.Receiver.SPC016003;
        public const string Message = "Do not define Receiver 'EmailReceived' on SiteCollection level";

        public SPC016003Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
