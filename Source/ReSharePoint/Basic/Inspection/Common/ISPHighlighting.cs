using JetBrains.ReSharper.Psi.Tree;

namespace ReSharePoint.Basic.Inspection.Common
{
    public interface ISPHighlighting<TElement> where TElement : ITreeNode
    {
        TElement Element { get; set; }
    }
}
