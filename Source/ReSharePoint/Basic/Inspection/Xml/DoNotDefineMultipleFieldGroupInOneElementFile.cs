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

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(DoNotDefineMultipleFieldGroupInOneElementFileHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  DoNotDefineMultipleFieldGroupInOneElementFileHighlighting.CheckId + ": " + DoNotDefineMultipleFieldGroupInOneElementFileHighlighting.Message,
  "Do not define multiple field groups in one element file.",
  Severity.SUGGESTION
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDefineMultipleFieldGroupInOneElementFile : SPXmlAttributeProblemAnalyzer
    {
        private bool _moreThenOneNames;

        public override void Init(IXmlFile file)
        {
            base.Init(file);

            var tags = file.GetNestedTags<IXmlTag>("Elements/Field").Where(t => t.AttributeExists("Group"));
            var groupNames = tags.Select(t => t.GetAttribute("Group").UnquotedValue);
            _moreThenOneNames = groupNames.Distinct().Count() > 1;
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = element.IsFieldDefinition() && element.AttributeExists("Group") && _moreThenOneNames;

            if (result)
            {
                ProblemAttribute = element.GetAttribute("Group");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DoNotDefineMultipleFieldGroupInOneElementFileHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotDefineMultipleFieldGroupInOneElementFileHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DoNotDefineMultipleFieldGroupInOneElementFile;
        public const string Message = "Do not define multiple field groups in one element file";

        public DoNotDefineMultipleFieldGroupInOneElementFileHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
