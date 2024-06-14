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

[assembly: RegisterConfigurableSeverity(SPC012202Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC012202Highlighting.CheckId + ": " + SPC012202Highlighting.Message,
  "Do not use a <Field Name=\"InternalType\"> element in a custom field type definition.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDeclareInternalTypeInFieldTypes : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            
            if (element.Header.ContainerName == "Field")
            {
                result = element.CheckAttributeValue("Name", new[] {"InternalType"}, true);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC012202Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC012202Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldType.SPC012202;
        public const string Message = "Do not declare field 'InternalType' in FieldTypes";

        public SPC012202Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC012202Fix : SPXmlQuickFix<SPC012202Highlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Remove Field tag";
        private const string SCOPED_TEXT = "Remove all Field tags";

        public SPC012202Fix([NotNull] SPC012202Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string ScopedText => SCOPED_TEXT;

        public override string Text => ACTION_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.Remove();
        }
    }
}
