using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(PutSPFileExistsIntoTryCatchBlockHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  PutSPFileExistsIntoTryCatchBlockHighlighting.CheckId + ": " + PutSPFileExistsIntoTryCatchBlockHighlighting.Message,
  "If a file doesn’t exist, it throws an ArgumentException error.",
  Severity.HINT
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(PutSPFileExistsIntoTryCatchBlockHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class PutSPFileExistsIntoTryCatchBlock : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsPropertyUsage(ClrTypeKeys.SPFile, new[] { "Exists" }) && element.FindInnermostExceptionHandler() == null;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new PutSPFileExistsIntoTryCatchBlockHighlighting(element);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class PutSPFileExistsIntoTryCatchBlockHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.PutSPFileExistsIntoTryCatchBlock;
        public const string Message = "Put SPFile.Exists into try ... catch block";

        public PutSPFileExistsIntoTryCatchBlockHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
