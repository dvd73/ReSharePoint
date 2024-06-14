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

[assembly: RegisterConfigurableSeverity(SPC020205Highlighting.CheckId,
  null,
  Consts.SECURITY_GROUP,
  SPC020205Highlighting.CheckId + ": " + SPC020205Highlighting.Message,
  "The property ListAllowEveryoneViewItems of the SPList object should not be set to true, as it allows every authenticated user of the web application to access the list items when the URL is known.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IAssignmentExpression),
        HighlightingTypes = new[] {typeof (SPC020205Highlighting)})]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotSetSPListAllowEveryoneViewItemsToTrue : SPElementProblemAnalyzer<IAssignmentExpression>
    {
        protected override bool IsInvalid(IAssignmentExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.Dest.IsResolvedAsPropertyUsage(ClrTypeKeys.SPList, new[] {"AllowEveryoneViewItems"}) &&
                         element.Source != null &&
                         element.Source.ConstantValue.IsTrue();
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IAssignmentExpression element)
        {
            return new SPC020205Highlighting(element);
        }
    }
    
    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC020205Highlighting : SPCSharpErrorHighlighting<IAssignmentExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC020205;
        public const string Message = "Do not set 'AllowEveryoneViewItems' for SPList to TRUE";

        public SPC020205Highlighting(IAssignmentExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
