using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
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
    [RegisterConfigurableSeverity(DoNotUseEntityEditorEntitiesHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUseEntityEditorEntitiesHighlighting.CheckId + ": " + DoNotUseEntityEditorEntitiesHighlighting.Message,
  "It is not recommended to use the Entities property to get the selected entities, because using this sometimes causes unexpected behavior.",
  Severity.WARNING
  )]

    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(DoNotUseEntityEditorEntitiesHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseEntityEditorEntities : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsPropertyUsage(ClrTypeKeys.EntityEditor, new[] { "Entities" });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new DoNotUseEntityEditorEntitiesHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseEntityEditorEntitiesHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotUseEntityEditorEntities;
        public const string Message = "Do not use EntityEditor.Entities collection";

        public DoNotUseEntityEditorEntitiesHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class DoNotUseEntityEditorEntitiesFix : SPCSharpQuickFix<DoNotUseEntityEditorEntitiesHighlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Replace to EntityEditor.ResolvedEntities";
        private const string SCOPED_TEXT = "Replace to EntityEditor.ResolvedEntities for all occurrences";

        public DoNotUseEntityEditorEntitiesFix([NotNull] DoNotUseEntityEditorEntitiesHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IReferenceExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            ICSharpExpression newElement = elementFactory.CreateExpression(element.GetText().Replace(".Entities", ".ResolvedEntities"));

            using (WriteLockCookie.Create(element.IsPhysical()))
                element.ReplaceBy(newElement);
        }
    }
}
