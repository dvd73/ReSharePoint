using JetBrains.ReSharper.Psi.Tree;

namespace ReSharePoint.Common.Extensions
{
    public static class ITreeNodeExtension
    {
        public static T FindNextSiblingByType<T>(this ITreeNode node, int siblingIndex)
            where T : class, ITreeNode
        {
            int num = -1;
            if (node != null)
            {
                do
                {
                    if (!(node is T))
                    {
                        ++num;
                        if (num == siblingIndex)
                            break;
                        node = node.NextSibling;
                    }
                    else
                        return (T)node;
                } while (node != null);
            }
            return default(T);
        }
    }
}
