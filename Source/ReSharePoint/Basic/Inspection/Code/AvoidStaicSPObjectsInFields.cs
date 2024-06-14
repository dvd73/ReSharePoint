using System;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.Html;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(AvoidStaicSPObjectsInFieldsHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  AvoidStaicSPObjectsInFieldsHighlighting.CheckId + ": " + AvoidStaicSPObjectsInFieldsHighlighting.Message,
  "Having static SP-Objects as a fields are quite dangerous.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IMultipleFieldDeclaration), HighlightingTypes = new[] { typeof(AvoidStaicSPObjectsInFieldsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidStaicSPObjectsInFields : SPElementProblemAnalyzer<IMultipleFieldDeclaration>
    {
        protected override bool IsInvalid(IMultipleFieldDeclaration element)
        {
            return element.ModifiersList != null && element.ModifiersList.IsAny(modifier => modifier.HasModifier(CSharpTokenType.STATIC_KEYWORD)) &&
                   element.Declarators.Any(CheckDeclarator);
        }

        protected override IHighlighting GetElementHighlighting(IMultipleFieldDeclaration element)
        {
            return new AvoidStaicSPObjectsInFieldsHighlighting(element);
        }

        private bool CheckDeclarator(IMultipleDeclarationMember declarator)
        {
            bool result = false;
            IClrTypeName[] typeNames = {ClrTypeKeys.SPWeb, ClrTypeKeys.SPSite, ClrTypeKeys.SPFolder, ClrTypeKeys.SPListItem, ClrTypeKeys.SPFile};

            if (declarator is IFieldDeclaration declaration)
            {
                ITypeElement containingType = declaration.DeclaredElement.Type().GetTypeElement<ITypeElement>();
                if (containingType != null)
                {
                    result = typeNames.Any(
                                    typeName =>
                                        containingType.GetClrName().Equals(typeName));
                }
            }

            return result;
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidStaicSPObjectsInFieldsHighlighting : SPCSharpErrorHighlighting<IMultipleFieldDeclaration>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.AvoidStaticSPObjectsInFields;
        public const string Message = "Avoid using static SharePoint type object as field";

        public AvoidStaicSPObjectsInFieldsHighlighting(IMultipleFieldDeclaration element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
