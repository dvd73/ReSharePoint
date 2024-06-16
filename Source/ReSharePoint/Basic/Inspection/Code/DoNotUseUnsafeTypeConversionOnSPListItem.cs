using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(DoNotUseUnsafeTypeConversionOnSPListItemHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUseUnsafeTypeConversionOnSPListItemHighlighting.CheckId + ": " + DoNotUseUnsafeTypeConversionOnSPListItemHighlighting.Message,
  "SPListItem is untyped entity, so null reference exceptions on nullable types or wrong type conversion exception might be arised.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IElementAccessExpression), HighlightingTypes = new[] { typeof(DoNotUseUnsafeTypeConversionOnSPListItemHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseUnsafeTypeConversionOnSPListItem : SPElementProblemAnalyzer<IElementAccessExpression>
    {
        protected override bool IsInvalid(IElementAccessExpression element)
        {
            bool result = false;
            IExpressionType expressionType = element.GetExpressionType();
            if (expressionType.IsResolved)
            {
                if (element.Operand.GetExpressionType().ToString() == TypeKeys.SPListItem)
                {
                    int i = 0;

                    bool dotFound = false;
                    bool toStringFound = false;
                    foreach (ITreeNode node in element.RightSiblings())
                    {
                        if (i == 0 && node.NodeType.ToString() == "DOT")
                        {
                            dotFound = true;
                        }

                        if (i == 1 && node.NodeType.ToString() == "IDENTIFIER" &&
                            node.Parent is IReferenceExpression expression)
                        {
                            if (expression.NameIdentifier.Name == "ToString")
                                toStringFound = true;
                        }

                        i++;
                        if (i > 1) break;
                    }

                    bool rparenthFound = false;
                    bool lparenthFound = false;
                    bool usertypeUsage = false;
                    foreach (ITreeNode node in element.LeftSiblings())
                    {
                        if (node.IsWhitespaceToken()) continue;

                        if (i == 0 && node.NodeType.ToString() == "RPARENTH")
                        {
                            rparenthFound = true;
                        }

                        if (i == 1 && node.NodeType.ToString() == "USER_TYPE_USAGE")
                        {
                            usertypeUsage = true;
                        }

                        if (i == 2 && node.NodeType.ToString() == "LPARENTH")
                        {
                            lparenthFound = true;
                        }

                        i++;
                        if (i > 2) break;
                    }

                    if (dotFound && toStringFound ||
                        rparenthFound && lparenthFound && usertypeUsage)
                        result = true;
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IElementAccessExpression element)
        {
            return new DoNotUseUnsafeTypeConversionOnSPListItemHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseUnsafeTypeConversionOnSPListItemHighlighting : SPCSharpErrorHighlighting<IElementAccessExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotUseUnsafeTypeConversionOnSPListItem;
        public const string Message = "Do not use unsafe cast of SPListItem";

        public DoNotUseUnsafeTypeConversionOnSPListItemHighlighting(IElementAccessExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
