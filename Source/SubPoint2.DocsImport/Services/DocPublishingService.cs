using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CsQuery;
using Ganss.XSS;
using MarkdownSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SubPoint2.DocsImport.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubPoint2.DocsImport.Resources;

namespace SubPoint2.DocsImport.Services
{
    public class DocPublishingService
    {

        public DocPublishingService()
        {
            TimeStamp = DateTime.Now.ToString("D", new CultureInfo(1033));
        }

        #region methods

        public DocPublishingServiceResult ProcessProjectDocumentation(DocPublishingServiceOptions options)
        {
            var result = new DocPublishingServiceResult();

            var srcFolder = options.CSharpProjectFolder;

            var outputFolder = string.IsNullOrEmpty(options.OutputFolder) ? string.Empty : options.OutputFolder;
            var destTmpFolderPath = Path.Combine(outputFolder, DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            Directory.CreateDirectory(destTmpFolderPath);
            var mdFiles = Directory.GetFiles(srcFolder, "*.md", SearchOption.AllDirectories);

            result.PagesFolderPath = destTmpFolderPath;

            foreach (var mdFilePath in mdFiles)
            {
                var mdFileName = Path.GetFileNameWithoutExtension(mdFilePath);
                var dstFileName = string.Format("{0}.html", mdFileName);
                var dstFilePath = Path.Combine(destTmpFolderPath, dstFileName);

                var resultPage = ProcessMarkdownToHtml(mdFilePath);

                result.Pages.Add(resultPage);

                File.WriteAllText(dstFilePath, resultPage.Content);
            }

            return result;
        }

        private WordpressPageDefinition ProcessMarkdownToHtml(string mdFilePath)
        {
            var fileFolderPath = Path.GetDirectoryName(mdFilePath);
            var fileName = Path.GetFileNameWithoutExtension(mdFilePath);

            var csFile = Path.Combine(fileFolderPath, fileName + ".cs");
            var xmlFile = Path.Combine(fileFolderPath, fileName + ".xml");

            var postName = fileName.Replace("Tests", string.Empty);

            var resultPage = new WordpressPageDefinition
            {
                Name = postName,
                Title = postName
            };

            var postContent = File.ReadAllText(mdFilePath);

            resultPage.IsTodoPage = postContent.Contains("M2-TODO") ||
                                    postContent.Contains("RESP-TODO");

            postContent = ProcessMarkdown(postContent);
            postContent = ProcessPageAttributes(resultPage, postContent);

            postContent = ProcessSanitize(postContent);

            postContent = ProcessJSMethods(postContent, mdFilePath);

            postContent = ProcessCSMethods(postContent, csFile);
            postContent = ProcessXmlMethods(postContent, xmlFile);

            postContent = ProcessReleasesToken(postContent, mdFilePath);


            postContent = ProcessHelpImproiveArea(postContent, mdFilePath);

            postContent = ProcessRefreshStamp(postContent, xmlFile);

            postContent = postContent.Trim();
            resultPage.Content = postContent;

            return resultPage;
        }

        private string ProcessHelpImproiveArea(string postContent, string mdFile)
        {
            var result = new StringBuilder();

            //result.Append(HtmlTmplates.DocImproveAres);
            //result.Append(Environment.NewLine);

            var iprovementTemplate = HtmlTmplates.DocImproveAres;

            if (mdFile.Contains("SPMeta2"))
            {
                var onlinePartPath = mdFile.Replace("SPMeta2.Docs", "$$$$").Split(new string[] { "$$$$" }, StringSplitOptions.None)[1];
                var onlineFullPath = "https://github.com/SubPointSolutions/spmeta2-docs/blob/dev/SPMeta2.Docs/" + onlinePartPath;

                iprovementTemplate = iprovementTemplate.Replace("$MORE_LINKS$",
                    string.Format("<a href='{0}' class='' style='background: #5bc0de;padding: 10px; color: #fff !important;;margin-left: 10px;'>Edit @ githiub</a>", onlineFullPath));
            }
            else
            {
                iprovementTemplate = iprovementTemplate.Replace("$MORE_LINKS$", string.Empty);
            }


            result.Append(postContent);

            result.Append(Environment.NewLine);
            result.Append(iprovementTemplate);

            return result.ToString();
        }

        private string ProcessReleasesToken(string postContent, string mdFilePath)
        {
            if (!postContent.Contains("[RELEASES.TIMELINE]"))
                return postContent;

            var fileFolderPath = Path.GetDirectoryName(mdFilePath);
            var fileName = Path.GetFileNameWithoutExtension(mdFilePath);

            var releasesFolder = Path.Combine(fileFolderPath, "Releases");

            if (Directory.Exists(releasesFolder))
            {
                var allReleaseFiles = Directory.GetFiles(releasesFolder, "*.md");
                var realReleaseFiles = new List<ReleaseVersionFile>();

                foreach (var file in allReleaseFiles)
                {
                    var relFileName = Path.GetFileNameWithoutExtension(file);
                    var versionName = relFileName;

                    if (versionName.Contains("-"))
                        versionName = versionName.Split('-')[0];

                    var version = new Version(1, 0);
                    if (Version.TryParse(versionName, out version))
                    {
                        realReleaseFiles.Add(new ReleaseVersionFile
                        {
                            FilePath = file,
                            Version = version
                        });
                    }

                }

                if (realReleaseFiles.Count > 0)
                {
                    var sotedReleases = realReleaseFiles.OrderByDescending(r => r.Version);

                    var releaseContentBlock = new StringBuilder();

                    releaseContentBlock.Append("<ul class='timeline'>");

                    foreach (var releaseFile in sotedReleases)
                    {
                        var page = ProcessMarkdownToHtml(releaseFile.FilePath);

                        CQ csQuery = page.Content;

                        var paragraphs = csQuery.Select("p");
                        var allPs = new List<IDomObject>();

                        foreach (var p in paragraphs)
                        {
                            if (p.NextElementSibling.NodeName.ToUpper() != "p".ToUpper())
                            {
                                allPs.Add(p);
                                break;
                            }

                            allPs.Add(p);
                        }

                        var allPsContent = allPs.Select(p => string.Format("<p>{0}</p>", p.Render()));
                        var description = string.Join(Environment.NewLine, allPsContent);

                        //<ul class="timeline">

                        var dateString = string.Join("<br/>", page.Title.Split(',')[1].Trim().Split(' '));

                        releaseContentBlock.Append(string.Format("{0}{1}", CreatMainReleaseSection(new ReleaseItem
                        {
                            Version = releaseFile.Version,
                            Title = string.Format("SPMeta2 {0}", releaseFile.Version.ToString()),
                            Content = description,
                            Date = dateString,
                        }), Environment.NewLine));

                        //releaseContentBlock.Append(string.Format("SPMeta2 {0}", releaseFile.Version.ToString()));
                        // releaseContentBlock.Append("<br/>");
                        //releaseContentBlock.Append(description);
                        //releaseContentBlock.Append("<hr/>");
                    }

                    releaseContentBlock.Append("</ul>");

                    postContent = postContent.Replace("[RELEASES.TIMELINE]", releaseContentBlock.ToString());
                }
            }

            return postContent;
        }

        public class ReleaseItem
        {
            public ReleaseItem()
            {
                Title = string.Empty;
                Content = string.Empty;
                Buttons = string.Empty;
            }

            public string Title { get; set; }
            public string Content { get; set; }

            public string Buttons { get; set; }

            public Version Version { get; set; }

            public string Date { get; set; }
        }

        private string CreatMainReleaseSection(ReleaseItem item)
        {
            var rsult = HtmlTmplates.MainReleaseTimelineItem;

            var itemClass = "";

            if (item.Version.Build % 10 > 0)
            {
                itemClass = "timeline-inverted";
            }

            rsult = rsult.Replace("[ITEM-CLASS]", itemClass);
            rsult = rsult.Replace("[TITLE]", item.Title);
            rsult = rsult.Replace("[CONTENT]", item.Content);
            rsult = rsult.Replace("[BUTTONS]", item.Buttons);
            rsult = rsult.Replace("[DATE]", item.Date);



            return rsult;
        }

        protected class ReleaseVersionFile
        {
            public Version Version { get; set; }
            public string FilePath { get; set; }
        }

        private string TimeStamp = "";

        private string ProcessRefreshStamp(string postContent, string pageHtmlContent)
        {
            postContent += string.Format("<i>Published on {0}</i>", TimeStamp);

            return postContent;

            //CQ csQuery = pageHtmlContent;

            //var properties = csQuery.Select("input[type=hidden]").FirstOrDefault();

            //if (properties != null)
            //{
            //    page.Title = GetAttribute(page, properties, "pageTitle", page.Name);
            //    page.ParentPageId = GetAttribute(page, properties, "parentPageId", page.Name);
            //    page.Name = GetAttribute(page, properties, "pageName", page.Name);
            //}
            //else
            //{
            //    Assert.Fail(string.Format("Missing <properties /> tag for page: [{0}]", page.Name));
            //}

            //properties.Remove();

            //csQuery.Select("p:empty").Remove();

            //return csQuery.Render();
        }

        private string ProcessXmlMethods(string postContent, string xmlFilePath)
        {
            var samples = LoaXmlSamples(xmlFilePath);

            foreach (var sample in samples)
            {
                var sampleToken = string.Format("[XML.{0}]", sample.Id);
                var wrappedContent = "[sourcecode language='xml']" + sample.Content + "[/sourcecode]";

                postContent = postContent.Replace(sampleToken, wrappedContent);
            }

            return postContent;
        }

        private List<XmlSample> LoaXmlSamples(string xmlFilePath)
        {
            var result = new List<XmlSample>();

            if (!File.Exists(xmlFilePath))
                return result;

            var lines = File.ReadAllLines(xmlFilePath);

            XmlSample sample = new XmlSample();
            var hasSample = false;
            var isFirstLine = false;

            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("<!--") && line.Contains("sample="))
                {
                    hasSample = true;
                    sample = new XmlSample();

                    sample.Id = line.Trim()
                        .Replace("<!--", string.Empty)
                        .Replace("-->", string.Empty)
                        .Trim()
                        .Split(new string[] { "=" }, StringSplitOptions.None)[1].Trim();

                    isFirstLine = true;
                    result.Add(sample);
                }
                else
                {
                    if (hasSample && line.Trim() != "</Samples>")
                    {
                        sample.Content += string.Format("{0}{1}", line, Environment.NewLine);
                    }
                }
            }

