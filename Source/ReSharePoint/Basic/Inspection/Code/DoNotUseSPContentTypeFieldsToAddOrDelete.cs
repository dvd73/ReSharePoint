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

[assembly: RegisterConfigurableSeverity(DoNotUseSPContentTypeFieldsToAddOrDeleteHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUseSPContentTypeFieldsToAddOrDeleteHighlighting.CheckId + ": " + DoNotUseSPContentTypeFieldsToAddOrDeleteHighlighting.Message,
  "SPContentType contains two collections, Fields (of type SPFieldCollection) and FieldLinks (of type SPFieldLinkCollection). Even if the object model appears to support addition or deletion of fields using the Fields collection, an exception is thrown if you try to do so.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(DoNotUseSPContentTypeFieldsToAddOrDeleteHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseSPContentTypeFieldsToAddOrDelete : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;
            IExpressionType expressionType = element.GetExpressionType();
            if (expressionType.IsResolved &&
                element.IsResolvedAsPropertyUsage(ClrTypeKeys.SPContentType,
                    new[] {"Fields"}))
            {
                ICSharpExpression containingExpression = element.GetContainingExpression();
                if (containingExpression is IReferenceExpression parentExpression)
                {
                    result = parentExpression.IsResolvedAsMethodCall(ClrTypeKeys.SPFieldCollection,
                        new[] {new MethodCriteria() {ShortName = "Add"}, new MethodCriteria() {ShortName = "Delete"}});
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new DoNotUseSPContentTypeFieldsToAddOrDeleteHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseSPContentTypeFieldsToAddOrDeleteHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotUseSPContentTypeFieldsToAddOrDelete;
        public const string Message = "Do not use SPContentType.Fields collection to add or delete items";
        
        public DoNotUseSPContentTypeFieldsToAddOrDeleteHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
