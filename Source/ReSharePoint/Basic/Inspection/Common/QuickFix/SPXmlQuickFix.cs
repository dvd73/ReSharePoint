using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Intentions.Scoped;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;

namespace ReSharePoint.Basic.Inspection.Common.QuickFix
{
    public abstract class SPXmlQuickFix<T1, T2> : SPQuickFix<T1, T2>
        where T1 : SPXmlErrorHighlighting<T2>
        where T2 : IXmlTreeNode
    {
        protected SPXmlQuickFix([NotNull] T1 highlighting)
            : base(highlighting)
        {
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            return _highlighting.IsValid();
        }

        public override FileCollectorInfo FileCollectorInfo => FileCollectorInfo.Default;
    }
}
