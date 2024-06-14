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

[assembly: RegisterConfigurableSeverity(DoNotUseDirectorySearcherHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUseDirectorySearcherHighlighting.CheckId + ": " + DoNotUseDirectorySearcherHighlighting.Message,
  "Consider SPUtility.GetPrincipalsInGroup method to perform necessary queries.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(DoNotUseDirectorySearcherHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseDirectorySearcher : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsOneOfTypes(new[] {ClrTypeKeys.DirectorySearcher});
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new DoNotUseDirectorySearcherHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseDirectorySearcherHighlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotUseDirectorySearcher;
        public const string Message = "Do not use DirectorySearcher class to query ActiveDirectory";

        public DoNotUseDirectorySearcherHighlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }

    }
}
