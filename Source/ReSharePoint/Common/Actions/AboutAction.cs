using System;
using System.Windows.Forms;
using JetBrains.Application.DataContext;
using JetBrains.Application.UI.Actions;
using JetBrains.Application.UI.ActionsRevised.Menu;
using System.Reflection;

namespace ReSharePoint.Common.Actions
{
    [Action("About", Id = 11002)]
    public class AboutAction : IExecutableAction
    {
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            if (MessageBox.Show(
                    $"Essential tool to ensure SharePoint code quality. \r\nVersion {Assembly.GetExecutingAssembly().GetName().Version}\r\nVisit our site http://www.subpointsolutions.com/resp",
                    "About reSP Plugin",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("http://www.subpointsolutions.com/resp");
            }
        }
    }
}