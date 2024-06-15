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

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC015208Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015208Highlighting.CheckId + ": " + SPC015208Highlighting.Message,
  "The attributes 'ResourceFolder' and 'DocumentTemplate' are obsolete and should not be declared in ContentType.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDeclareObsoleteAttributesInContentType : SPXmlTagProblemAnalyzer
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
                        SPC015208Highlighting errorHighlighting = new SPC015208Highlighting(wrongAttribute);
                        consumer.ConsumeHighlighting(new HighlightingInfo(wrongAttribute.GetDocumentRange(), errorHighlighting));
                    }
                }
            }
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _wrongAttributes.Clear();

            if (element.Header.ContainerName == "ContentType")
            {
                _wrongAttributes = element.GetAttributes().Where(a => a.AttributeName == "ResourceFolder" || a.AttributeName == "DocumentTemplate").ToList();
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
    public class SPC015208Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC015208;
        public const string Message = "Do not declare obsolete attributes in ContentType";

        public SPC015208Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC015208Fix : SPXmlQuickFix<SPC015208Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Remove obsolete attributes";
        private const string SCOPED_TEXT = "Remove all obsolete attributes";

        public SPC015208Fix([NotNull] SPC015208Highlighting highlighting)
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
