using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubPoint2.DocsImport.Resources;
using WordPressSharp;
using WordPressSharp.Models;
using System.Collections.Generic;
using System.IO;
using CsQuery;
using Ganss.XSS;
using MarkdownSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SubPoint2.DocsImport
{
    [TestClass]
    public class MarkDownTests
    {
        #region constrctors
        public MarkDownTests()
        {

        }

        #endregion

        #region tests


        [TestMethod]
        public void Can_ProcessMarkdownPage()
        {
            var resultFile = string.Format("result-{0}.html", Environment.TickCount);

            var m = new Markdown();

            var result = MarkdownPages.AboutSPMeta2;


            SyntaxTree tree =
                CSharpSyntaxTree.ParseText(
                    File.ReadAllText(
                        @"C:\Users\sp-dev\Source\Repos\spmeta2\SPMeta2\SPMeta2.Regression.Tests\Impl\Scenarios\ListItemValueScenariousTest.cs"));


            var root = (CompilationUnitSyntax)tree.GetRoot();

            var methods = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();

            var targetMethod = methods.FirstOrDefault(mt => mt.Identifier.Text == "CanDeploy_ListItemValue_ById");
            var content = targetMethod.ToString();

            var realContent = "[sourcecode language='csharp']" + content + "[/sourcecode]";

            result = result.Replace("@@CanDeploy_ListItemValue_ById@@", realContent);

            result = m.Transform(result);

            var sanitizer = new HtmlSanitizer();
            var sanitizedResult = sanitizer.Sanitize(result);

            File.WriteAllText(resultFile, sanitizedResult);


            // CanDeploy_TasksList

            System.Diagnostics.Process.Start(resultFile);
        }

        #endregion
    }
}
