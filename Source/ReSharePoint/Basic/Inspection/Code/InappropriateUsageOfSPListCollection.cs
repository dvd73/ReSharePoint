using System;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Feature.Services.CSharp.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
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
[RegisterConfigurableSeverity(InappropriateUsageOfSPListCollectionHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  InappropriateUsageOfSPListCollectionHighlighting.CheckId + ": Inappropriate SPListCollection usage",
  "SPWeb.Lists collection shoud not be enumerated directly. Avoid using TryGetList method.",
  Severity.ERROR
  )]
    [DaemonStage(StagesBefore = new[] {typeof (LanguageSpecificDaemonStage)})]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class InappropriateUsageOfSPListCollectionDaemonStage : CSharpDaemonStageBase
    {
        protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings,
            DaemonProcessKind processKind, ICSharpFile file)
        {
            IPsiSourceFile sourceFile = process.SourceFile;
            if (sourceFile.HasExcluded(settings)) return null;

            IProject project = sourceFile.GetProject();

            if (project != null)
            {
                if (project.IsApplicableFor(this, sourceFile.PsiModule.TargetFrameworkId))
                {
                    return new ResolverProcess(process, settings, file);
                }
            }

            return null;
        }
        private class ResolverProcess : CSharpDaemonStageProcessBase
        {
            private readonly IContextBoundSettingsStore _settings;

            public ResolverProcess(IDaemonProcess process, IContextBoundSettingsStore settings, ICSharpFile file)
                : base(process, file)
            {
                _settings = settings;
            }

            public override void Execute(Action<DaemonStageResult> commiter)
            {
                if (!DaemonProcess.FullRehighlightingRequired)
                    return;

                HighlightInFile((file, consumer) => file.ProcessThisAndDescendants(this, consumer), commiter,
                    _settings);
            }

            // Anouthere way to implement Execute method
            //public override void Execute(Action<DaemonStageResult> commiter)
            //{
            //    if (!DaemonProcess.FullRehighlightingRequired)
            //        return;

            //    List<HighlightingInfo> highlightings = new List<HighlightingInfo>();

            //    File.ProcessChildren<IExpression>(
            //        expression =>
            //        {
            //            IExpressionType expressionType = expression.GetExpressionType();
            //            if (expressionType.IsResolved)
            //            {
            //                if (expressionType is CSharpMethodGroupTypeBase)
            //                {
            //                    CSharpMethodGroupTypeBase methodGroupType = expressionType as CSharpMethodGroupTypeBase;
            //                    switch (methodGroupType.MethodShortName)
            //                    {
            //                        case "TryGetList":
            //                            highlightings.Add(new HighlightingInfo(expression.GetElementDocumentRange(),
            //                                new InappropriateUsageOfSPListCollectionHighlighting(expression,
            //                                    "Avoid using TryGetList method")));
            //                            break;
            //                        case "OfType":
            //                        case "Cast":
            //                        {
            //                            ReferenceCollection childClasses = expression.FirstChild.GetFirstClassReferences();

            //                            if (childClasses.Count > 0)
            //                            {
            //                                string childClassName = childClasses.First().GetName();
            //                                if (childClassName == "Lists")
            //                                {
            //                                    // web.Lists.Cast<SPList>()
            //                                    highlightings.Add(new HighlightingInfo(expression.GetElementDocumentRange(),
            //                                        new InappropriateUsageOfSPListCollectionHighlighting(expression,
            //                                            String.Format("Avoid all list enumerations via linq {0}<T> expression", methodGroupType.MethodShortName))));
            //                                }
            //                            }    
            //                        }
            //                            break;
            //                    }
            //                }
            //                else if (expression is IReferenceExpression)
            //                {
            //                    IReferenceExpression referenceExpression = expression as IReferenceExpression;
            //                    if (referenceExpression.GetExpressionType().ToString() == "Microsoft.SharePoint.SPListCollection")
            //                    {
            //                        if (expression.Parent is IForeachHeader)
            //                        {
            //                            // foreach (var list in web.Lists)
            //                            highlightings.Add(new HighlightingInfo(expression.GetElementDocumentRange(),
            //                                new InappropriateUsageOfSPListCollectionHighlighting(expression,
            //                                    "Avoid all list enumerations via enumerator calls")));
            //                        }
            //                    }
            //                }
            //                else if (expression is IElementAccessExpression)
            //                {
            //                    IElementAccessExpression accessExpression = expression as IElementAccessExpression;
            //                    if (accessExpression.Operand.GetExpressionType().ToString() ==
            //                        "Microsoft.SharePoint.SPListCollection")
            //                    {
            //                        // web.Lists["Project Task Details"]
            //                        highlightings.Add(new HighlightingInfo(expression.GetElementDocumentRange(),
            //                            new InappropriateUsageOfSPListCollectionHighlighting(expression, "Avoid string based index calls to obtain the list")));
            //                    }
            //                }
            //            }
            //        }
            //        );

            //    HighlightInFile((file, consumer) =>
            //    {
            //        consumer.Highlightings.AddRange(highlightings);
            //    },
            //    commiter, _settings);
            //}

            public override void VisitForeachHeader(IForeachHeader foreachHeader, IHighlightingConsumer consumer)
            {
                IExpressionType expressionType = foreachHeader.Collection.GetExpressionType();
                if (expressionType.IsResolved && expressionType.ToString() == TypeKeys.SPListCollection)
                {
                    consumer.AddHighlighting(
                        new InappropriateUsageOfSPListCollectionHighlighting(foreachHeader,
                            "Avoid all list enumerations via enumerator calls"));
                }

                base.VisitForeachHeader(foreachHeader, consumer);
            }

            public override void VisitElementAccessExpression(IElementAccessExpression elementAccessExpression,
                IHighlightingConsumer consumer)
            {
                TreeNodeCollection<ICSharpArgument> arguments = elementAccessExpression.Arguments;
                IExpressionType expressionType = elementAccessExpression.Operand.GetExpressionType();
                ICSharpArgument firstArgument = arguments.FirstOrDefault();

                // web.Lists["Project Task Details"]
                if (expressionType.IsResolved && expressionType.ToString() == TypeKeys.SPListCollection &&
                    arguments.IsSingle() && firstArgument != null && firstArgument.MatchingParameter != null &&
                    firstArgument.MatchingParameter.Element.Type.IsString())
                {
                    consumer.AddHighlighting(
                        new InappropriateUsageOfSPListCollectionHighlighting(elementAccessExpression,
                            "Avoid string based index calls to obtain the list"));
                }

                base.VisitElementAccessExpression(elementAccessExpression, consumer);
            }

            /// <summary>
            /// Get called when you encounter a method call in the syntax tree.   
            /// </summary>
            /// <param name="referenceExpression"></param>
            /// <param name="consumer"></param>
            public override void VisitReferenceExpression(IReferenceExpression referenceExpression,
                IHighlightingConsumer consumer)
            {
                IDeclaredElement referenceExpressionTarget = referenceExpression.ReferenceExpressionTarget();
                if (referenceExpressionTarget is IMethod method && referenceExpression.FirstChild != null)
                {
                    ITypeElement declaringType = method.GetContainingType();

                    if (declaringType != null)
                    {
                        // web.Lists.TryGetList("Announcements");
                        if (declaringType.GetClrName().Equals(ClrTypeKeys.SPListCollection) &&
                            method.ShortName == "TryGetList")
                        {
                            consumer.AddHighlighting(
                                new InappropriateUsageOfSPListCollectionHighlighting(referenceExpression,
                                    "Avoid using TryGetList method"));
                        }
                        else if (referenceExpression.FirstChild != null)
                        {
                            ReferenceCollection childClasses = referenceExpression.FirstChild.GetFirstClassReferences();
                            if (childClasses.Count > 0)
                            {
                                IReference firstChild = childClasses.FirstOrDefault();
                                if (firstChild != null &&  
                                    firstChild.Resolve().IsValid())
                                {
                                    if (firstChild.Resolve().DeclaredElement != null && 
                                        firstChild.Resolve().DeclaredElement.ShortName == "Lists")
                                    {
                                        switch (method.ShortName)
                                        {
                                                // web.Lists.Cast<SPList>()
                                            case "OfType":
                                            case "Cast":
                                            {
                                                consumer.AddHighlighting(
                                                    new InappropriateUsageOfSPListCollectionHighlighting(
                                                        referenceExpression,
                                                        $"Avoid all list enumerations via linq {method.ShortName}<T> expression"));
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                base.VisitReferenceExpression(referenceExpression, consumer);
            }
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class InappropriateUsageOfSPListCollectionHighlighting : SPCSharpErrorHighlighting<ITreeNode>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.InappropriateUsageOfSPListCollection;

        public InappropriateUsageOfSPListCollectionHighlighting(ITreeNode element, string tooltip)
            : base(element, $"{CheckId}: {tooltip}")
        {
            
        }
    }
}
