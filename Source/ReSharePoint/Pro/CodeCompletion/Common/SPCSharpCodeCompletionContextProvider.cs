using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Settings;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Settings;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.ExpectedTypes;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using JetBrains.Util.Special;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Pro.CodeCompletion.Common
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    [IntellisensePart]
    public class SPCSharpCodeCompletionContextProvider : CSharpCodeCompletionContextProvider
    {
        public override bool IsApplicable(CodeCompletionContext context)
        {
            if (context.File is ICSharpFile)
            {
                IPsiSourceFile sourceFile = context.File.GetSourceFile();
                IProject project = sourceFile?.GetProject();
                if (project != null)
                {
                    return project.IsApplicableFor(this, sourceFile.PsiModule.TargetFrameworkId);
                }
            }

            return false;
        }

        public SPCSharpCodeCompletionContextProvider(CSharpCodeCompletionManager cSharpCodeCompletionManager, ILanguageManager languageManager) : base(cSharpCodeCompletionManager,  languageManager)
        {
            
        }
        //protected override ICSharpFile IsAvailableImpl(CodeCompletionContext context)
        //{
        //    if (context.CodeCompletionType == CodeCompletionType.ImportCompletion)
        //        return null;
        //    else
        //        return context.File as ICSharpFile;
        //}

        //[CanBeNull]
        //protected override ISpecificCodeCompletionContext CreateSpecificCompletionContext(CodeCompletionContext context, TextLookupRanges ranges, XmlReparsedCodeCompletionContext unterminatedContext)
        //{
        //    return new SPCSharpCodeCompletionContext(context, ranges, unterminatedContext);
        //}

        public override ISpecificCodeCompletionContext GetCompletionContext(CodeCompletionContext context)
        {
            ICSharpFile file = context.File as ICSharpFile;
            if (file == null)
                return null;
            if (context.CodeCompletionType == CodeCompletionType.ImportCompletion)
                return null;

            if (context.CodeCompletionType == CodeCompletionType.BasicCompletion && !CodeCompletionManager.GetIntellisenseEnabled(context.ContextBoundSettingsStore))
                return null;

            return null;
            //ISpecificCodeCompletionContext result = base.GetCompletionContext(context);
            //return new SPCSharpCodeCompletionContext(context);
            /*
            IDocument document = context.TextControl.Document;
            TreeTextRange selectedTreeRange = context.SelectedTreeRange;
            ITreeNode treeNode1 = file.FindNodeAt(selectedTreeRange) ?? file;
            int startOffset = context.CaretDocumentRange.TextRange.StartOffset;
            bool thereIsBraceAfterCaret = false;
            if (startOffset < document.GetTextLength() - 1)
                thereIsBraceAfterCaret = document.GetText(new TextRange(startOffset, startOffset + 1)).IsAnyOf("(", "[");
            CSharpReparsedCompletionContext reparseContext1 = CreateReparseContext(context, selectedTreeRange, "__");
            CSharpReparsedCompletionContext reparseContext2 = CreateReparseContext(context, selectedTreeRange, "__;");
            reparseContext1.Init();
            reparseContext2.Init();
            IReference reference = reparseContext1.Reference;
            ITreeNode element = reparseContext1.TreeNode;
            if (element == null)
                return null;
            ITreeNode treeNode2 = element.GetContainingNode<IPreprocessorDirective>(true);
            if (treeNode2 != null)
                element = treeNode2;
            TreeTextRange treeRange = reference != null ? reference.GetTreeTextRange() : GetElementRange(element);
            TextRange completedElementRange = reparseContext1.ToDocumentRange(treeRange);
            if (!completedElementRange.IsValid)
                return null;
            if (!completedElementRange.Contains(context.CaretDocumentRange.TextRange))
                return null;
            CodeCompletionFollowingExpressionContext followingExpression = null;
            if (selectedTreeRange.Length == 0)
                followingExpression = CodeCompletionFollowingExpressionContext.Create(file, selectedTreeRange);
            bool isQualified = reference is IQualifiableReferenceWithGlobalSymbolTable && ((IQualifiableReferenceBase)reference).IsQualified;
            TextLookupRanges textLookupRanges = GetTextLookupRanges(context, completedElementRange);
            TextRange replaceRange = textLookupRanges.ReplaceRange;
            CodeCompletionArgumentsContext argumentsContext = null;
            ICSharpIdentifier identifier = element as ICSharpIdentifier;
            TextRange replaceRangeWithJoinedArguments = replaceRange;
            if (identifier != null)
            {
                TreeTextRange argumentsListRange;
                argumentsContext = CodeCompletionArgumentsContext.Create(identifier, out argumentsListRange);
                if (argumentsListRange.IsValid())
                {
                    replaceRangeWithJoinedArguments = replaceRange.Join(reparseContext1.ToDocumentRange(argumentsListRange));
                    if (!replaceRangeWithJoinedArguments.IsValid)
                        return null;
                }
            }
            string typeArgumentsText = this.TypeArgumentsText(reparseContext1.Reference);

            return
                new SPCSharpCodeCompletionContext(
                    this.myLanguageManager.GetService<ICSharpExpectedTypesProvider, CSharpLanguage>(), context,
                    reparseContext2, file.GetPsiModule(), isQualified,
                    new AccessContextOfCompletedElement(treeNode1), textLookupRanges,
                    treeNode1, reparseContext1, thereIsBraceAfterCaret, followingExpression, typeArgumentsText != null,
                    argumentsContext, typeArgumentsText, replaceRangeWithJoinedArguments, this.myLanguageManager);
             */
        }
    }
}