            foreach (var r in result)
                r.Content = NormilizeXmlBody(r.Content);

            return result;
        }

        protected class XmlSample
        {
            public string Id { get; set; }
            public string Content { get; set; }
        }

        private string ProcessPageAttributes(WordpressPageDefinition page, string pageHtmlContent)
        {
            CQ csQuery = pageHtmlContent;

            var properties = csQuery.Select("properties").FirstOrDefault();

            if (properties != null)
            {
                page.Title = GetAttribute(page, properties, "pageTitle", page.Name);
                page.ParentPageId = GetAttribute(page, properties, "parentPageId", page.Name);
                page.Name = GetAttribute(page, properties, "pageName", page.Name);
            }
            else
            {
                Assert.Fail(string.Format("Missing <properties /> tag for page: [{0}]", page.Name));
            }

            properties.Remove();

            csQuery.Select("p:empty").Remove();

            return csQuery.Render();
        }

        protected string GetAttribute(WordpressPageDefinition page, IDomObject obj, string attrName, string defaultValue)
        {
            if (obj.HasAttribute(attrName))
                return obj.GetAttribute(attrName);

            return string.Empty;

            //Assert.Fail(string.Format("Missing attr [{1}] fir page [{0}]", page.Name, attrName));

            //throw new ArgumentException(string.Format("Missing attr [{1}] fir page [{0}]", page.Name, attrName));
        }


