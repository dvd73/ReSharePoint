using System;
using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.Application.UI.Options;

namespace ReSharePoint.Common.Options
{
    public interface IReSharePointOptionsStore
    {
        string GetCategory();

        IEnumerable<SettingsScalarEntry> GetSettingsEntries(
            OptionsSettingsSmartContext context);

        IEnumerable<string> GetBlackList(IContextBoundSettingsStore settingsStore);

        void SaveBlackList(IContextBoundSettingsStore settingsStore, IEnumerable<string> value);
    }
}