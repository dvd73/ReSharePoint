using JetBrains.ReSharper.Daemon.Html.Highlightings;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharePoint.Basic.Inspection.Common.JavaScriptAnalysis
{
    public abstract class SPJavaScriptErrorHighlighting<T> : HtmlWarningHighlighting, ISPHighlighting<T>  where T : IJavaScriptTreeNode
    {
        public T Element { get; set; }

        protected SPJavaScriptErrorHighlighting(T element, string tooltipTextFormat) :
            base(tooltipTextFormat, element.GetDocumentRange())
        {
            Element = element;
        }

        public override bool IsValid()
        {
            return Element.IsValid();
        }
    }
}
