using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [RegisterConfigurableSeverity(SPC050228Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050228Highlighting.CheckId + ": " + SPC050228Highlighting.Message,
  "If your code only queries lists, rather than adding, deleting, or editing list items, then you can turn off object change tracking.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(SPC050228Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotEnableObjectTrackingInLinqDataContext : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;

            if (element.IsOneOfTypes(new[] { ClrTypeKeys.DataContext }))
            {
                ICSharpTypeMemberDeclaration method = element.GetContainingTypeMemberDeclarationIgnoringClosures();
                ILocalVariableDeclaration variable = element.GetContainingNode<ILocalVariableDeclaration>();
                IUsingStatement usingStatement = element.GetContainingNode<IUsingStatement>();

                bool inInitializer = false;
                
                if (element.Initializer != null)
                {
                    inInitializer = element.Initializer.InitializerElements.Any(
                        initializerElement =>
                            initializerElement is INamedMemberInitializer initializer && initializer.NameIdentifier.Name == "ObjectTrackingEnabled");
                }

                if (!inInitializer && variable != null && usingStatement != null)
                {
                    string varName = variable.DeclaredElement.ShortName;
                    result = !method.HasPropertySet(ClrTypeKeys.DataContext, "ObjectTrackingEnabled", varName);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new SPC050228Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050228Highlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050228;
        public const string Message = "Set ObjectTrackingEnabled = false";

        public SPC050228Highlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC050228Fix : SPCSharpQuickFix<SPC050228Highlighting, IObjectCreationExpression>
    {
        private const string ACTION_TEXT = "Set ObjectTrackingEnabled = false";
        private const string SCOPED_TEXT = "Set all ObjectTrackingEnabled = false";

        public SPC050228Fix([NotNull] SPC050228Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IObjectCreationExpression element)
        {
            string expressionFormat = @"{0}.ObjectTrackingEnabled = false;";
            var elementFactory = CSharpElementFactory.GetInstance(element);

            ILocalVariableDeclaration variable = element.GetContainingNode<ILocalVariableDeclaration>();
            IUsingStatement usingStatement = element.GetContainingNode<IUsingStatement>();

            if (variable != null && usingStatement?.Body.FirstChild != null)
            {
                string varName = variable.DeclaredElement.ShortName;
                expressionFormat = String.Format(expressionFormat, varName);
                ICSharpStatement containingStatement = element.GetContainingStatement();

                var newStatement = elementFactory.CreateStatement(expressionFormat, new object());
                if (containingStatement != null)
                {
                    using (WriteLockCookie.Create(element.IsPhysical()))
                        ModificationUtil.AddChildAfter(usingStatement.Body.FirstChild, newStatement);
                }
            }
        }
    }
}
