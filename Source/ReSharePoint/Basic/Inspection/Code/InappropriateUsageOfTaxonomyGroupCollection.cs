using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(InappropriateUsageOfTaxonomyGroupCollectionHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  InappropriateUsageOfTaxonomyGroupCollectionHighlighting.CheckId + ": " + InappropriateUsageOfTaxonomyGroupCollectionHighlighting.Message,
  "Consider fetching term group or term set by GUID or string comporation by collection enumeration.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IElementAccessExpression), HighlightingTypes = new[] { typeof(InappropriateUsageOfTaxonomyGroupCollectionHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class InappropriateUsageOfTaxonomyGroupCollection : SPElementProblemAnalyzer<IElementAccessExpression>
    {
        protected override bool IsInvalid(IElementAccessExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
                ICSharpArgument firstArgument = arguments.FirstOrDefault();

                if (arguments.IsSingle() && firstArgument.MatchingParameter != null &&
                    firstArgument.MatchingParameter.Element.Type.IsString() &&
                    element.Operand.IsOneOfTypes(new[]
                    {
                        ClrTypeKeys.TermStoreCollection, 
                        ClrTypeKeys.TermCollection, 
                        ClrTypeKeys.GroupCollection, 
                        ClrTypeKeys.TermSetCollection
                    }))
                {
                    result = true;
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IElementAccessExpression element)
        {
            return new InappropriateUsageOfTaxonomyGroupCollectionHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class InappropriateUsageOfTaxonomyGroupCollectionHighlighting : SPCSharpErrorHighlighting<IElementAccessExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.InappropriateUsageOfTaxonomyGroupCollection;
        public const string Message = "Avoid taxonomy collection string based index call";

        public InappropriateUsageOfTaxonomyGroupCollectionHighlighting(IElementAccessExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
