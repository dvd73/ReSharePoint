using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordPressSharp;
using WordPressSharp.Models;
using SubPoint2.DocsImport.Data;

namespace SubPoint2.DocsImport.Services
{
    public class WordPressService
    {
        public WordPressService()
        {

        }


        #region properties

        public string Url { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        #endregion

        public void WithWordPressClient(Action<WordPressClient> action)
        {
            using (var client = new WordPressClient(new WordPressSiteConfig
            {
                BaseUrl = Url,
                Username = UserName,
                Password = UserPassword,
                BlogId = 1
            }))
            {
                action(client);
            }
        }

        private List<Post> CachedPosts = null;

        protected Post GetPostByName(WordPressClient client, WordpressPageDefinition pageDefinition)
        {
            if (CachedPosts == null)
            {
                CachedPosts = new List<Post>();

                var drafts = client.GetPosts(new PostFilter
                {
                    PostStatus = "draft",
                    PostType = "page",
                    Number = 1000
                });

                CachedPosts.AddRange(drafts);

                var published = client.GetPosts(new PostFilter
                {
                    PostStatus = "published",
                    PostType = "page",
                    Number = 1000
                });

                CachedPosts.AddRange(published);
            }

            var pageName = pageDefinition.Name.ToUpper();
            var pageParentIdName = pageDefinition.ParentPageId;

            // by real name
            var result = CachedPosts.FirstOrDefault(p =>
                !string.IsNullOrEmpty(p.Name)
                && (p.Name.ToUpper() == pageName)
                && (string.IsNullOrEmpty(pageParentIdName) || (p.ParentId == pageParentIdName)));

            //// if null, then old reSP?
            //if (result == null)
            //{
            //    int value;
            //    if (int.TryParse(pageDefinition.Name, out value))
            //    {
            //        pageName = "resp51" + pageDefinition.Name;
            //        pageName = pageName.ToUpper();

            //        // ok, resp page
            //        result = CachedPosts.FirstOrDefault(p =>
            //            !string.IsNullOrEmpty(p.Name)
            //            && (p.Name.ToUpper() == pageName)
            //            && (string.IsNullOrEmpty(pageParentIdName) || (p.ParentId == pageParentIdName)));

            //    }
            //}


            return result;
        }

        public Post GetOrCreatePageByName(WordPressClient client, WordpressPageDefinition pageDefinition)
        {
            var currentPage = GetPostByName(client, pageDefinition);

            if (currentPage == null)
            {
                var post = new Post
                {
                    PostType = "page",
                    Title = pageDefinition.Title,
                    Name = pageDefinition.Name,
                    Content = pageDefinition.Content,
                    //PublishDateTime = DateTime.Now,
                    Status = "draft",
                    ParentId = pageDefinition.ParentPageId,
                };

                var id = client.NewPost(post);

                var newPost = client.GetPost(int.Parse(id));

                CachedPosts.Add(newPost);

                return GetPostByName(client, pageDefinition);
            }

            return currentPage;
        }


        public void DeletePagesByParentId(string parentPageId)
        {
            WithWordPressClient(client =>
            {
                GetPostByName(client, new WordpressPageDefinition
                {
                    Name = Environment.TickCount.ToString(),
                    ParentPageId = "0"
                });

                var wpPost = CachedPosts.Where(p => p.ParentId == parentPageId);

                foreach (var post in wpPost)
                {
                    client.DeletePost(int.Parse(post.Id));
                }
            });
        }
    }
}
