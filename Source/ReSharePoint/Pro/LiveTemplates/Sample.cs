namespace ReSharePoint.Pro.LiveTemplates
{
    /*
    [MacroDefinition("rwep", 
        ShortDescription = "using SPSecurity.RunWithElevatedPrivileges()", 
        LongDescription = "using SPSecurity.RunWithElevatedPrivileges()")]
    public class RWEPMacroDef : SimpleMacroDefinition
    {
    }

     [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SP2013FarmSolution)]
    [MacroImplementation(Definition = typeof(RWEPMacroDef), ScopeProvider = typeof(CSharpScopeProvider))]
    public class RWEPMacroImpl : QuickParameterlessMacro
    {
         
        public override string QuickEvaluate(string value)
        {
            var windowsIdentity = WindowsIdentity.GetCurrent();
            //Debug.Assert(windowsIdentity != null);
            string name = windowsIdentity.Name;

            if (value != null && value.Contains("\\"))
            {
                name = name.Replace("\\", "\\\\");
            }

            return name;
        }
    }
     */
}
