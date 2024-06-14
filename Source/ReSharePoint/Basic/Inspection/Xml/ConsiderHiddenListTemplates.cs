using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(ConsiderHiddenListTemplatesHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  ConsiderHiddenListTemplatesHighlighting.CheckId + ": " + ConsiderHiddenListTemplatesHighlighting.Message,
  "List template should not be created by end user.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class ConsiderHiddenListTemplates : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListTemplate")
            {
                result = !element.CheckAttributeValue("Hidden", new[] {"true"}, true);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new ConsiderHiddenListTemplatesHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class ConsiderHiddenListTemplatesHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.ConsiderHiddenListTemplates;
        public const string Message = "Consider create list template as hidden";

        public ConsiderHiddenListTemplatesHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class ConsiderHiddenListTemplatesFix : SPXmlQuickFix<ConsiderHiddenListTemplatesHighlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Ensure Hidden = \"TRUE\" attribute";
        private const string SCOPED_TEXT = "Ensure all Hidden = \"TRUE\" attributes";
        public ConsiderHiddenListTemplatesFix([NotNull] ConsiderHiddenListTemplatesHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.EnsureAttribute("Hidden", "TRUE");
        }
    }
}
