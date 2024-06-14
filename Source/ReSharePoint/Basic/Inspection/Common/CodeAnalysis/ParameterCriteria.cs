using JetBrains.ReSharper.Psi;

namespace ReSharePoint.Basic.Inspection.Common.CodeAnalysis
{
    public class ParameterCriteria
    {
        public string Name { get; set; }
        public string ParameterType { get; set; }
        public ParameterKind Kind { get; set; }
    }
}
