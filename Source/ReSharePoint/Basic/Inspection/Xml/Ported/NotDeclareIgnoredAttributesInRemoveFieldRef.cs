using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Tree;
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

[assembly: RegisterConfigurableSeverity(SPC016602Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC016602Highlighting.CheckId + ": " + SPC016602Highlighting.Message,
  "Do not declare attributes except ID in RemoveFieldRef. All other attributes are ignored by SharePoint.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class NotDeclareIgnoredAttributesInRemoveFieldRef : SPXmlTagProblemAnalyzer
    {
        List<IXmlAttribute> _wrongAttributes = new List<IXmlAttribute>();

        public override void Run(IXmlTag element, IHighlightingConsumer consumer)
        {
            if (element.GetProject().IsApplicableFor(this, element.GetPsiModule().TargetFrameworkId))
            {
                if (IsInvalid(element))
                {
                    foreach (IXmlAttribute wrongAttribute in _wrongAttributes)
                    {
                        SPC016602Highlighting errorHighlighting = new SPC016602Highlighting(wrongAttribute);
                        consumer.ConsumeHighlighting(new HighlightingInfo(wrongAttribute.GetDocumentRange(), errorHighlighting));
                    }
                }
            }
        }
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _wrongAttributes.Clear();

            if (element.Header.ContainerName == "RemoveFieldRef")
            {
                _wrongAttributes = element.GetAttributes().Where(a => a.AttributeName != "ID").ToList();
                result = _wrongAttributes.Any();
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            throw new NotImplementedException();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC016602Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.RemoveFieldRef.SPC016602;
        public const string Message = "Do not declare ignored attributes in RemoveFieldRef";

        public SPC016602Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC016602Fix : SPXmlQuickFix<SPC016602Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Remove ignored attributes";
        private const string SCOPED_TEXT = "Remove all ignored attributes";

        public SPC016602Fix([NotNull] SPC016602Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                element.Remove();
            }
        }
    }
}
