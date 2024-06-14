//--------------------------------------------------------------------
// File: SimpleWebPart.cs
//
// Purpose: A sample Web Part that demonstrates how to create a basic
// Web Part.
//--------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.Utilities;
using System.Web.UI.HtmlControls;

namespace ConsoleApplication1
{
    /// <summary>
    /// This Web Part changes its title and implements a custom property.
    /// </summary>
    [XmlRoot(Namespace = "WebPartLibrary1")]
    public class SimpleWebPart : WebPart
    {
        private const string defaultText = "hello";
        private string text = defaultText;

        // Declare variables for HtmlControls user interface elements.
        HtmlButton _mybutton;
        HtmlInputText _mytextbox;

        // Event handler for _mybutton control that sets the
        // Title property to the value in _mytextbox control.
        public void _mybutton_click(object sender, EventArgs e)
        {
            this.Title = _mytextbox.Value;
            try
            {
                this.SaveProperties = true;
            }
            catch
            {
                Caption = "Error... Could not save property.";
            }
        }

        // Override the ASP.NET Web.UI.Controls.CreateChildControls 
        // method to create the objects for the Web Part's controls.      
        protected override void CreateChildControls()
        {
            // Create _mytextbox control.
            _mytextbox = new HtmlInputText();
            _mytextbox.Value = "";
            Controls.Add(_mytextbox);

            // Create _mybutton control and wire its event handler.
            _mybutton = new HtmlButton();
            _mybutton.InnerText = "Set Web Part Title";
            _mybutton.ServerClick += new EventHandler(_mybutton_click);
            Controls.Add(_mybutton);
        }

        [Browsable(true), Category("Miscellaneous"),
        DefaultValue(defaultText),
        WebPartStorage(Storage.Personal),
        FriendlyName("Text"), Description("Text Property")]
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        protected override void RenderWebPart(HtmlTextWriter output)
        {
            RenderChildren(output);
            // Securely write out HTML
            output.Write("<BR>Text Property: " + SPEncode.HtmlEncode(Text));
        }
    }
}
