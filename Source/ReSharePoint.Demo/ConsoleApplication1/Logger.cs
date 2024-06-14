using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using MOSS.Common.Utilities;

namespace ConsoleApplication1
{
    public class Logger
    {
        public static void LogTrace(string msg)
        {
            MOSSLogger.Instance.Trace(msg);

            SPDiagnosticsService diagnosticsService = SPDiagnosticsService.Local;
            SPDiagnosticsCategory cat = diagnosticsService.Areas["SharePoint Foundation"].Categories["Unknown"];

            string format = "Test trace logging for category {0} in area {1}";
            diagnosticsService.WriteTrace(1, cat, TraceSeverity.Medium, format, cat.Name, cat.Area.Name);
        }
    }
}
