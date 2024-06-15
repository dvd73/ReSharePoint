using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [RegisterConfigurableSeverity(SPC056003Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC056003Highlighting.CheckId + ": " + SPC056003Highlighting.Message,
  "Do not instantiate an SPSite object within an event receiver.",
  Severity.WARNING
  )]

    [ElementProblemAnalyzer(typeof(IObjectCreationExpression),
        HighlightingTypes = new[] { typeof(SPC056003Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotInstantiateSPSiteInEventReceiver : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;

            var elementContainingTypeDeclaration = element.GetContainingTypeDeclaration();
            if (elementContainingTypeDeclaration.DeclaredElement != null &&
                element.IsOneOfTypes(new[] { ClrTypeKeys.SPSite}))
            {
                IEnumerable<IDeclaredType> parenClasses = elementContainingTypeDeclaration.DeclaredElement.GetAllSuperClasses();

                result =
                    ClrTypeKeys.SPEventReceivers.Any(
                        eventReceiver =>
                            parenClasses.Any(parenClass => parenClass.GetClrName().Equals(eventReceiver)));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new SPC056003Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC056003Highlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC056003;
        public const string Message = "Do not instantiate SPSite in Receiver";

        public SPC056003Highlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
