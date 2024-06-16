using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(CSharpLanguage))]
    public class SPWebCollectionAdd : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            IPsiSourceFile sourceFile = context.BasicContext.SourceFile;
            if (sourceFile == null || !sourceFile.LanguageType.Is<CSharpProjectFileType>() ||
                context.BasicContext.CodeCompletionType == CodeCompletionType.ImportCompletion)
                return false;

            ITreeNode node = TextControlToPsi.GetElement<ITreeNode>(context.BasicContext.Solution,
                context.BasicContext.TextControl);

            return node != null && node.Parent is ICSharpLiteralExpression expression && IsInvalid(expression);
        }

        private bool IsInvalid(ICSharpLiteralExpression literalExpression)
        {
            bool result = false;

            var expression = literalExpression.GetContainingNode<IInvocationExpression>();
            var argument = literalExpression.GetContainingNode<ICSharpArgument>();
            if (expression != null && argument != null)
            {
                IExpressionType expressionType = expression.GetExpressionType();
                var stringFullName = typeof (String).FullName;
                var boolFullName = typeof (Boolean).FullName;
                var i = argument.ContainingArgumentList.Arguments.IndexOf(argument);
                if (expressionType.IsResolved && i == 4 && expression.IsResolvedAsMethodCall(ClrTypeKeys.SPWebCollection, new[]
                {
                    // String, String, String, UInt32, String, Boolean, Boolean
                    new MethodCriteria()
                    {
                        ShortName = "Add",
                        Parameters = new[]
                        {
                            new ParameterCriteria()
                            {
                                Name = "strWebUrl",
                                ParameterType = stringFullName,
                                Kind = ParameterKind.VALUE
                            },
                            new ParameterCriteria()
                            {
                                Name = "strTitle",
                                ParameterType = stringFullName,
                                Kind = ParameterKind.VALUE
                            },
                            new ParameterCriteria()
                            {
                                Name = "strDescription",
                                ParameterType = stringFullName,
                                Kind = ParameterKind.VALUE
                            },
                            new ParameterCriteria()
                            {
                                Name = "nLCID",
                                ParameterType = typeof (UInt32).FullName,
                                Kind = ParameterKind.VALUE
                            },
                            new ParameterCriteria()
                            {
                                Name = "strWebTemplate",
                                ParameterType = stringFullName,
                                Kind = ParameterKind.VALUE
                            },
                            new ParameterCriteria()
                            {
                                Name = "useUniquePermissions",
                                ParameterType = boolFullName,
                                Kind = ParameterKind.VALUE
                            },
                            new ParameterCriteria()
                            {
                                Name = "bConvertIfThere",
                                ParameterType = boolFullName,
                                Kind = ParameterKind.VALUE
                            }
                        }
                    }
                }))
                {
                    result = true;
                }
            }

            return result;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            //var solution = context.BasicContext.SourceFile.GetSolution();
            //var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document,
                context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset.GetHashCode()), new[] {'#'});
            Func<TypeInfo.WebTemplate, bool> predicateBuiltIn = x => x.Id != 3;

            prefix = prefix.Split('#')[0];

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicateBuiltIn = x => x.Id != 3 && x.Title.ToLower().Contains(prefix);
            }

            foreach (var webTemplate in TypeInfo.WebTemplates.Where(predicateBuiltIn))
            {
                foreach (TypeInfo.WebTemplateConfiguration webTemplateConfiguration in webTemplate.Configurations)
                {
                    string lookupItemId = $"{webTemplate.Title}#{webTemplateConfiguration.Id}";
                    string lookupItemTitle = webTemplateConfiguration.Title;
                    string lookupItemDescription = webTemplateConfiguration.Description;
                    collector.Add(new WebTemplateConfigurationLookupItem(true, prefix, lookupItemId, lookupItemTitle,
                        lookupItemDescription,
                        context.ReplaceRangeWithJoinedArguments, CompletionCaseType._SPWebCollectionAdd));
                }
            }

            return base.AddLookupItems(context, collector);
        }
    }
}

