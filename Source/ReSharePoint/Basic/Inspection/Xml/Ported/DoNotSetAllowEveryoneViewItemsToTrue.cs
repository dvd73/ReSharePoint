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
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC025501Highlighting.CheckId,
  null,
  Consts.SECURITY_GROUP,
  SPC025501Highlighting.CheckId + ": " + SPC025501Highlighting.Message,
  "The attribute ListAllowEveryoneViewItems of a ListDefinition should not be set to true, as it allows every authenticated user of the web application to access the list items when the URL is known.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DoNotSetAllowEveryoneViewItemsToTrue : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListTemplate")
            {
                result = element.CheckAttributeValue("AllowEveryoneViewItems", new[] {"true"}, true);
                if (result)
                    ProblemAttribute = element.GetAttribute("AllowEveryoneViewItems");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC025501Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC025501Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.SPC025501;
        public const string Message = "Do not set 'AllowEveryoneViewItems' to TRUE in ListDefinition";

        public SPC025501Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class DoNotSettAllowEveryoneViewItemsToTrueForListTemplateFix : SPXmlQuickFix<SPC025501Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Remove AllowEveryoneViewItems attribute";
        private const string SCOPED_TEXT = "Remove all AllowEveryoneViewItems attributes";
        public DoNotSettAllowEveryoneViewItemsToTrueForListTemplateFix([NotNull] SPC025501Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute attribute)
        {
            using (WriteLockCookie.Create(attribute.IsPhysical()))
            {
                attribute.Remove();
            }
        }
    }
}
