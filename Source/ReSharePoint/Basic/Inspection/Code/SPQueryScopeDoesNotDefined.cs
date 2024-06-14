using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
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

[assembly: RegisterConfigurableSeverity(SPQueryScopeDoesNotDefinedHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPQueryScopeDoesNotDefinedHighlighting.CheckId + ": " + SPQueryScopeDoesNotDefinedHighlighting.Message,
  "All SPViewScope enumeration values are covered all possible developer's intentions. If not specified SharePoint will use Default value.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(SPQueryScopeDoesNotDefinedHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class SPQueryScopeDoesNotDefined : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;

            if (element.IsOneOfTypes(new[] { ClrTypeKeys.SPQuery }))
            {
                ICSharpTypeMemberDeclaration method = element.GetContainingTypeMemberDeclarationIgnoringClosures();
                ILocalVariableDeclaration variable = element.GetContainingNode<ILocalVariableDeclaration>();
                bool inInitializer = false;

                if (element.Initializer != null)
                {
                    inInitializer = element.Initializer.InitializerElements.Any(
                        initializerElement =>
                            initializerElement is INamedMemberInitializer initializer && initializer.NameIdentifier.Name == "ViewAttributes");
                }

                if (!inInitializer && variable != null)
                {
                    string varName = variable.DeclaredElement.ShortName;
                    result = !method.HasPropertySet(ClrTypeKeys.SPQuery, "ViewAttributes", varName);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new SPQueryScopeDoesNotDefinedHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPQueryScopeDoesNotDefinedHighlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPQueryScopeDoesNotDefined;
        public const string Message = "SPQuery Scope is not defined";

        public SPQueryScopeDoesNotDefinedHighlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPQueryScopeDoesNotDefinedFix : SPCSharpQuickFix<SPQueryScopeDoesNotDefinedHighlighting, IObjectCreationExpression>
    {
        private const string ACTION_TEXT = "Specify ViewAttributes as Recursive";
        private const string SCOPED_TEXT = "Specify all ViewAttributes as Recursive";

        public SPQueryScopeDoesNotDefinedFix([NotNull] SPQueryScopeDoesNotDefinedHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IObjectCreationExpression element)
        {
            string expressionFormat = @"{0}.ViewAttributes = ""Scope=\""Recursive\"""";";
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
