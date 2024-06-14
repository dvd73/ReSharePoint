using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050250Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050250Highlighting.CheckId + ": " + SPC050250Highlighting.Message,
  "Assign RowLimit for SPQuery within the limited range (default 1 to 2000).",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IAssignmentExpression),
        HighlightingTypes = new[] {typeof (SPC050250Highlighting)})]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AssignSPQueryRowLimitInLimitedRange : SPElementProblemAnalyzer<IAssignmentExpression>
    {
        protected override bool IsInvalid(IAssignmentExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved && element.Dest.IsResolvedAsPropertyUsage(ClrTypeKeys.SPQuery, new[] { "RowLimit" }) && element.Source != null && element.Source.ConstantValue.IsInteger())
            {
                int rowlimit = Convert.ToInt32(element.Source.ConstantValue.Value);
                result = rowlimit < 1 || rowlimit > 2000;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IAssignmentExpression element)
        {
            return new SPC050250Highlighting(element);
        }
    }
    
    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050250Highlighting : SPCSharpErrorHighlighting<IAssignmentExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050250;
        public const string Message = "Assign RowLimit for SPQuery in limited range";

        public SPC050250Highlighting(IAssignmentExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
