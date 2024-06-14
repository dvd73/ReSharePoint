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

[assembly: RegisterConfigurableSeverity(SPC030203Highlighting.CheckId,
  null,
  Consts.SHAREPOINT_SUPPORTABILITY_GROUP,
  SPC030203Highlighting.CheckId + ": " + SPC030203Highlighting.Message,
  "There is no reason to read the connection string of a SharePoint content database except if you want to read from the database which is not supported.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IAssignmentExpression),
        HighlightingTypes = new[] {typeof (SPC030203Highlighting)})]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotReadContentDatabaseConnectionString : SPElementProblemAnalyzer<IAssignmentExpression>
    {
        protected override bool IsInvalid(IAssignmentExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved && element.Source != null)
            {
                result = element.Source.IsResolvedAsPropertyUsage(ClrTypeKeys.SPDatabase,
                    new[] {"DatabaseConnectionString"});
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IAssignmentExpression element)
        {
            return new SPC030203Highlighting(element);
        }
    }
    
    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC030203Highlighting : SPCSharpErrorHighlighting<IAssignmentExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC030203;
        public const string Message = "Do not read ConnectionString from SPContentDatabase";

        public SPC030203Highlighting(IAssignmentExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
