using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(AvoidSPObjectNameStringComparisonHighlighting.CheckId + "-2",
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidSPObjectNameStringComparisonHighlighting.CheckId + ": Avoid SPObject.Name == <string> comparison",
  "Depending on the case, SPObject.Name string based comparison is quite unsafe and might lead to the potential issues.",
  Severity.SUGGESTION
  )]

    [ElementProblemAnalyzer(typeof(IEqualityExpression),
        HighlightingTypes = new[] { typeof(AvoidSPObjectNameStringComparisonHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidSPObjectNameStringComparison2 : SPElementProblemAnalyzer<IEqualityExpression>
    {
        AvoidSPObjectNameStringComparison.ValidationResult _validationResult = AvoidSPObjectNameStringComparison.ValidationResult.Valid;
        
        protected override bool IsInvalid(IEqualityExpression element)
        {
            _validationResult = AvoidSPObjectNameStringComparison.ValidationResult.Valid;
            IList<ICSharpArgumentInfo> arguments = element.Arguments;
            string[] propertyNames = {"Name"};

            if (arguments.Count > 0)
            {
                if (arguments.Any(argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPContentType, propertyNames)))
                    _validationResult |= AvoidSPObjectNameStringComparison.ValidationResult.SPContentType;

                if (arguments.Any(argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPPersistedObject, propertyNames)))
                    _validationResult |= AvoidSPObjectNameStringComparison.ValidationResult.SPPersistedObject;

                if (arguments.Any(argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.PageLayout, propertyNames)))
                    _validationResult |= AvoidSPObjectNameStringComparison.ValidationResult.PageLayout;

                if (arguments.Any(argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPListItem, propertyNames)))
                    _validationResult |= AvoidSPObjectNameStringComparison.ValidationResult.SPListItem;

                if (arguments.Any(argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.TaxonomyItem, propertyNames)))
                    _validationResult |= AvoidSPObjectNameStringComparison.ValidationResult.TaxonomyItem;

                if (arguments.Any(argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPWeb, propertyNames)))
                    _validationResult |= AvoidSPObjectNameStringComparison.ValidationResult.SPWeb;

                if (arguments.Any(argument => argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPPrincipal, propertyNames)))
                    _validationResult |= AvoidSPObjectNameStringComparison.ValidationResult.SPPrincipal;
            }

            return (uint)_validationResult > 0;
        }

        protected override IHighlighting GetElementHighlighting(IEqualityExpression element)
        {
            return new AvoidSPObjectNameStringComparisonHighlighting(element, _validationResult);
        }
    }
}
