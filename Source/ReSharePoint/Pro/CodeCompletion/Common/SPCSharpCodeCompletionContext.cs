using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.ExpectedTypes;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace ReSharePoint.Pro.CodeCompletion.Common
{
    public class SPCSharpCodeCompletionContext : CSharpCodeCompletionContext
    {
        public object Tag { get; set; }

        public override string ContextId => "SPCSharpCodeCompletionContext";

        public SPCSharpCodeCompletionContext(ICSharpExpectedTypesProvider expectedTypesProvider,
            CodeCompletionContext context, CSharpReparsedCompletionContext terminatedContext, IPsiModule psiModule,
            bool isQualified, IAccessContext accessContext, TextLookupRanges completionRanges, ITreeNode nodeInFile,
            CSharpReparsedCompletionContext unterminatedContext, bool thereIsBraceAfterCaret,
            CodeCompletionFollowingExpressionContext followingExpression, bool hasTypeArguments,
            CodeCompletionArgumentsContext argumentsContext, string typeArgumentsText,
            DocumentRange replaceRangeWithJoinedArguments)
            : base(
                expectedTypesProvider, context, terminatedContext, psiModule, isQualified, accessContext,
                completionRanges, nodeInFile, unterminatedContext, thereIsBraceAfterCaret, followingExpression,
                hasTypeArguments, argumentsContext, typeArgumentsText, replaceRangeWithJoinedArguments)
        {
        }
        
    }
}
