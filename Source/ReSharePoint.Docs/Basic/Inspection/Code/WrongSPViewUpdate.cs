﻿using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReSharePoint.Docs.Basic.Inspection.Code
{
    [TestClass]
    public class WrongSPViewUpdate
    {
        [TestMethod]
        public void IncorrectSPViewUpdate(SPList list)
        {
            // won't save view (!!!)
            list.Views["TestView1"].DefaultView = true;
            list.Views["TestView1"].Update();

            // saves the "NewField2" only (!!!)
            list.DefaultView.ViewFields.Add("NewField1");
            list.DefaultView.ViewFields.Add("NewField2");
            list.DefaultView.Update();

        }

        [TestMethod]
        public void CorrectSPViewUpdate(SPList list)
        {
            // save SPView instance into a local varible, always!
            var view = list.Views["TestView1"];

            view.DefaultView = true;
            view.Paged = true;

            view.Update();
        }
    }
}
