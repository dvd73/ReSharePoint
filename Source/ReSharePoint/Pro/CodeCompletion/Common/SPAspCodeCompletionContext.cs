using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.Asp;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.Html;
using JetBrains.ReSharper.Psi.Asp.Html;

namespace ReSharePoint.Pro.CodeCompletion.Common
{
    public class SPAspCodeCompletionContext : AspCodeCompletionContext
    {
        public object Tag { get; set; }

        public override string ContextId => "SPAspCodeCompletionContext";

        public SPAspCodeCompletionContext(CodeCompletionContext context, HtmlReparsedCompletionContext unterminatedContext, TextLookupRanges ranges, IAspDeclaredElementTypes aspDeclaredElementTypes)
            : base(context, unterminatedContext, ranges, aspDeclaredElementTypes)
        {
        }
    }
}
