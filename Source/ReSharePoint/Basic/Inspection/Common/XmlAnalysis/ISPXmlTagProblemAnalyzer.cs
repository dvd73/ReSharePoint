using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml.Tree;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public interface ISPXmlTagProblemAnalyzer
    {
        void Run(IXmlTag element, IHighlightingConsumer consumer);
        void Init(IXmlFile file);
        void Done(IXmlFile file, IHighlightingConsumer consumer);
    }
}
