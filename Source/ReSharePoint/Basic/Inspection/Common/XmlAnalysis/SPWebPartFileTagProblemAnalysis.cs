using System.Collections.Generic;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public class SPWebPartFileTagProblemAnalysis : SPXmlFileTagProblemAnalysisBase
    {
        public SPWebPartFileTagProblemAnalysis(IEnumerable<ISPXmlTagProblemAnalyzer> analyzers) :
            base("webParts/webPart", "http://schemas.microsoft.com/WebPart/", analyzers)
        {
        }
    }
}
