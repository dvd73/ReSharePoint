using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Psi.Xml.Util;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Util;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC019903Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC019903Highlighting.CheckId + ": " + SPC019903Highlighting.Message,
  "If attributes in SharePoint elements are localized with resources the resource key cannot contains spaces.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class RemoveSpacesFromResourceKey : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.GetAttributes().Any(
                attr => CheckSpacesInResourceKey(attr.UnquotedValue)) ||
                   (element.Header.ContainerName != "AllUsersWebPart" && !element.Children<IXmlTag>().Any() &&
                    CheckSpacesInResourceKey(element.InnerText.Trim()));
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC019903Highlighting(element);
        }

        public static bool CheckSpacesInResourceKey(string p)
        {
            return p.Contains("Resources") && p.Contains("$") &&
                   p.Contains(":") && p.Contains(" ");
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC019903Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.General.SPC019903;
        public const string Message = "Remove spaces from resource keys";

        public SPC019903Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC019903Fix : SPXmlQuickFix<SPC019903Highlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Remove spaces";
        private const string SCOPED_TEXT = "Remove all spaces";
        public SPC019903Fix([NotNull] SPC019903Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                foreach (var attr in element.GetAttributes()
                    .Where(attr => RemoveSpacesFromResourceKey.CheckSpacesInResourceKey(attr.UnquotedValue))) XmlAttributeUtil.SetValue(attr, attr.UnquotedValue.Replace(" ", ""));

                if (!element.Children<IXmlTag>().Any() &&
                    RemoveSpacesFromResourceKey.CheckSpacesInResourceKey(element.InnerText.Trim()))
                {
                    var spaceTokens = element.Children<XmlWhitespaceToken>().ToList();

                    if (spaceTokens.Any())
                    {
                        for (int i = spaceTokens.Count - 1; i >= 0; i--)
                        {
                            ModificationUtil.DeleteChild(spaceTokens[i]);
                        }
                    }
                }
            }
        }
    }
}
