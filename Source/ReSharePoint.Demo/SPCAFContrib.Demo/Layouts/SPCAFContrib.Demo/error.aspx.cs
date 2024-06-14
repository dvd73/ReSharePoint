using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCAFContrib.Demo.Layouts.SPCAFContrib.Demo
{
    public partial class usercard : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string jTagScript = @"<script language=""JavaScript\"">
                                    $(document).ready(function(){
			                            $('ul#jcloud-tags').jcloud({
				                            radius:200,
				                            size:30,
				                            step:2,
				                            speed:50,
				                            flats:2,
				                            clock:10,
				                            areal:100,
				                            splitX:100,
				                            splitY:100,
				                            colors:['#000000','#DD2222','#2267DD','#2A872B','#872A7B','#CAC641']
			                            });
                                    });
                                    </script>";
        }
    }
}
