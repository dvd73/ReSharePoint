using System;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(DoNotChangeSPPersistedObjectHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotChangeSPPersistedObjectHighlighting.CheckId + ": " + DoNotChangeSPPersistedObjectHighlighting.Message,
  "SharePoint 2010+ has security feature to all objects inheriting from SPPersistedObject. This feature explicitly disallows modification of SPPersistedObject objects from content web applications.",
  Severity.ERROR
  )]

    [ElementProblemAnalyzer(typeof(IReferenceExpression),
        HighlightingTypes = new[] { typeof(DoNotChangeSPPersistedObjectHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotChangeSPPersistedObject : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;
            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved &&
                element.IsResolvedAsMethodCall(ClrTypeKeys.SPPersistedObject, new[] { new MethodCriteria() { ShortName = "Update" } }))
            {
                var project = element.GetProject();
                string assemblyFullName = project.GetOutputAssemblyFullName();
                var containingTypeDeclaration = element.GetContainingTypeDeclaration().CLRName;
                string containerReadableName = element.ContainerReadableName();

                result =
                    !FeatureCache.GetInstance(project.GetSolution())
                        .GetReceivers(SPFeatureScope.WebApplication)
                        .Any(
                            receiver =>
                                String.Equals(receiver.ReceiverAssembly, assemblyFullName,
                                    StringComparison.OrdinalIgnoreCase) &&
                                (String.Equals(receiver.ReceiverClass, containingTypeDeclaration,
                                    StringComparison.OrdinalIgnoreCase) ||
                                 receiver.ReceiverClassReferences.Any(
                                     reference => reference.Equals(containerReadableName)))
                        );
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new DoNotChangeSPPersistedObjectHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotChangeSPPersistedObjectHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotChangeSPPersistedObject;
        public const string Message = "Do not change SPPersistedObject in the content web application";

        public DoNotChangeSPPersistedObjectHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
