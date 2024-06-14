using System.Collections.Generic;

namespace ReSharePoint.Entities
{
    public class ExcludedItems
    {
        #region properties

        public static readonly List<string> Assemblies = new List<string>()
        {
            "Aspose.*.dll",
            "Telerik.*.dll",
            "System.*.dll",
            "Microsoft.*.dll",
            "DocumentFormat.OpenXml.dll",
            "ClosedXml.dll",
            "NVelocity.dll",
            "Camlex.NET.dll",
            "AjaxControlToolkit.dll",
            "NPOI.dll",
            "NLog.dll",
            "Elmah.dll",
            "log4net.dll",
            "loggr-dotnet.dll",
            "nspring.dll",
            "Logger.NET.dll",
            "NetTrace.dll",
            "csharp-logger.dll",
            "Common.Logging.dll",
            "gsdll32.dll",
            "gsdll64.dll",
            "EntityFramework.dll",
            "EntityFramework.*.dll",
            "Bamboo.*.dll",
            "EPPlus.dll"
        };

        public static readonly List<string> Files = new List<string>()
        {
            "angular.js",
            "angular.*.js",
            "angular-*.js",
            "balloontip.js",
            "bootstrap.*.js",
            "ECOTree.js",
            "excanvas.js",
            "expand.js",
            "flowplayer.*.js",
            "hoverIntent.js",
            "jit-yc.js",
            "jit.js",
            "jquery.js",
            "jquery.*.js",
            "jquery-*.js",
            "jcarousellite.js",
            "knockout*.js",
            "ng-table.js",
            "ng-grid.js",
            "SPJS_Charts*.js",
            "superfish.js",
            "supersubs.js",
            "swfobject.js",
            "thickbox.js",
            "wz_tooltip.js",
            "*.g.cs",
            "*.designer.cs"
        };

        #endregion
    }
}
