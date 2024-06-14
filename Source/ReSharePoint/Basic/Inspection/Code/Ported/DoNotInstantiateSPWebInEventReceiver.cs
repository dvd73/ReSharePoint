using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC056004Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC056004Highlighting.CheckId + ": " + SPC056004Highlighting.Message,
  "Do not instantiate an SPWeb object within an event receiver.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression),
        HighlightingTypes = new[] { typeof(SPC056004Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotInstantiateSPWebInEventReceiver : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            var elementContainingTypeDeclaration = element.GetContainingTypeDeclaration();
            if (elementContainingTypeDeclaration?.DeclaredElement != null && element.IsResolvedAsMethodCall(ClrTypeKeys.SPSite, new [] {new MethodCriteria(){ ShortName = "OpenWeb"}}))
            {
                IEnumerable<IDeclaredType> parenClasses = elementContainingTypeDeclaration.DeclaredElement.GetAllSuperClasses();

                result =
                    ClrTypeKeys.SPEventReceivers.Any(
                        eventReceiver =>
                            parenClasses.Any(parenClass => parenClass.GetClrName().Equals(eventReceiver)));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPC056004Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC056004Highlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC056004;
        public const string Message = "Do not instantiate SPWeb in Receiver";

        public SPC056004Highlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
