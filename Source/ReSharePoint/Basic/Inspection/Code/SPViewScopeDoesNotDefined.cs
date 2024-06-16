using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(SPViewScopeDoesNotDefinedHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPViewScopeDoesNotDefinedHighlighting.CheckId + ": " + SPViewScopeDoesNotDefinedHighlighting.Message,
  "All SPViewScope enumeration values are covered all possible developer's intentions. If not specified SharePoint will use Default value.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(SPViewScopeDoesNotDefinedHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class SPViewScopeDoesNotDefined : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved && element.IsResolvedAsMethodCall(ClrTypeKeys.SPViewCollection, new [] {new MethodCriteria(){ShortName = "Add"}} ))
            {
                ICSharpTypeMemberDeclaration method = element.GetContainingTypeMemberDeclarationIgnoringClosures();
                ILocalVariableDeclaration variable = element.GetContainingNode<ILocalVariableDeclaration>();

                if (variable != null)
                {
                    string varName = variable.DeclaredElement.ShortName;
                    result = !method.HasPropertySet(ClrTypeKeys.SPView, "Scope", varName);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPViewScopeDoesNotDefinedHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPViewScopeDoesNotDefinedHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPViewScopeDoesNotDefined;
        public const string Message = "SPView scope is not defined";
        
        public SPViewScopeDoesNotDefinedHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPViewScopeDoesNotDefinedFix : SPCSharpQuickFix<SPViewScopeDoesNotDefinedHighlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Specify Scope as SPViewScope.Recursive";
        private const string SCOPED_TEXT = "Specify all Scope as SPViewScope.Recursive";

        public SPViewScopeDoesNotDefinedFix([NotNull] SPViewScopeDoesNotDefinedHighlighting highlighting)    
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IReferenceExpression element)
        {
            string expressionFormat = "{0}.Scope = SPViewScope.Recursive;";
            var elementFactory = CSharpElementFactory.GetInstance(element);

            ILocalVariableDeclaration variable = element.GetContainingNode<ILocalVariableDeclaration>();

            if (variable != null)
            {
                string varName = variable.DeclaredElement.ShortName;
                expressionFormat = String.Format(expressionFormat, varName);
                ICSharpStatement containingStatement = element.GetContainingStatement();

                var newStatement = elementFactory.CreateStatement(expressionFormat, new object());
                if (containingStatement != null)
                {
                    using (WriteLockCookie.Create(element.IsPhysical()))
                        ModificationUtil.AddChildAfter(containingStatement, newStatement);
                }
            }
        }
    }
}
