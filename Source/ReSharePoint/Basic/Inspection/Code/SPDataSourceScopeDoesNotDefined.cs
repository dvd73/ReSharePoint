using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
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
    [RegisterConfigurableSeverity(SPDataSourceScopeDoesNotDefinedHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPDataSourceScopeDoesNotDefinedHighlighting.CheckId + ": " + SPDataSourceScopeDoesNotDefinedHighlighting.Message,
  "All SPViewScope enumeration values are covered all possible developer's intentions. If not specified SharePoint will use Default value.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(SPDataSourceScopeDoesNotDefinedHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class SPDataSourceScopeDoesNotDefined : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;

            if (element.IsOneOfTypes(new[] { ClrTypeKeys.SPDataSource }))
            {
                ICSharpTypeMemberDeclaration method = element.GetContainingTypeMemberDeclarationIgnoringClosures();
                ILocalVariableDeclaration variable = element.GetContainingNode<ILocalVariableDeclaration>();
                bool inInitializer = false;

                if (element.Initializer != null)
                {
                    inInitializer = element.Initializer.InitializerElements.Any(
                        initializerElement =>
                            initializerElement is INamedMemberInitializer initializer && initializer.NameIdentifier.Name == "Scope");
                }

                if (!inInitializer && variable != null)
                {
                    string varName = variable.DeclaredElement.ShortName;
                    result = !method.HasPropertySet(ClrTypeKeys.SPDataSource, "Scope", varName);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new SPDataSourceScopeDoesNotDefinedHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPDataSourceScopeDoesNotDefinedHighlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPDataSourceScopeDoesNotDefined;
        public const string Message = "SPDataSource scope is not defined";

        public SPDataSourceScopeDoesNotDefinedHighlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPDataSourceScopeDoesNotDefinedFix : SPCSharpQuickFix<SPDataSourceScopeDoesNotDefinedHighlighting, IObjectCreationExpression>
    {
        private const string ACTION_TEXT = "Specify Scope as SPViewScope.Recursive";
        private const string SCOPED_TEXT = "Specify all Scope as SPViewScope.Recursive";

        public SPDataSourceScopeDoesNotDefinedFix([NotNull] SPDataSourceScopeDoesNotDefinedHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IObjectCreationExpression element)
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
