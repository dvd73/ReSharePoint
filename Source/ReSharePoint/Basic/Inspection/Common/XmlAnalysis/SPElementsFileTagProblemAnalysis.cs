using System.Collections.Generic;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public class SPElementsFileTagProblemAnalysis : SPXmlFileTagProblemAnalysisBase
    {
        public SPElementsFileTagProblemAnalysis(IEnumerable<ISPXmlTagProblemAnalyzer> analyzers) :
            base("Elements", "http://schemas.microsoft.com/sharepoint", analyzers)
        {
        }
    }
}
