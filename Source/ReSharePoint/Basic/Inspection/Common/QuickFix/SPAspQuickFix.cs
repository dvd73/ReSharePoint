using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Intentions.Scoped;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asp;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Common.AspAnalysis;

namespace ReSharePoint.Basic.Inspection.Common.QuickFix
{
    public abstract class SPAspQuickFix<T1, T2> : SPQuickFix<T1, T2>
        where T1 : SPAspErrorHighlighting<T2>
        where T2 : IAspTag
    {
        protected SPAspQuickFix([NotNull] T1 highlighting)
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
