using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.CSharpCache;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(SendMailFromWcfServiceHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SendMailFromWcfServiceHighlighting.CheckId + ": " + SendMailFromWcfServiceHighlighting.Message,
  "Sending an e-mail when SPContext is not available could fail.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(SendMailFromWcfServiceHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPServerAPIReferenced)]
    public class SendMailFromWcfService : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                var wcfInterfaceCache = WcfInterfaceCache.GetInstance(element.GetSolution());
                result = element.IsResolvedAsMethodCall(ClrTypeKeys.SPUtility,
                    new[] { new MethodCriteria() { ShortName = "SendEmail" } }) && 
                    element.GetContainingTypeDeclaration().SuperTypes.Any(_ => wcfInterfaceCache.Items.Any(__ => __.Title == _.GetClrName().FullName)) &&
                    !CheckHttpContextClearing(element);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SendMailFromWcfServiceHighlighting(element);
        }

        private bool CheckHttpContextClearing(IReferenceExpression element)
        {
            bool result = false;

            ICSharpTypeMemberDeclaration typeMemberDeclaration = element.GetContainingTypeMemberDeclarationIgnoringClosures();
            foreach (var _ in typeMemberDeclaration.ThisAndDescendants<IAssignmentExpression>())
            {
                if (_.Source is ICSharpLiteralExpression &&
                    _.Dest.IsResolvedAsPropertyUsage(ClrTypeKeys.HttpContext, new[] { "Current" }) &&
                    (_.Source as ICSharpLiteralExpression).Literal.GetTokenType() ==
                    CSharpTokenType.NULL_KEYWORD)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SendMailFromWcfServiceHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SendMailFromWcfService;
        public const string Message = "Avoid SPUtility.SendEmail if SPContext is null";

        public SendMailFromWcfServiceHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SendMailFromWcfServiceFix : SPCSharpQuickFix<SendMailFromWcfServiceHighlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Clear HttpContext";
        private const string SCOPED_TEXT = "Clear all HttpContext";

        public SendMailFromWcfServiceFix([NotNull] SendMailFromWcfServiceHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IReferenceExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            ICSharpStatement newElement = elementFactory.CreateStatement("HttpContext.Current = null;");
            var containingStatement = element.GetContainingStatement();

            if (containingStatement != null)
            {
                using (WriteLockCookie.Create(element.IsPhysical()))
                    ModificationUtil.AddChildBefore(containingStatement.Parent, containingStatement, newElement);
            }
        }
    }
}
