using System.Collections.Generic;

namespace ReSharePoint.Basic.Inspection.Common.CodeAnalysis
{
    public class MethodCriteria
    {
        public string ShortName { get; set; }
        public IEnumerable<ParameterCriteria> Parameters { get; set; }
    }
}
