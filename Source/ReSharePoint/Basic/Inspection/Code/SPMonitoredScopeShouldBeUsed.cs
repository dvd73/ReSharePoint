using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPMonitoredScopeShouldBeUsedHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPMonitoredScopeShouldBeUsedHighlighting.CheckId + ": " + SPMonitoredScopeShouldBeUsedHighlighting.Message,
  "Some recommended practices regarding SPMonitoredScope class utilization.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IClassDeclaration), HighlightingTypes = new[] { typeof(SPMonitoredScopeShouldBeUsedHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class SPMonitoredScopeShouldBeUsed : SPElementProblemAnalyzer<IClassDeclaration>
    {
        protected override bool IsInvalid(IClassDeclaration element)
        {
            bool result = false;

            if (element.DeclaredElement != null)
            {
                IEnumerable<IDeclaredType> parentTypes = element.DeclaredElement.GetAllSuperClasses();

                if (!parentTypes.Any(parentType => parentType.GetClrName().Equals(ClrTypeKeys.Page) || parentType.GetClrName().Equals(ClrTypeKeys.MasterPage)))
                {
                    result = ClrTypeKeys.AllowedWebControls.Any(typeName => parentTypes.Any(
                        parentType => parentType.GetClrName().Equals(typeName)));


                    if (result && (from node in element.ThisAndDescendants<IObjectCreationExpression>().ToEnumerable()
                        select node
                        into objectCreationExpression
                        let expressionType = objectCreationExpression.GetExpressionType()
                        where
                            expressionType.IsResolved &&
                            objectCreationExpression.IsOneOfTypes(new[] {ClrTypeKeys.SPMonitoredScope})
                        select objectCreationExpression).Any())
                    {
                        result = false;
                    }
                }
            }
            

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IClassDeclaration element)
        {
            return new SPMonitoredScopeShouldBeUsedHighlighting(element);
        }

        protected override DocumentRange GetElementRange(IClassDeclaration element)
        {
            return element.NameIdentifier.GetDocumentRange();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPMonitoredScopeShouldBeUsedHighlighting : SPCSharpErrorHighlighting<IClassDeclaration>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPMonitoredScopeShouldBeUsed;
        public const string Message = "SPMonitoredScope should be used";

        public SPMonitoredScopeShouldBeUsedHighlighting(IClassDeclaration element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
