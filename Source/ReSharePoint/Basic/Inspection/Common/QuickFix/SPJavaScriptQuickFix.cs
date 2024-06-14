using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Intentions.Scoped;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Common.JavaScriptAnalysis;

namespace ReSharePoint.Basic.Inspection.Common.QuickFix
{
    public abstract class SPJavaScriptQuickFix<T1, T2> : SPQuickFix<T1, T2>
        where T1 : SPJavaScriptErrorHighlighting<T2>
        where T2 : IJavaScriptTreeNode
    {
        protected SPJavaScriptQuickFix([NotNull] T1 highlighting)
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
