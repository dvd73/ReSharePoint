using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Util;
using JetBrains.ReSharper.Feature.Services.Daemon.OptionPages;
using JetBrains.ReSharper.Psi.GeneratedCode;
using ReSharePoint.Common.Assets;
using ReSharePoint.Common.Options;
using MessageBox = JetBrains.Util.MessageBox;
using JetBrains.Application.UI.Options;
using JetBrains.Application.UI.Options.OptionsDialog;
using JetBrains.Collections.Viewable;
using JetBrains.DataFlow;
using JetBrains.IDE.UI;
using JetBrains.IDE.UI.Extensions;
using JetBrains.IDE.UI.Extensions.Properties;
using JetBrains.IDE.UI.Extensions.Validation;
using JetBrains.Lifetimes;
using JetBrains.IDE.UI.Options;
using JetBrains.Rider.Model.UIAutomation;
using JetBrains.Threading;

namespace ReSharePoint.Common.UI
{
    /// <summary>
    /// Copied from ParameterNameHintsOptionsPage
    /// </summary>
    [OptionsPage(PID, "reSP", typeof(AssetsThemedIcons.SharePoint2013), ParentId = CodeInspectionPage.PID)]
    public class ReSharePointSettingsOptionPage : BeSimpleOptionsPage
    {
        private const string PID = "CodeInspection.ReSharePointSettingsOptionPageId";
        [NotNull]
        private readonly IconHostBase _iconHost;

        public ReSharePointSettingsOptionPage(
            Lifetime lifetime,
            [NotNull] OptionsPageContext optionsPageContext,
            [NotNull] OptionsSettingsSmartContext optionsSettingsSmartContext,
            [NotNull] IconHostBase iconHost)
            : base(lifetime, optionsPageContext, optionsSettingsSmartContext)
        {
            this._iconHost = iconHost;

            this.AddHeader("Ignored files:");
            var tabbedControl = BeControls.GetTabbedControl();
            var reSharePointOptionsStores = new IReSharePointOptionsStore[]
            {
                new IgnoredJSFileMasksOptionsStore(), new IgnoredXmlFileMasksOptionsStore(),
                new IgnoredOtherFileMasksOptionsStore(), new LoggersMasksOptionsStore()
            };

            foreach (var reSharePointOptionsStore in reSharePointOptionsStores)
            {
                var grid = BeControls.GetGrid();
                grid.AddElement(this.CreateBlackListControl(reSharePointOptionsStore), BeSizingType.Fill);
                var control = grid.WithMargin(BeMargins.Create(5, 0, 5, 0));
                tabbedControl.AddTab(() => control, reSharePointOptionsStore.GetCategory(), this.Lifetime);
            }

            this.AddControl(tabbedControl, true);
        }

        private BeControl CreateBlackListControl(
            IReSharePointOptionsStore reSharePointOptionsStore)
        {
            BlackListViewModel model = new BlackListViewModel(this.Lifetime, this.OptionsSettingsSmartContext, reSharePointOptionsStore);
            return model.SelectedEntry.GetBeSingleSelectionListWithToolbar(model.Entries, this.Lifetime, (entryLt, entry, properties) => new List<BeControl>()
            {
                entry.Pattern.GetBeTextBox(entryLt).WithValidationRule(this.Lifetime, GeneratedUtils.IsValidGeneratedFilesMask, "Pattern is not valid", _iconHost.Transform(JetBrains.Application.UI.Icons.CommonThemedIcons.CommonThemedIcons.Error.Id), ValidationStates.validationError, null, (Func<BeTextBox, IViewableProperty<string>>) null)
            }, this._iconHost, new [] { "Pattern,*" }, true, BeDock.RIGHT).AddButtonWithListAction(BeListAddAction.ADD, i => model.CreateNewEntry()).AddButtonWithListAction<BlackListEntry>(BeListAction.REMOVE, i => model.OnEntryRemoved());
        }

        private class BlackListEntry
        {
            public readonly IProperty<string> Pattern;

            public BlackListEntry([NotNull] Lifetime lifetime, [NotNull] ISimpleSignal entryChanged, string pattern)
            {
                this.Pattern = new Property<string>(lifetime, "BlackListEntry.Pattern", pattern);
                this.Pattern.Change.Advise_NoAcknowledgement(lifetime, entryChanged.Fire);
            }
        }

        private class BlackListViewModel
        {
            private readonly GroupingEvent _entryChanged;
            private readonly Lifetime _lifetime;
            private readonly OptionsSettingsSmartContext _optionContext;
            private readonly IReSharePointOptionsStore _optionStore;

            public BlackListViewModel(
              [NotNull] Lifetime lifetime,
              [NotNull] OptionsSettingsSmartContext context,
              [NotNull] IReSharePointOptionsStore store)
            {
                this._lifetime = lifetime;
                this._optionContext = context;
                this._optionStore = store;
                this._entryChanged = new GroupingEventHost(lifetime, false).CreateEvent(lifetime, "BlackListViewModel.EntryChangedGrouped", TimeSpan.FromMilliseconds(100.0), this.OnEntryChanged);
                var list = store.GetBlackList(context).Select(entry => new BlackListEntry(lifetime, this._entryChanged.Incoming, entry.Trim())).ToList();
                this.Entries = new ListEvents<BlackListEntry>(lifetime, "BlackListViewModel.Entries", list, false);
                this.SelectedEntry = new Property<BlackListEntry>(lifetime, "BlackListViewModel.SelectedEntry");
            }

            public ListEvents<BlackListEntry> Entries { get; }

            public IProperty<BlackListEntry> SelectedEntry { get; }

            public BlackListEntry CreateNewEntry()
            {
                return new BlackListEntry(this._lifetime, this._entryChanged.Incoming, string.Empty);
            }

            public void OnEntryRemoved()
            {
                this._entryChanged.Incoming.Fire();
            }

            private void OnEntryChanged()
            {
                var values = this.Entries.Select(entry => entry.Pattern.Value.Trim()).Where(entry => !entry.IsEmpty());
                this._optionStore.SaveBlackList(this._optionContext, values);
            }
        }

    }
}
