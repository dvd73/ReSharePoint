using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SubPoint2.DocsImport.Data;
using SubPoint2.DocsImport.Services;

using SPMeta2.Utils;
using SubPoint2.DocsImport.Resources;
using System.IO;
using System.Linq;
using SPMeta2.Standard.Definitions.Taxonomy;
using SubPoint2.DocsImport.Base;

namespace SubPoint2.DocsImport
{
    /// <summary>
    /// Summary description for Publishing
    /// </summary>
    [TestClass]
    public class reSPPublishing : BasePublishingTest
    {
        #region constructors

        public reSPPublishing()
        {
            VSProjectPath = @"C:\Projects\VSOnline\ReSharePoint\Source\ReSharePoint.Docs";
        }

        #endregion

        #region properties

        protected override DocPublishingServiceResult ProcessedPages
        {
            get
            {
                var pages = base.ProcessedPages;

                foreach (var p in pages.Pages)
                {
                    if (!p.Terms.Contains(91))
                        p.Terms.Add(91);
                }

                foreach (var p in pages.Pages)
                {
                    if (p.Title.ToUpper().StartsWith("RESP"))
                    {
                        p.Title = p.Title.Split(':')[1].Trim();
                    }
                }

                return pages;
            }
        }

        #endregion

        #region reSP

        /// <summary>
        /// Creates documentation and opens a local folder/
        /// </summary>
        [TestMethod]
        [TestCategory("Publishing.reSP.Local")]
        public void Publish_DocumentationToFolder()
        {
            System.Diagnostics.Process.Start(ProcessedPages.PagesFolderPath);
        }

        /// <summary>
        /// Publishes everything to Wordpress.
        /// </summary>
        [TestMethod]
        [TestCategory("Publishing.reSP.All")]
        public void Publish_AllDocumentationToWordpress()
        {
            CreateOrUpdatePagesByName(ProcessedPages.Pages);
        }


        /// <summary>
        /// Publishes only rule pages to Wordpress.
        /// Check IsRulePage method for the details.
        /// </summary>
        [TestMethod]
        [TestCategory("Publishing.reSP.Parts")]
        public void Publish_AllRulePagesToWordpress()
        {
            var pages = ProcessedPages.Pages.Where(p => IsRulePage(p.ParentPageId)).ToList();

            CreateOrUpdatePagesByName(pages);
        }

        /// <summary>
        /// Publishes only basic page to Wordpress.
        /// </summary>
        [TestMethod]
        [TestCategory("Publishing.reSP.Parts")]
        public void Publish_AllBasicPagesTpWprdpress()
        {
            var pages = ProcessedPages.Pages.Where(p => !IsRulePage(p.ParentPageId)).ToList();

            CreateOrUpdatePagesByName(pages);
        }

        /// <summary>
        /// Published a target page to Wordpres.
        /// Change pageName to publish page you like. 
        /// </summary>
        [TestMethod]
        [TestCategory("Publishing.reSP.Parts")]
        public void Publish_PageByName()
        {
            // change page name to the one you need
            var pageName = "1_0_0_54";

            var pages = ProcessedPages.Pages.Where(p => p.Name.ToUpper() == pageName.ToUpper()).ToList();
            Assert.IsTrue(pages.Count == 1);

            CreateOrUpdatePagesByName(pages);
        }

        #endregion

        #region utils

        private bool IsRulePage(string parentPageId)
        {
            var ruleParents = new List<string>();

            // this is all parent page IDs for rule based pages

            // code
            ruleParents.Add("18511");
            // xml
            ruleParents.Add("18491");

            // aps.net
            ruleParents.Add("40691");

            // js
            ruleParents.Add("18521");

            return ruleParents.Contains(parentPageId);
        }

        #endregion
    }
}
