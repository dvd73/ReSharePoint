using System;
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

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(UseSecureStoreServiceHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  UseSecureStoreServiceHighlighting.CheckId + ": " + UseSecureStoreServiceHighlighting.Message,
  "At some point, it is better to use Secure Store Provider to store credentials/connection strins intead of saving them into web.config, property bags or lists.",
  Severity.SUGGESTION
  )]
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(UseSecureStoreServiceHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseSecureStoreService : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsOneOfTypes(ClrTypeKeys.DBAccessTypes);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new UseSecureStoreServiceHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class UseSecureStoreServiceHighlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.UseSecureStoreService;
        public const string Message = "Consider use Secure Store Service instead of direct db connection.";

        public UseSecureStoreServiceHighlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
