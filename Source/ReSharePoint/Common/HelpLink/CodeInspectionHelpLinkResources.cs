using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ReSharePoint.Common.HelpLink
{
    /// <summary>
    /// A strongly-typed resource class, for looking up localized strings, etc.
    /// 
    /// </summary>
    [CompilerGenerated]
    [DebuggerNonUserCode]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class CodeInspectionHelpLinkResources
    {
        private static ResourceManager resourceMan;

        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                return resourceMan ?? (resourceMan = new ResourceManager(
                           "ReSharePoint.Properties.Resources",
                           typeof(CodeInspectionHelpLinkResources).Assembly));
            }
        }

        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture { get; set; }

        /// <summary>
        /// Looks up a localized string similar to &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        ///             &lt;CodeInspectionHelpLink&gt;
        ///               &lt;Item Id="CSC510209" Url="http://docs.subpointsolutions.com/resp/CSC510209"/&gt;
        ///               &lt;Item [rest of string was truncated]";.
        /// 
        /// </summary>
        internal static string CodeInspectionHelpLink =>
            ResourceManager.GetString("CodeInspectionHelpLink",
                Culture);

        internal CodeInspectionHelpLinkResources()
        {
        }
    }
}
