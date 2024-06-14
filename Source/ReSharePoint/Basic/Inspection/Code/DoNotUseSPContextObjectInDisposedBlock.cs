using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(DoNotUseSPContextObjectInDisposedBlockHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUseSPContextObjectInDisposedBlockHighlighting.CheckId + ": " + DoNotUseSPContextObjectInDisposedBlockHighlighting.Message,
  "Avoid using SPWeb and SPSite from current context in the disposable block.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(DoNotUseSPContextObjectInDisposedBlockHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseSPContextObjectInDisposedBlock : SPElementProblemAnalyzer<IReferenceExpression>
    {
        private string _objectType = String.Empty;
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;
            _objectType = String.Empty;
            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved &&
                element.IsOneOfTypes(new[] { ClrTypeKeys.SPSite, ClrTypeKeys.SPWeb })
                )
            {
                IUsingStatement usingStatement = element.GetContainingNode<IUsingStatement>();
                IObjectCreationExpression ctorExpression = element.GetContainingNode<IObjectCreationExpression>();
                IInvocationExpression invocationExpression = element.GetContainingNode<IInvocationExpression>();
                var t = element.GetText();
                result = usingStatement != null && ctorExpression == null && t.Contains("SPContext") &&
                         invocationExpression == null &&
                         usingStatement.RPar.ComparePositionTo(element) > 0;
                if (result)
                    _objectType = expressionType.ToString();
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new DoNotUseSPContextObjectInDisposedBlockHighlighting(element, _objectType);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseSPContextObjectInDisposedBlockHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotUseSPContextObjectInDisposedBlock;
        public const string Message = "Do not use SPContext objects in the disposable block";
        public string ObjectType { get; set; }

        public DoNotUseSPContextObjectInDisposedBlockHighlighting(IReferenceExpression element, string objectType)
            : base(element, $"{CheckId}: {Message}")
        {
            ObjectType = objectType;
        }
    }

    [QuickFix]
    public class DoNotUseSPContextObjectInDisposedBlockFix : SPCSharpQuickFix<DoNotUseSPContextObjectInDisposedBlockHighlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Create new {0}";
        private const string SCOPED_TEXT = "Create all new {0}";
        private readonly string _objectType;

        public DoNotUseSPContextObjectInDisposedBlockFix([NotNull] DoNotUseSPContextObjectInDisposedBlockHighlighting highlighting)
            : base(highlighting)
        {
            _objectType = highlighting.ObjectType;
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return !_objectType.Contains("SPWeb");
        }

        public override string Text => String.Format(ACTION_TEXT, _objectType);

        public override string ScopedText => String.Format(SCOPED_TEXT, _objectType);

        protected override void Fix(IReferenceExpression element)
        {
            string e = "new SPSite(SPContext.Current.Site.ID, SPContext.Current.Site.Zone)";
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            var referenceExpression =
                elementFactory.CreateExpressionAsIs(e);

            using (WriteLockCookie.Create(element.IsPhysical()))
                ModificationUtil.ReplaceChild(element, referenceExpression);
        }
    }
}
