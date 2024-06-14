using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050222Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050222Highlighting.CheckId + ": " + SPC050222Highlighting.Message,
  "Do not call SPList.Items. Use SPList.GetItems(SPQuery query) instead.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(SPC050222Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseSPListItems : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                var nextToken = element.GetNextMeaningfulToken();
                result = element.IsResolvedAsPropertyUsage(ClrTypeKeys.SPList, new[] {"Items"}) &&
                         (nextToken == null ||
                          (nextToken.GetTokenType() != CSharpTokenType.DOT &&
                           nextToken.GetTokenType() != CSharpTokenType.LBRACKET));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPC050222Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050222Highlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050222;
        public const string Message = "Do not call SPList.Items";

        public SPC050222Highlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