        private Markdown mdEngine = new Markdown();
        private HtmlSanitizer sanitizeEngine = new HtmlSanitizer();

        private string ProcessSanitize(string postContent)
        {
            return sanitizeEngine.Sanitize(postContent);
        }

        private string ProcessMarkdown(string postContent)
        {
            return mdEngine.Transform(postContent);
        }

        private string ProcessJSMethods(string postContent, string mdFile)
        {
            var jsMethods = GetJSharpMethods(mdFile);

            foreach (var method in jsMethods)
            {
                var methodTag = string.Format("[JS.{0}]", method.Name);
                var normilizedMethodBody = NormilizeMethodBody(method.Body);

                var wrappedMethodContentContent = "[sourcecode language='js']" + normilizedMethodBody + "[/sourcecode]";

                var metaLinks = string.Empty;

                //metaLinks += "<div style='border:1px #dedede solid; padding:5px'>";

                //if (mdFile.Contains("SPMeta2"))
                //{
                //    var onlinePartPath = mdFile.Replace("SPMeta2.Docs.ProvisionSamples", "$$$$").Split(new string[] { "$$$$" }, StringSplitOptions.None)[1];
                //    var onlineFullPath = "https://github.com/SubPointSolutions/spmeta2-docs/tree/dev/SPMeta2.Docs/Web/" + onlinePartPath;

                //    metaLinks += string.Format("<a class='' href='{0}' target='_blank'>view this sample @ github</a> | ", onlineFullPath);
                //}

                //metaLinks += string.Format("<a href='{0}' target='_blank'>suggest improvements</a>", "https://github.com/SubPointSolutions/spmeta2-docs/issues");
                //metaLinks += string.Format(" | <a href='{0}' target='_blank'>discuss @ reSP Yammer Group</a>", "https://www.yammer.com/spmeta2feedback/#/threads/inGroup?type=in_group&feedId=4913150");

                //metaLinks += "</div>";

                wrappedMethodContentContent += metaLinks;

                postContent = postContent.Replace(methodTag, wrappedMethodContentContent);
            }

            return postContent;
        }

