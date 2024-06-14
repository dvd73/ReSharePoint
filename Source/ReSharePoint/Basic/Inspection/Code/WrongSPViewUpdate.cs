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

[assembly: RegisterConfigurableSeverity(WrongSPViewUpdateHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  WrongSPViewUpdateHighlighting.CheckId + ": " + WrongSPViewUpdateHighlighting.Message,
  "SPList.DefaultView and SPList.Views[] properties returns a new SPView instance with every call.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression),
        HighlightingTypes = new[] {typeof (WrongSPViewUpdateHighlighting)})]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class WrongSPViewUpdate : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();
            ICSharpExpression qualifier = element.GetExtensionQualifier();

            if (expressionType.IsResolved && qualifier != null &&
                element.IsResolvedAsMethodCall(ClrTypeKeys.SPView, new[] {new MethodCriteria(){ShortName = "Update"}}))
            {
                result = qualifier.IsResolvedAsPropertyUsage(ClrTypeKeys.SPList, new[] { "DefaultView", "Views" });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new WrongSPViewUpdateHighlighting(element);
        }
    }
    
    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class WrongSPViewUpdateHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.WrongSPViewUsage;
        public const string Message = "Multiple SPView instances could not be updated at once";

        public WrongSPViewUpdateHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
