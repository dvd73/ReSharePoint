using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Asp.Tree;

namespace ReSharePoint.Basic.Inspection.Common.AspAnalysis
{
    public abstract class SPAspErrorHighlighting<T> : ISPHighlighting<T>, IHighlighting where T : IAspTag
    {
        public T Element { get; set; }

        protected SPAspErrorHighlighting(T element, string toolTip)
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
