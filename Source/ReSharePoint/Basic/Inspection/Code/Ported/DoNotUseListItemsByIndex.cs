using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050224Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050224Highlighting.CheckId + ": " + SPC050224Highlighting.Message,
  "Do not call SPList.Items[int] or SPList.Items[Guid]. Use SPList.GetItemByUniqueId(Guid) or SPList.GetItemById(int) instead.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IElementAccessExpression), HighlightingTypes = new[] { typeof(SPC050224Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseListItemsByIndex : SPElementProblemAnalyzer<IElementAccessExpression>
    {
        public enum ValidationResult : uint
        {
            Valid = 0,
            GetItemById = 1,
            GetItemByUniqueId = 2
        }

        ValidationResult _validationResult = ValidationResult.Valid;

        protected override bool IsInvalid(IElementAccessExpression element)
        {
            _validationResult = ValidationResult.Valid;

            IExpressionType expressionType = element.Operand.GetExpressionType();

            if (expressionType.IsResolved)
            {
                var nextToken = element.Operand.GetNextMeaningfulToken();

                if (element.Operand.IsResolvedAsPropertyUsage(ClrTypeKeys.SPList, new[] {"Items"}) &&
                    (nextToken != null && nextToken.GetTokenType() == CSharpTokenType.LBRACKET))
                {
                    TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
                    ICSharpArgument firstArgument = arguments.FirstOrDefault();
                    if (firstArgument != null && firstArgument.MatchingParameter != null)
                    {
                        _validationResult = ValidationResult.GetItemById;
                        if (firstArgument.MatchingParameter.Element.Type.IsGuid())
                            _validationResult = ValidationResult.GetItemByUniqueId;
                    }
                }
            }

            return (uint)_validationResult > 0;
        }

        protected override IHighlighting GetElementHighlighting(IElementAccessExpression element)
        {
            return new SPC050224Highlighting(element, _validationResult);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050224Highlighting : SPCSharpErrorHighlighting<IElementAccessExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050224;
        public const string Message = "Do not call SPList.Items[]";

        public DoNotUseListItemsByIndex.ValidationResult ValidationResult;

        public SPC050224Highlighting(IElementAccessExpression element, DoNotUseListItemsByIndex.ValidationResult validationResult)
            : base(element, $"{CheckId}: {Message}")
        {
            ValidationResult = validationResult;
        }
    }

    [QuickFix]
    public class SPC050224Fix : SPCSharpQuickFix<SPC050224Highlighting, IElementAccessExpression>
    {
        public SPC050224Fix([NotNull] SPC050224Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text
        {
            get
            {
                switch (_highlighting.ValidationResult)
                {
                    case DoNotUseListItemsByIndex.ValidationResult.GetItemById:
                        return "Replace to SPList.GetItemById";
                    case DoNotUseListItemsByIndex.ValidationResult.GetItemByUniqueId:
                        return "Replace to SPList.GetItemByUniqueId";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override string ScopedText => Text + " for all occurrences";

        protected override void Fix(IElementAccessExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
            ICSharpArgument firstArgument = arguments.FirstOrDefault();

            if (firstArgument != null && firstArgument.MatchingParameter != null)
            {
                string replacement = ".GetItemById";
                if (firstArgument.MatchingParameter.Element.Type.IsGuid())
                    replacement = ".GetItemByUniqueId";

                ICSharpExpression newElement =
                    elementFactory.CreateExpression(
                        element.GetText().Replace(".Items", replacement).Replace("[", "(").Replace("]", ")"));

                using (WriteLockCookie.Create(element.IsPhysical()))
                    element.ReplaceBy(newElement);
            }
        }
    }
}
