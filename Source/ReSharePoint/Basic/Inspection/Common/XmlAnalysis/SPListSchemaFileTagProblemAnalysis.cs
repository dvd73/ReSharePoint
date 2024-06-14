using System.Collections.Generic;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public class SPListSchemaFileTagProblemAnalysis : SPXmlFileTagProblemAnalysisBase
    {
        public SPListSchemaFileTagProblemAnalysis(IEnumerable<ISPXmlTagProblemAnalyzer> analyzers) : 
            base("List", "http://schemas.microsoft.com/sharepoint", analyzers)
        {
        }
    }
}
