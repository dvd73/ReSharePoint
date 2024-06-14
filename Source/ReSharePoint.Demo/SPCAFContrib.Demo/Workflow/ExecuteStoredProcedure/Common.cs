using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Workflow.ComponentModel;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;

namespace SPCAFContrib.Demo.Workflow.ExecuteStoredProcedure
{
    #region  Trace Provider

    internal static class TraceProvider
    {
        // Fields
        private static ulong hTraceLog;
        private static ulong hTraceReg;

        // Methods
        private static unsafe uint ControlCallback(NativeMethods.WMIDPREQUESTCODE RequestCode, IntPtr Context, uint* InOutBufferSize, IntPtr Buffer)
        {
            uint num;
            switch (RequestCode)
            {
                case NativeMethods.WMIDPREQUESTCODE.WMI_ENABLE_EVENTS:
                    hTraceLog = NativeMethods.GetTraceLoggerHandle(Buffer);
                    num = 0;
                    break;

                case NativeMethods.WMIDPREQUESTCODE.WMI_DISABLE_EVENTS:
                    hTraceLog = (ulong)0;
                    num = 0;
                    break;

                default:
                    num = 0x57;
                    break;
            }
            InOutBufferSize[0] = 0;
            return num;
        }

        public static void RegisterTraceProvider()
        {
            Guid controlGuid = SPFarm.Local.TraceSessionGuid;
        }

        public static uint TagFromString(string wzTag)
        {
            Debug.Assert(wzTag.Length == 4);
            return (uint)((((wzTag[0] << 0x18) | (wzTag[1] << 0x10)) | (wzTag[2] << 8)) | wzTag[3]);
        }

        public static void UnregisterTraceProvider()
        {
            Debug.Assert(NativeMethods.UnregisterTraceGuids(hTraceReg) == 0);
        }

        public static void WriteTrace(uint tag, TraceSeverity level, Guid correlationGuid, string exeName, string productName, string categoryName, string message)
        {
            NativeMethods.ULSTrace evnt = new NativeMethods.ULSTrace();
            evnt.Header.Size = (ushort)Marshal.SizeOf(typeof(NativeMethods.ULSTrace));
            evnt.Header.Flags = 0x20000;
            evnt.ULSHeader.dwVersion = 1;
            evnt.ULSHeader.dwFlags = NativeMethods.TraceFlags.TRACE_FLAG_ID_AS_ASCII;
            evnt.ULSHeader.Size = (ushort)Marshal.SizeOf(typeof(NativeMethods.ULSTraceHeader));
            evnt.ULSHeader.Id = tag;
            evnt.Header.Class.Level = (byte)level;
            evnt.ULSHeader.wzExeName = exeName;
            evnt.ULSHeader.wzProduct = productName;
            evnt.ULSHeader.wzCategory = categoryName;
            evnt.ULSHeader.wzMessage = message;
            evnt.ULSHeader.correlationID = correlationGuid;
            if (message.Length < 800)
            {
                ushort num = (ushort)((800 - (message.Length + 1)) * 2);
                evnt.Header.Size = (ushort)(evnt.Header.Size - num);
                evnt.ULSHeader.Size = (ushort)(evnt.ULSHeader.Size - num);
            }
            if (hTraceLog != 0)
            {
                NativeMethods.TraceEvent(hTraceLog, ref evnt);
            }
        }

        // Nested Types
        private static class NativeMethods
        {
            // Fields
            internal const int ERROR_INVALID_PARAMETER = 0x57;
            internal const int ERROR_SUCCESS = 0;
            internal const int TRACE_VERSION_CURRENT = 1;
            internal const int WNODE_FLAG_TRACED_GUID = 0x20000;

            // Methods
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
            internal static extern ulong GetTraceLoggerHandle([In] IntPtr Buffer);
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
            internal static extern unsafe uint RegisterTraceGuids([In] EtwProc cbFunc, [In] void* context, [In] ref Guid controlGuid, [In] uint guidCount, IntPtr guidReg, [In] string mofImagePath, [In] string mofResourceName, out ulong regHandle);
            [DllImport("advapi32.dll", SetLastError = true)]
            internal static extern uint TraceEvent([In] ulong traceHandle, [In] ref ULSTrace evnt);
            [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
            internal static extern uint UnregisterTraceGuids([In] ulong regHandle);

            // Nested Types
            internal unsafe delegate uint EtwProc(TraceProvider.NativeMethods.WMIDPREQUESTCODE requestCode, IntPtr requestContext, uint* bufferSize, IntPtr buffer);

            [StructLayout(LayoutKind.Sequential)]
            internal struct EVENT_TRACE_HEADER
            {
                internal ushort Size;
                internal ushort FieldTypeFlags;
                internal TraceProvider.NativeMethods.EVENT_TRACE_HEADER_CLASS Class;
                internal uint ThreadId;
                internal uint ProcessId;
                internal long TimeStamp;
                internal Guid Guid;
                internal uint ClientContext;
                internal uint Flags;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct EVENT_TRACE_HEADER_CLASS
            {
                internal byte Type;
                internal byte Level;
                internal ushort Version;
            }

            internal enum TraceFlags
            {
                TRACE_FLAG_END = 2,
                TRACE_FLAG_ID_AS_ASCII = 4,
                TRACE_FLAG_MIDDLE = 3,
                TRACE_FLAG_START = 1
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct ULSTrace
            {
                internal TraceProvider.NativeMethods.EVENT_TRACE_HEADER Header;
                internal TraceProvider.NativeMethods.ULSTraceHeader ULSHeader;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal struct ULSTraceHeader
            {
                internal ushort Size;
                internal uint dwVersion;
                internal uint Id;
                internal Guid correlationID;
                internal TraceProvider.NativeMethods.TraceFlags dwFlags;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
                internal string wzExeName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
                internal string wzProduct;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
                internal string wzCategory;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 800)]
                internal string wzMessage;
            }

            internal enum WMIDPREQUESTCODE
            {
                WMI_GET_ALL_DATA,
                WMI_GET_SINGLE_INSTANCE,
                WMI_SET_SINGLE_INSTANCE,
                WMI_SET_SINGLE_ITEM,
                WMI_ENABLE_EVENTS,
                WMI_DISABLE_EVENTS,
                WMI_ENABLE_COLLECTION,
                WMI_DISABLE_COLLECTION,
                WMI_REGINFO,
                WMI_EXECUTE_METHOD
            }
        }

        public enum TraceSeverity
        {
            Assert = 7,
            CriticalEvent = 1,
            Exception = 4,
            High = 20,
            InformationEvent = 3,
            Medium = 50,
            Monitorable = 15,
            Unassigned = 0,
            Unexpected = 10,
            Verbose = 100,
            WarningEvent = 2
        }
    }

