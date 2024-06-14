using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public abstract class SPXmlAttributeProblemAnalyzer : SPXmlTagProblemAnalyzer
    {
        protected IXmlAttribute ProblemAttribute = null;

        #region SPXmlTagProblemAnalyzer members
        public override void Run(IXmlTag element, IHighlightingConsumer consumer)
        {
            if (element.GetProject().IsApplicableFor(this, element.GetPsiModule().TargetFrameworkId))
            {
                if (IsInvalid(element))
                {
                    consumer.ConsumeHighlighting(new HighlightingInfo(GetElementDocumentRange(element), GetElementHighlighting(element)));
                }
            }
        }

        #endregion

        protected override DocumentRange GetElementDocumentRange(IXmlTag element)
        {
            return ProblemAttribute?.GetDocumentRange() ?? element.GetDocumentRange();
        }
    }
}
