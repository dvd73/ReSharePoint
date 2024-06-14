using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public abstract class SPXmlTagProblemAnalyzer : ISPXmlTagProblemAnalyzer
    {
        #region ISPXmlTagProblemAnalyzer members
        public virtual void Run(IXmlTag element, IHighlightingConsumer consumer)
        {
            if (element.GetProject().IsApplicableFor(this, element.GetPsiModule().TargetFrameworkId))
            {
                if (IsInvalid(element))
                {
                    consumer.ConsumeHighlighting(new HighlightingInfo(GetElementDocumentRange(element), GetElementHighlighting(element)));
                }
            }
        }

        public virtual void Init(IXmlFile file)
        {

        }

        public virtual void Done(IXmlFile file, IHighlightingConsumer consumer)
        {

        }
        #endregion
        
        protected abstract bool IsInvalid(IXmlTag element);

        protected abstract IHighlighting GetElementHighlighting(IXmlTag element);

        protected virtual DocumentRange GetElementDocumentRange(IXmlTag element)
        {
            return element.Header.Name.GetDocumentRange();
        }
    }
}
