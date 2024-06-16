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
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Pro.CodeCompletion.Common;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(CSharpLanguage))]
    public class SPItemEventProperties : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            IPsiSourceFile sourceFile = context.BasicContext.SourceFile;
            if (sourceFile == null || !sourceFile.LanguageType.Is<CSharpProjectFileType>() ||
                context.BasicContext.CodeCompletionType == CodeCompletionType.ImportCompletion)
                return false;

            ITreeNode node = TextControlToPsi.GetElement<ITreeNode>(context.BasicContext.Solution,
                context.BasicContext.TextControl);

            return IsInvalid(node);
        }

        private bool IsInvalid(ITreeNode treeNode)
        {
            bool result = false;

            var element = treeNode.GetContainingNode<IElementAccessExpression>();
            if (element != null)
            {
                IExpressionType expressionType = element.Operand.GetExpressionType();

                if (expressionType.IsResolved)
                {
                    var nextToken = element.Operand.GetNextMeaningfulToken();

                    if (element.Operand.IsResolvedAsPropertyUsage(ClrTypeKeys.SPItemEventProperties, new[] { "AfterProperties", "BeforeProperties" }) &&
                        (nextToken != null && nextToken.GetTokenType() == CSharpTokenType.LBRACKET))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, IItemsCollector collector)
        {
            var solution = context.BasicContext.SourceFile.GetSolution();
            var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document,
                context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset.GetHashCode()));
            CommonHelper.FillFields(context, collector, prefix, solution, project, CompletionCaseType._SPItemEventProperties);

            return base.AddLookupItems(context, collector);
        }
    }
}

