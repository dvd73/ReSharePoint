using JetBrains.Application.UI.Actions.MenuGroups;
using JetBrains.Application.UI.ActionSystem.ActionsRevised.Menu;

namespace ReSharePoint.Common.Actions
{
    [ActionGroup("ReSharper.ReSharePoint", ActionGroupInsertStyles.Submenu, Id = 11001, Text = "reSP")]
    public class ReSharePointMenuGroup : IAction, IInsertLast<VsMainMenuGroup>
    {
        public ReSharePointMenuGroup(AboutAction aboutAction)
        {
        }
    }
}
