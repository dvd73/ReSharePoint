using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Intentions.Scoped;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;

namespace ReSharePoint.Basic.Inspection.Common.QuickFix
{
    public abstract class SPCSharpQuickFix<THighlighting, TElement> : SPQuickFix<THighlighting, TElement>
        where THighlighting : SPCSharpErrorHighlighting<TElement>
        where TElement : ICSharpExpression
    {
        protected SPCSharpQuickFix([NotNull] THighlighting highlighting)
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
