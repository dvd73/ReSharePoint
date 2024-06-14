using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Application.Settings;

namespace ReSharePoint.Common.Options
{
    public class ReSharePointOptionsStore
    {
        protected void ApplyDiff(IContextBoundSettingsStore settingsStore, Expression<Func<ReSharePointSettingsKey, IIndexedEntry<string, string>>> keyExpression, IEnumerable<string> newValues)
        {
            HashSet<string> addedAlreadyFileMasks = new HashSet<string>();

            foreach (string indexedValue in settingsStore.EnumIndexedValues(keyExpression))
            {
                if (!newValues.Contains(indexedValue))
                    settingsStore.RemoveIndexedValue(keyExpression, indexedValue);
                else
                    addedAlreadyFileMasks.Add(indexedValue);
            }

            foreach (string entryIndex in newValues.Where(x => !addedAlreadyFileMasks.Contains(x)))
                settingsStore.SetIndexedValue(keyExpression, entryIndex, entryIndex);
        }
    }
}
