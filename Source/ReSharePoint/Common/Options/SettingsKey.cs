using JetBrains.Application.Settings;
using JetBrains.ReSharper.Resources.Settings;

namespace ReSharePoint.Common.Options
{
    [SettingsKey(typeof(CodeInspectionSettings), "reSP settings")]
    public class ReSharePointSettingsKey
    {
        [SettingsIndexedEntry("List of ignored JavasSript files")]
        public IIndexedEntry<string, string> IgnoredJsFiles { get; set; }

        [SettingsIndexedEntry("List of ignored Xml files")]
        public IIndexedEntry<string, string> IgnoredXmlFiles { get; set; }

        [SettingsIndexedEntry("List of other ignored files")]
        public IIndexedEntry<string, string> IgnoredOtherFiles { get; set; }

        [SettingsIndexedEntry("List of logging classes")]
        public IIndexedEntry<string, string> Loggers { get; set; }
    }
}