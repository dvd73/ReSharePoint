﻿using System.Collections.Generic;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    public class WebTempFileTagProblemAnalysis : SPXmlFileTagProblemAnalysisBase
    {
        public WebTempFileTagProblemAnalysis(IEnumerable<ISPXmlTagProblemAnalyzer> analyzers) :
            base("Templates", "Microsoft SharePoint", analyzers)
        {
        }

        protected override bool SPSchemaIsValid(IXmlTag validatedTag)
        {
            return validatedTag.CheckAttributeValue("xmlns:ows", new[] {"Microsoft SharePoint"}) ||
                validatedTag.CheckAttributeValue("xmlns", new[] {"http://schemas.microsoft.com/sharepoint"}) ||
                (validatedTag.GetSourceFile() != null &&
                    validatedTag.GetSourceFile().Name.ToLower().StartsWith("webtemp.xml")); 
        }
    }
}
