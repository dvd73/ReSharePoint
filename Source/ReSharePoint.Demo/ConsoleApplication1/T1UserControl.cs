using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;

namespace ConsoleApplication1
{
    class T1UserControl : UserControl
    {
        private void Method1()
        {
            using (new SPMonitoredScope("My Scope Name"))
            {
                ;
            }
        }
    }

     
}
