using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
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

[assembly: RegisterConfigurableSeverity(SpecifySPZoneInSPSiteHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SpecifySPZoneInSPSiteHighlighting.CheckId + ": " + SpecifySPZoneInSPSiteHighlighting.Message,
  "Constructor would take default SPUrlZone so that you may have issues with the *.Url properties.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(SpecifySPZoneInSPSiteHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class SpecifySPZoneInSPSite : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;
            TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;

            if (element.IsOneOfTypes(new[] { ClrTypeKeys.SPSite }) && !element.IsOutOfSPContext(element.GetContainingTypeDeclaration()))
            {
                if (arguments.Count > 0 && arguments[0].Value != null && 
                    !arguments[0].Value.ConstantValue.IsString())
                {
                    ICSharpArgument p1 = arguments[0];

                    if (p1.MatchingParameter != null &&
                        p1.MatchingParameter.Element.IsOneOfTheTypes(new[] {ClrTypeKeys.Guid}))
                    {
                        if (arguments.IsSingle()) 
                            result = true;
                        else
                        {
                            // analyse second argument
                            ICSharpArgument p2 = arguments[1];
                            if (p2.MatchingParameter != null)
                            {
                                result = !p2.MatchingParameter.Element.IsOneOfTheTypes(new[] {ClrTypeKeys.SPUserToken}) &&
                                    !p2.MatchingParameter.Element.IsOneOfTheTypes(new[] {ClrTypeKeys.SPUrlZone});
                            }
                        }
                    }
                }
            }
            
            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new SpecifySPZoneInSPSiteHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SpecifySPZoneInSPSiteHighlighting : SPCSharpErrorHighlighting<IObjectCreationExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SpecifySPZoneInSPSite;
        public const string Message = "Missing SPUrlZone parameter in SPSite constructor";

        public SpecifySPZoneInSPSiteHighlighting(IObjectCreationExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SpecifySPZoneInSPSiteFix : SPCSharpQuickFix<SpecifySPZoneInSPSiteHighlighting, IObjectCreationExpression>
    {
        private const string ACTION_TEXT = "Insert SPContext.Current.Site.Zone as second argument";
        private const string SCOPED_TEXT = "Insert SPContext.Current.Site.Zone as second argument everywere";

        public SpecifySPZoneInSPSiteFix([NotNull] SpecifySPZoneInSPSiteHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IObjectCreationExpression element)
        {

            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
            IReferenceExpression referenceExpression =
                elementFactory.CreateReferenceExpression("SPContext.Current.Site.Zone", new object());
            ICSharpArgument p1 = arguments[0];
            ICSharpArgument p2 = elementFactory.CreateArgument(ParameterKind.VALUE, referenceExpression);

            using (WriteLockCookie.Create(element.IsPhysical()))
                element.AddArgumentAfter(p2, p1);
        }
    }
}