        private List<JSMethod> GetJSharpMethods(string mdFile)
        {
            var result = new List<JSMethod>();

            var fileFolderPath = Path.GetDirectoryName(mdFile);
            var fileName = Path.GetFileNameWithoutExtension(mdFile);

            var filePath = Path.Combine(fileFolderPath, fileName);

            var jsFiles = Directory.GetFiles(fileFolderPath, fileName + ".*.js");

            if (jsFiles.Count() > 0)
            {
                foreach (var jsFile in jsFiles)
                {
                    var jsFileName = Path.GetFileNameWithoutExtension(jsFile);

                    var methodName = jsFileName.Split('.')[1];
                    var methodContent = File.ReadAllText(jsFile);

                    var jsMethod = new JSMethod
                    {
                        Body = methodContent,
                        Name = methodName
                    };

                    result.Add(jsMethod);
                }
            }

            return result;
        }

        private string ProcessCSMethods(string postContent, string csFile)
        {
            var csMethods = GetCSharpMethods(csFile);

            foreach (var method in csMethods)
            {
                var methodTag = string.Format("[TEST.{0}]", method.Name);
                var normilizedMethodBody = NormilizeMethodBody(method.Body);

                var wrappedMethodContentContent = "[sourcecode language='csharp']" + normilizedMethodBody + "[/sourcecode]";

                var metaLinks = string.Empty;

                //metaLinks += "<div style='border:1px #dedede solid; padding:5px'>";

                //if (csFile.Contains("SPMeta2.Docs.ProvisionSamples"))
                //{
                //    var onlinePartPath = csFile.Replace("SPMeta2.Docs", "$$$$").Split(new string[] { "$$$$" }, StringSplitOptions.None)[1];
                //    var onlineFullPath = "https://github.com/SubPointSolutions/spmeta2-docs/tree/dev/SPMeta2.Docs/Web/" + onlinePartPath;

                //    metaLinks += string.Format("<a class='' href='{0}' target='_blank'>view this sample @ github</a> | ", onlineFullPath);
                //}

                //metaLinks += string.Format("<a href='{0}' target='_blank'>suggest improvements</a>", "https://github.com/SubPointSolutions/spmeta2-docs/issues");
                //metaLinks += string.Format(" | <a href='{0}' target='_blank'>discuss @ reSP Yammer Group</a>", "https://www.yammer.com/spmeta2feedback/#/threads/inGroup?type=in_group&feedId=4913150");

                //metaLinks += "</div>";

                wrappedMethodContentContent += metaLinks;

                postContent = postContent.Replace(methodTag, wrappedMethodContentContent);
            }

            return postContent;
        }

        protected string NormilizeBody(string body, string startString)
        {
            var lines = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            var firstLine = lines.FirstOrDefault(l => l.Contains(startString));
            if (firstLine != null)
            {

                var chardToRemove = firstLine.IndexOf(startString);

                for (int i = 0; i < lines.Length; i++)
                {
                    var currentLine = lines[i];
                    if (currentLine.StartsWith(" ") && currentLine.Length > chardToRemove)
                    {
                        currentLine = currentLine.Substring(chardToRemove, currentLine.Length - chardToRemove);
                        lines[i] = currentLine;
                    }
                }
            }

            var result = string.Join(Environment.NewLine, lines);

            return result;
        }

        protected string NormilizeMethodBody(string body)
        {
            return NormilizeBody(body, "public");
        }

        protected string NormilizeXmlBody(string body)
        {
            return NormilizeBody(body, "<");
        }

        private List<CSMethod> GetCSharpMethods(string csFile)
        {
            var result = new List<CSMethod>();

            if (File.Exists(csFile))
            {
                var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(csFile));

                var root = (CompilationUnitSyntax)tree.GetRoot();
                var methods = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

                foreach (var method in methods)
                {
                    result.Add(new CSMethod
                    {
                        Name = method.Identifier.Text,
                        Body = method.ToString()
                    });
                }
            }

            return result;
        }

        protected class AMethod
        {

            public string Name { get; set; }
            public string Body { get; set; }
        }

        protected class CSMethod : AMethod
        {
        }

        protected class JSMethod : AMethod
        {

        }

        #endregion
    }
}
