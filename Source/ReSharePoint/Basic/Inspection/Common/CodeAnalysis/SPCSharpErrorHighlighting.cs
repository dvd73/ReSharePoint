using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharePoint.Basic.Inspection.Common.CodeAnalysis
{
    public abstract class SPCSharpErrorHighlighting<TElement> : ISPHighlighting<TElement>, IHighlighting
        where TElement : ITreeNode
    {
        public TElement Element { get; set; }

        protected SPCSharpErrorHighlighting(TElement element, string toolTip)
        {
            ToolTip = toolTip;
            Element = element;
        }

        #region IHighlighting Members
        public DocumentRange CalculateRange()
        {
            return Element.GetNavigationRange();
        }
        public string ToolTip { get; }

        public string ErrorStripeToolTip => ToolTip;

        public int NavigationOffsetPatch => 0;

        public bool IsValid()
        {
            return Element != null && Element.IsValid();
        }

        #endregion
    }
}
