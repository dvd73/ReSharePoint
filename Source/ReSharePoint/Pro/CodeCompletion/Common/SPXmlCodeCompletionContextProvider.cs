using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Features.Intellisense.CodeCompletion.Xml;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Xml.Tree;
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
    public class SPXmlCodeCompletionContextProvider : XmlCodeCompletionContextProvider
    {
        public override bool IsApplicable(CodeCompletionContext context)
        {
            if (context.File is IXmlFile)
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

        protected override IXmlFile IsAvailableImpl(CodeCompletionContext context)
        {
            if (context.CodeCompletionType == CodeCompletionType.ImportCompletion)
                return null;
            else
                return context.File as IXmlFile;
        }

        protected override ISpecificCodeCompletionContext CreateSpecificCompletionContext(CodeCompletionContext context, TextLookupRanges ranges, XmlReparsedCodeCompletionContext unterminatedContext)
        {
            return new SPXmlCodeCompletionContext(context, ranges, unterminatedContext);
        }

    }
}