    #endregion
    public class WorkflowHistoryTraceListener : TraceListener
    {
        ISharePointService service = null;
        Guid workflowInstanceID = default(Guid);


        public WorkflowHistoryTraceListener(ActivityExecutionContext context, Guid workflowInstanceID)
        {
            service = (ISharePointService)context.GetService(typeof(ISharePointService));
            if (service == null)
            {
                throw new InvalidOperationException();
            }
            this.workflowInstanceID = workflowInstanceID;
        }

        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            service.LogToHistoryList(workflowInstanceID, SPWorkflowHistoryEventType.None, 0, TimeSpan.MinValue, "", message, message);
        }
    }


    [global::System.Serializable]
    public class ExecuteStoredProcedureException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types.
        //

        public ExecuteStoredProcedureException() { }
        public ExecuteStoredProcedureException(string message) : base(message) { }
        public ExecuteStoredProcedureException(string message, Exception inner) : base(message, inner) { }

        protected ExecuteStoredProcedureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    internal static class Common
    {
        /// <summary>
        /// downloads a file over http using a GET request with default credentials
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static Stream GetHttpFileUsingDefaultCredentials(string url)
        {
            return GetHttpFileWCredentials(url, CredentialCache.DefaultNetworkCredentials);
        }
        /// <summary>
        /// downloads a file over http using a GET request with custom credentials
        /// </summary>
        /// <param name="url"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        internal static Stream GetHttpFileWCredentials(string url, ICredentials credentials)
        {
            WebRequest myFileDl = WebRequest.Create(url);
            myFileDl.Credentials = credentials;
            WebResponse myWr = myFileDl.GetResponse();
            return myWr.GetResponseStream();
        }

        /// <summary>
        /// resolves all workflow lookup items within a string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string ProcessStringField(ActivityExecutionContext context, string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            Activity parent = context.Activity;
            while (parent.Parent != null)
            {
                parent = parent.Parent;
            }
            return Helper.ProcessStringField(str, parent, null);
        }

    
        #region Logs Exceptions
        /// <summary>
        /// logs exceptions to sharepoint's workflow history log
        /// </summary>
        /// <param name="e"></param>
        /// <param name="context"></param>
        internal static void LogExceptionToWorkflowHistory(Exception e, ActivityExecutionContext context, Guid wrkflowInstanceID)
        {

            ISharePointService service = (ISharePointService)context.GetService(typeof(ISharePointService));
            if (service == null)
            {
                throw new InvalidOperationException();
            }
            service.LogToHistoryList(wrkflowInstanceID, SPWorkflowHistoryEventType.WorkflowError, 0, TimeSpan.MinValue, "Error", e.ToString(), "");
        }

        internal static void LogExceptionToWorkflowHistory(string exceptionMessage, ActivityExecutionContext context, Guid wrkflowInstanceID)
        {

            ISharePointService service = (ISharePointService)context.GetService(typeof(ISharePointService));
            if (service == null)
            {
                throw new InvalidOperationException();
            }
            service.LogToHistoryList(wrkflowInstanceID, SPWorkflowHistoryEventType.WorkflowError, 0, TimeSpan.MinValue, "Error", exceptionMessage, "");
        }

        internal static void AddCommentWorkflowHistory(string message, ActivityExecutionContext context, Guid wrkflowInstanceID)
        {
            ISharePointService service = (ISharePointService)context.GetService(typeof(ISharePointService));
            if (service == null)
            {
                throw new InvalidOperationException();
            }
            service.LogToHistoryList(wrkflowInstanceID, SPWorkflowHistoryEventType.None, 0, TimeSpan.MinValue, "", message, message);
        }

        internal static ExecuteStoredProcedureException WrapWithFriedlyException(Exception e, string message)
        {
            return new ExecuteStoredProcedureException(message, e);
        }
        #endregion

        // Function to test for Positive Integers.
        internal static bool IsNaturalNumber(String strNumber)
        {
            Regex objNotNaturalPattern = new Regex("[^0-9]");
            Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");

            return !objNotNaturalPattern.IsMatch(strNumber) &&
                    objNaturalPattern.IsMatch(strNumber);
        }
    }
}
