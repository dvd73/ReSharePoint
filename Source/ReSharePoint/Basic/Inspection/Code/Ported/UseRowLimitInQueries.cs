using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050233Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050233Highlighting.CheckId + ": " + SPC050233Highlighting.Message,
  "Define RowLimit for SPQuery to limit the number of items returned in the query.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(SPC050233Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseRowLimitInQueries : SPElementProblemAnalyzer<IObjectCreationExpression>
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
                            initializerElement is INamedMemberInitializer initializer && initializer.NameIdentifier.Name == "RowLimit");
                }

                if (!inInitializer && variable != null)
                {
                    string varName = variable.DeclaredElement.ShortName;
                    result = !method.HasPropertySet(ClrTypeKeys.SPQuery, "RowLimit", varName);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new SPC050233Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050233Highlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050233;
        public const string Message = "Define RowLimit for SPQuery";

        public SPC050233Highlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
    
}
