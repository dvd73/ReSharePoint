using System;
using System.Linq.Expressions;
using JetBrains.Application.Settings;

namespace ReSharePoint.Common.Options
{
    public static class ReSharePointSettingsAccessor
    {
        public static readonly Expression<Func<ReSharePointSettingsKey, IIndexedEntry<string, string>>> 
            IgnoredJsFileMasks = key => key.IgnoredJsFiles;

        public static readonly Expression<Func<ReSharePointSettingsKey, IIndexedEntry<string, string>>>
            IgnoredXmlFileMasks = key => key.IgnoredXmlFiles;

        public static readonly Expression<Func<ReSharePointSettingsKey, IIndexedEntry<string, string>>>
            IgnoredOtherFileMasks = key => key.IgnoredOtherFiles;

        public static readonly Expression<Func<ReSharePointSettingsKey, IIndexedEntry<string, string>>>
            LoggersMasks = key => key.Loggers;
        
        static ReSharePointSettingsAccessor()
        {
            
        }
    }
}
