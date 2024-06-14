using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC010213Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC010213Highlighting.CheckId + ": " + SPC010213Highlighting.Message,
  "Property SPSite.ReadLocked is reserved for internal and is not intended to be used directly from your code. Use the IsReadLocked property instead.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IAssignmentExpression), HighlightingTypes = new[] { typeof(SPC010213Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotCallSPSiteReadLocked : SPElementProblemAnalyzer<IAssignmentExpression>
    {
        protected override bool IsInvalid(IAssignmentExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.Dest.IsResolvedAsPropertyUsage(ClrTypeKeys.SPSite, new[] { "ReadLocked" });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IAssignmentExpression element)
        {
            return new SPC010213Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC010213Highlighting : SPCSharpErrorHighlighting<IAssignmentExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC010213;
        public const string Message = "Do not set SPSite.ReadLocked property";

        public SPC010213Highlighting(IAssignmentExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC010213Fix : SPCSharpQuickFix<SPC010213Highlighting, IAssignmentExpression>
    {
        private const string ACTION_TEXT = "Replace to SPSite.IsReadLocked";
        private const string SCOPED_TEXT = "Replace to SPSite.IsReadLocked for all occurrences";
        public SPC010213Fix([NotNull] SPC010213Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IAssignmentExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);

            if (element.Dest != null)
            {
                ICSharpExpression newElement = elementFactory.CreateExpression(element.Dest.GetText().Replace(".ReadLocked", ".IsReadLocked"));

                using (WriteLockCookie.Create(element.IsPhysical()))
                    element.Dest.ReplaceBy(newElement);
            }
        }
    }
}
