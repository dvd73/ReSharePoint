using System;
using System.Text;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(AvoidSPObjectNameStringComparisonHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidSPObjectNameStringComparisonHighlighting.CheckId + ": Avoid SPObject.Name == <string> comparison",
  "Depending on the case, SPObject.Name string based comparison is quite unsafe and might lead to the potential issues.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression),
        HighlightingTypes = new[] { typeof(AvoidSPObjectNameStringComparisonHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidSPObjectNameStringComparison : SPElementProblemAnalyzer<IReferenceExpression>
    {
        [Flags]
        public enum ValidationResult : uint
        {
            Valid = 0,
            SPPersistedObject = 1,
            SPContentType = 2,
            PageLayout = 4,
            SPListItem = 8,
            TaxonomyItem = 16,
            SPWeb = 32,
            SPPrincipal = 64
        }

        ValidationResult _validationResult = ValidationResult.Valid;

        protected override bool IsInvalid(IReferenceExpression element)
        {
            _validationResult = ValidationResult.Valid;
            string[] propertyNames = {"Name"};

            if (element.IsResolvedAsMethodCall(ClrTypeKeys.SystemString, new[] { 
                new MethodCriteria(){ShortName = "Equals"}, 
                new MethodCriteria(){ShortName = "Compare"}, 
                new MethodCriteria(){ShortName = "CompareTo"} }))
            {
                if (element.QualifierExpression is IReferenceExpression qualifierExpression)
                {
                    if (qualifierExpression.IsResolvedAsPropertyUsage(ClrTypeKeys.SPContentType, propertyNames))
                        _validationResult |= ValidationResult.SPContentType;

                    if (qualifierExpression.IsResolvedAsPropertyUsage(ClrTypeKeys.SPPersistedObject, propertyNames))
                        _validationResult |= ValidationResult.SPPersistedObject;

                    if (qualifierExpression.IsResolvedAsPropertyUsage(ClrTypeKeys.PageLayout, propertyNames))
                        _validationResult |= ValidationResult.PageLayout;

                    if (qualifierExpression.IsResolvedAsPropertyUsage(ClrTypeKeys.SPListItem, propertyNames))
                        _validationResult |= ValidationResult.SPListItem;

                    if (qualifierExpression.IsResolvedAsPropertyUsage(ClrTypeKeys.TaxonomyItem, propertyNames))
                        _validationResult |= ValidationResult.TaxonomyItem;

                    if (qualifierExpression.IsResolvedAsPropertyUsage(ClrTypeKeys.SPWeb, propertyNames))
                        _validationResult |= ValidationResult.SPWeb;

                    if (qualifierExpression.IsResolvedAsPropertyUsage(ClrTypeKeys.SPPrincipal, propertyNames))
                        _validationResult |= ValidationResult.SPPrincipal;
                }

                if ((uint)_validationResult == 0)
                {
                    ICSharpExpression containingExpression = element.GetContainingExpression();
                    if (containingExpression is IInvocationExpression invocationExpression)
                    {
                        TreeNodeCollection<ICSharpArgument> arguments = invocationExpression.Arguments;

                        if (
                            arguments.Any(
                                argument =>
                                    argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPContentType, propertyNames)))
                            _validationResult |= ValidationResult.SPContentType;

                        if (
                            arguments.Any(
                                argument =>
                                    argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPPersistedObject, propertyNames)))
                            _validationResult |= ValidationResult.SPPersistedObject;

                        if (
                            arguments.Any(
                                argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.PageLayout, propertyNames)))
                            _validationResult |= ValidationResult.PageLayout;

                        if (
                            arguments.Any(
                                argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPListItem, propertyNames)))
                            _validationResult |= ValidationResult.SPListItem;

                        if (
                            arguments.Any(
                                argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.TaxonomyItem, propertyNames)))
                            _validationResult |= ValidationResult.TaxonomyItem;

                        if (
                            arguments.Any(
                                argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPWeb, propertyNames)))
                            _validationResult |= ValidationResult.SPWeb;

                        if (
                            arguments.Any(
                                argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPPrincipal, propertyNames)))
                            _validationResult |= ValidationResult.SPPrincipal;
                    }
                }
            }

            return (uint)_validationResult > 0; 
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new AvoidSPObjectNameStringComparisonHighlighting(element, _validationResult);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidSPObjectNameStringComparisonHighlighting : SPCSharpErrorHighlighting<IExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.AvoidSPObjectNameStringComparison;
        
        public AvoidSPObjectNameStringComparison.ValidationResult ValidationResult;

        private static string GetMessage(AvoidSPObjectNameStringComparison.ValidationResult validationResult)
        {
            const string MESSAGE_TEMPLATE = "Avoid {0}.Name == <string> comparison. ";
            StringBuilder sb = new StringBuilder();

            foreach (var item in EnumUtil.GetValues<AvoidSPObjectNameStringComparison.ValidationResult>())
            {
                if (item != AvoidSPObjectNameStringComparison.ValidationResult.Valid &&
                    (validationResult & item) == item)
                {
                    sb.Append(String.Format(MESSAGE_TEMPLATE, item));
                }
            }

            return sb.ToString().Trim();
        }

        public AvoidSPObjectNameStringComparisonHighlighting(IExpression element, AvoidSPObjectNameStringComparison.ValidationResult validationResult)
            : base(element, $"{CheckId}: {GetMessage(validationResult)}")
        {
            ValidationResult = validationResult;
        }
    }
}
