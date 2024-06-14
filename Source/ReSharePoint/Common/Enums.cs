using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSharePoint.Common
{
    [Flags]
    public enum IDEProjectType : uint
    {
        Unknown              = 0,
        /// <summary>
        /// SharePoint farm solution
        /// </summary>
        SPFarmSolution      = 1,
        /// <summary>
        /// SharePoint sandbox solution
        /// </summary>
        SPSandbox           = 2,
        /// <summary>
        /// Apps for SharePoint 
        /// </summary>
        SPApp               = 2 << 1,
        /// <summary>
        /// Any project with Microsoft.Sharepoint.dll reference
        /// </summary>
        SPServerAPIReferenced   = 2 << 2
    }

    public enum SPFeatureScope
    {
        Web,
        Site,
        WebApplication,
        Farm
    }

    public enum SharePointProjectItemType
    {
        GenericElement,
        ContentType,
        ListInstance,
        ListDefinition,
        Field,
        WebPart,
        EventHandler,
        Module,
        MappedFolder,
        CustomAction,
        CustomActionGroup,
        Branding,
        MasterPage,
        Workflow,
        CustomActivity
    }

    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
 
    
}
