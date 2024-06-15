using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Html.CodeCompletion.Settings;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.Asp;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.Html;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asp.Html;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.ReSharper.Psi.Html.Html;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Pro.CodeCompletion.Common
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    [IntellisensePart]
  public class SPAspCodeCompletionContextProvider : AspCodeCompletionContextProvider
  {
        private readonly IAspDeclaredElementTypes _aspDeclaredElementTypes;

        public override bool IsApplicable(CodeCompletionContext context)
        {
            if (context.File is IAspFile)
            {
                IPsiSourceFile sourceFile = context.File.GetSourceFile();
                IProject project = sourceFile?.GetProject();
                if (project != null)
                {
                    return project.IsApplicableFor(this, sourceFile.PsiModule.TargetFrameworkId);
                }
            }

            return false;
        }

    public SPAspCodeCompletionContextProvider(HtmlCodeCompletionManager htmlCodeCompletionManager, IHtmlDeclaredElementTypes htmlCache, IAspDeclaredElementTypes aspDeclaredElementTypes)
            : base(htmlCodeCompletionManager, htmlCache, aspDeclaredElementTypes)
    {
        _aspDeclaredElementTypes = aspDeclaredElementTypes;
    }

    [CanBeNull]
    protected override ISpecificCodeCompletionContext GetSpecificContext(CodeCompletionContext context, TextLookupRanges ranges, HtmlReparsedCompletionContext unterminatedContext)
    {
        return new SPAspCodeCompletionContext(context, unterminatedContext, ranges, _aspDeclaredElementTypes);
    }
  }
}
