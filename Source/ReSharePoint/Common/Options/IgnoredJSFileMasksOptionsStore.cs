using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;

namespace ReSharePoint.Common.Options
{
    public class IgnoredJSFileMasksOptionsStore : ReSharePointOptionsStore, IReSharePointOptionsStore
    {
        public string GetCategory()
        {
            return "JS & TS";
        }

        public IEnumerable<SettingsScalarEntry> GetSettingsEntries(
            OptionsSettingsSmartContext context)
        {
            return Enumerable.Empty<SettingsScalarEntry>();
        }

        public IEnumerable<string> GetBlackList(IContextBoundSettingsStore settingsStore)
        {
            return settingsStore.EnumIndexedValues(ReSharePointSettingsAccessor.IgnoredJsFileMasks).ToArray();
        }

        public void SaveBlackList(IContextBoundSettingsStore settingsStore, IEnumerable<string> values)
        {
            ApplyDiff(settingsStore, ReSharePointSettingsAccessor.IgnoredJsFileMasks, values);
        }
    }
}
