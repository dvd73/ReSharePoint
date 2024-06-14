using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubPoint2.DocsImport.Data;
using SubPoint2.DocsImport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordPressSharp;
using WordPressSharp.Models;

namespace SubPoint2.DocsImport.Base
{
    public abstract class BasePublishingTest
    {
        #region constructors

        public BasePublishingTest()
        {
            DocPublishingService = new DocPublishingService();

            WordpressService = new WordPressService();

            WordpressService.Url = "http://docs.subpointsolutions.com";
            WordpressService.UserName = "dvd73";
            WordpressService.UserPassword = "dbnzpm73";

            // setup correct VS paths here
            //SPMeta_VSProjectPath = @"C:\Users\sp-dev\Source\Repos\spmeta2-docs\SPMeta2.Docs";
            //ReSP_VSProjectPath = @"";
        }

        #endregion

        #region properties

        public DocPublishingService DocPublishingService { get; set; }
        public string VSProjectPath { get; set; }

        #endregion

        #region methods

        protected virtual DocPublishingServiceResult GetAllPages()
        {
            Assert.IsNotNull(VSProjectPath);

            return GetAllPages(new DocPublishingServiceOptions
            {
                CSharpProjectFolder = VSProjectPath
            });
        }

        protected virtual DocPublishingServiceResult GetAllPages(DocPublishingServiceOptions options)
        {
            return DocPublishingService.ProcessProjectDocumentation(options);
        }

        protected WordPressService WordpressService { get; set; }
        private DocPublishingServiceResult _pages;

        protected virtual DocPublishingServiceResult ProcessedPages
        {
            get
            {
                if (_pages == null)
                {
                    _pages = GetAllPages();
                }

                return _pages;
            }
        }

        #endregion

        protected void CreateOrUpdatePagesByName(IEnumerable<WordpressPageDefinition> pageDefinitions)
        {
            CreateOrUpdatePagesByName(pageDefinitions, "publish");
        }

        protected void CreateOrUpdatePagesByName(IEnumerable<WordpressPageDefinition> pageDefinitions, string status)
        {
            WordpressService.WithWordPressClient(client =>
            {

                var terms = new List<Term>();

                // resp
                terms.Add(client.GetTerm("category", 91));
                // spm
                terms.Add(client.GetTerm("category", 51));

                foreach (var pageDefinition in pageDefinitions)
                {
                    var page = WordpressService.GetOrCreatePageByName(client, pageDefinition);

                    page.Content = pageDefinition.Content;
                    page.Title = pageDefinition.Title;

                    if (page.Title.Contains(":"))
                    {
                        page.Title = page.Title.Split(':')[1];
                    }

                    if (!string.IsNullOrEmpty(pageDefinition.ParentPageId))
                        page.ParentId = pageDefinition.ParentPageId;

                    var isTodoPage = pageDefinition.IsTodoPage;

                    if (pageDefinition.Terms.Any())
                    {
                        foreach (var termId in pageDefinition.Terms)
                        {
                            var newTerm = terms.FirstOrDefault(t => t.Id.ToUpper() == termId.ToString().ToUpper());

                            if (!page.Terms.Any(t => t.Id == newTerm.Id))
                            {
                                var tmpArray = new List<Term>(page.Terms);
                                tmpArray.Add(newTerm);

                                page.Terms = tmpArray.ToArray();
                            }
                        }
                    }

                    if (isTodoPage)
                        page.Status = "draft";
                    else
                    {
                        if (!string.IsNullOrEmpty(status))
                        {
                            page.Status = status;
                        }
                        else
                        {
                            page.Status = "publish";
                        }
                    }

                    client.EditPost(page);
                }
            });
        }

        //protected void WithWordPressClient(Action<WordPressClient> action)
        //{
        //    using (var client = new WordPressClient(new WordPressSiteConfig
        //    {
        //        BaseUrl = Url,
        //        Username = UserName,
        //        Password = UserPassword,
        //        BlogId = 1
        //    }))
        //    {
        //        action(client);
        //    }
        //}


        protected void CreateOrUpdatePageByName(WordpressPageDefinition pageDefinition)
        {
            WordpressService.WithWordPressClient(client =>
            {
                var page = WordpressService.GetOrCreatePageByName(client, pageDefinition);

                page.Content = pageDefinition.Content;

                client.EditPost(page);
            });
        }
    }
}
