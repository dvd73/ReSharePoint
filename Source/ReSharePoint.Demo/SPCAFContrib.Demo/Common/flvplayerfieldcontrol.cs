using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Xml;

namespace SPCAFContrib.Demo.Common
{
    public class ConfigurableUrlFieldControl : BaseFieldControl
    {

    }

    public class FLVPlayerFieldControl : SPFieldUrl
    {

        bool _updating = false;

        public FLVPlayerFieldControl(SPFieldCollection fields, string fieldName)

            : base(fields, fieldName) { }

        public FLVPlayerFieldControl(SPFieldCollection fields, string typeName, string displayName)

            : base(fields, typeName, displayName) { }

        public override void OnUpdated()
        {


            if (_updating)

                return;

            _updating = true;

            {

                base.OnUpdated();

                ConfigureSchemaXml();

            }

            _updating = false;

        }

        public override void OnAdded(SPAddFieldOptions op)
        {

            base.OnAdded(op);

            ConfigureSchemaXml();

        }

        void ConfigureSchemaXml()
        {

            string howOpenUrl = (string)base.GetCustomProperty("HowOpenUrl");

            howOpenUrl = AreStringsEqual("New", howOpenUrl) ? "New" : "Self";

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(base.SchemaXml);

            if (doc.FirstChild.Attributes["HowOpenUrl"] == null)
            {

                XmlAttribute attrib = doc.CreateAttribute("HowOpenUrl");

                attrib.Value = howOpenUrl;

                doc.FirstChild.Attributes.Append(attrib);

            }

            else
            {

                doc.FirstChild.Attributes["HowOpenUrl"].Value = howOpenUrl;

            }

            base.SchemaXml = doc.OuterXml;

        }

        private bool AreStringsEqual(string s1, string s2)
        {

            return (String.Compare(s1, s2, true) == 0);

        }
        
        public override BaseFieldControl FieldRenderingControl
        {

            get
            {

                BaseFieldControl fldControl = new UrlField();
                fldControl.FieldName = InternalName;           

                return fldControl;

            }

        }
              
    }
}