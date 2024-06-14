using System;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(DoNotDefineMultipleContentTypeGroupInOneElementFileHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  DoNotDefineMultipleContentTypeGroupInOneElementFileHighlighting.CheckId + ": " + DoNotDefineMultipleContentTypeGroupInOneElementFileHighlighting.Message,
  "Do not define multiple content type groups in one element file.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDefineMultipleContentTypeGroupInOneElementFile : SPXmlAttributeProblemAnalyzer
    {
        private bool _moreThenOneNames;
        
        public override void Init(IXmlFile file)
        {
            base.Init(file);

            var tags = file.GetNestedTags<IXmlTag>("Elements/ContentType").Where(t => t.AttributeExists("Group"));
            var groupNames = tags.Select(t => t.GetAttribute("Group").UnquotedValue);
            _moreThenOneNames = groupNames.Distinct().Count() > 1;
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = element.Header.ContainerName == "ContentType" && element.AttributeExists("Group") && _moreThenOneNames;
            
            if (result)
            {
                ProblemAttribute = element.GetAttribute("Group");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DoNotDefineMultipleContentTypeGroupInOneElementFileHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotDefineMultipleContentTypeGroupInOneElementFileHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.DoNotDefineMultipleContentTypeGroupInOneElementFile;
        public const string Message = "Do not define multiple content type groups in one element file";

        public DoNotDefineMultipleContentTypeGroupInOneElementFileHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
