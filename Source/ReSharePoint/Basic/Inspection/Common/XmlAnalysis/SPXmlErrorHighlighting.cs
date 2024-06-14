using JetBrains.ReSharper.Daemon.Xml.Highlightings;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml.Tree;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public class SPXmlErrorHighlighting<TElement> : XmlErrorHighlighting, ISPHighlighting<TElement> where TElement : IXmlTreeNode
    {
        public TElement Element { get; set; }

        public SPXmlErrorHighlighting(TElement element, string tooltipText)
            : base(tooltipText, element.GetDocumentRange())
        {
            Element = element;
        }
        
         public override bool IsValid()
        {
            return Element != null && Element.IsValid();
        }
    }
}
