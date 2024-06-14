using System;

namespace ReSharePoint.Common.Attributes
{
    /// <summary>
    /// Specify rule applicability depends on IDE project type 
    /// </summary>
    public class ApplicabilityAttribute : Attribute
    {
        public IDEProjectType ProjectType { get; }

        public bool ForFarmSolution =>
            (ProjectType & IDEProjectType.SPFarmSolution) == IDEProjectType.SPFarmSolution ||
            (ProjectType & IDEProjectType.SPSandbox) == IDEProjectType.SPSandbox;

        public ApplicabilityAttribute(IDEProjectType projectType)
        {
            ProjectType = projectType;
        }
    }
}
