using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.Xml;

namespace ReSharePoint.Pro.CodeCompletion.Common
{
    public class SPXmlCodeCompletionContext : XmlCodeCompletionContext
    {
        public object Tag { get; set; }

        public override string ContextId => "SPXmlCodeCompletionContext";

        public SPXmlCodeCompletionContext(CodeCompletionContext context, TextLookupRanges ranges, XmlReparsedCodeCompletionContext unterminatedContext)
            : base(context, ranges, unterminatedContext)
        {
        }

        
    }
}
