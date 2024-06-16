using System;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Parsing;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Util;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(WebPartDefinitionMightBeImprovedHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  WebPartDefinitionMightBeImprovedHighlighting.CheckId + ": Improve web part description.",
  "Set of checks for web part file.",
  Severity.SUGGESTION
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class WebPartDefinitionMightBeImproved : SPXmlTagProblemAnalyzer
    {
        private bool _catalogIconImageUrlPresent;
        private bool _titleIconImageUrlPresent;
        private bool _descriptionPresent;
        private bool _chromeTypePresent;
        private readonly string[] RestrictedDescriptions = { "My Web Part", "My Visual Web Part" };

        [Flags]
        public enum ValidationResult : uint
        {
            Valid = 0,
            MissingCatalogIconImageUrl = 1,
            MissingTitleIconImageUrl = 2,
            MissingChromeType = 4,
            MissingDescription = 8,
            RestrictedDescription = 16
        }

        private ValidationResult _validationResult = ValidationResult.Valid;
        
        public override void Init(IXmlFile file)
        {
            base.Init(file);

            _catalogIconImageUrlPresent =
                file.GetNestedTags<IXmlTag>("webParts/webPart/data/properties/property")
                    .Any(t => t.CheckAttributeValue("name",new[] {"CatalogIconImageUrl"}, true));
            _titleIconImageUrlPresent =
                file.GetNestedTags<IXmlTag>("webParts/webPart/data/properties/property")
                    .Any(t => t.CheckAttributeValue("name", new[] {"TitleIconImageUrl"}, true));
            _descriptionPresent =
                file.GetNestedTags<IXmlTag>("webParts/webPart/data/properties/property")
                    .Any(t => t.CheckAttributeValue("name", new[] {"Description"}, true));
            _chromeTypePresent =
                file.GetNestedTags<IXmlTag>("webParts/webPart/data/properties/property")
                    .Any(t => t.CheckAttributeValue("name", new[] {"ChromeType"}, true));
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            _validationResult = ValidationResult.Valid;

            switch (element.Header.ContainerName)
            {
                case "properties":
                    if (!_catalogIconImageUrlPresent)
                        _validationResult |= ValidationResult.MissingCatalogIconImageUrl;
                    if (!_titleIconImageUrlPresent)
                        _validationResult |= ValidationResult.MissingTitleIconImageUrl;
                    if (!_descriptionPresent)
                        _validationResult |= ValidationResult.MissingDescription;
                    if (!_chromeTypePresent)
                        _validationResult |= ValidationResult.MissingChromeType;
                    break;
                case "property":
                    foreach (var restrictedDescription in RestrictedDescriptions)
                    {
                        if (element.CheckAttributeValue("name", new[] {"Description"}, true) && element.InnerText.Trim() == restrictedDescription)
                        {
                            _validationResult |= ValidationResult.RestrictedDescription;
                        }
                    }

                    break;
            }

            return (uint)_validationResult > 0;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new WebPartDefinitionMightBeImprovedHighlighting(element, _validationResult);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class WebPartDefinitionMightBeImprovedHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.WebPart.WebPartDefinitionMightBeImproved;
        public WebPartDefinitionMightBeImproved.ValidationResult ValidationResult;

        private static string GetMessage(WebPartDefinitionMightBeImproved.ValidationResult validationResult)
        {
            StringBuilder sb = new StringBuilder();

            if ((validationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl) ==
                WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl)
            {
                sb.AppendLine("Web part might have not null or empty CatalogIconImageUrl property.");
            }

            if ((validationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl) ==
                WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl)
            {
                sb.AppendLine("Web part might have not null or empty TitleIconImageUrl property.");
            }

            if ((validationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType) ==
                WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType)
            {
                sb.AppendLine("Web part might have not null or empty ChromeType property.");
            }

            if ((validationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription) ==
                WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription)
            {
                sb.AppendLine("Web part should have description property.");
            }

            if ((validationResult & WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription) ==
                WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription)
            {
                sb.AppendLine("Web part should not have autogenerated description property.");
            }

            return sb.ToString().Trim();
        }

        public WebPartDefinitionMightBeImprovedHighlighting(IXmlTag element,
            WebPartDefinitionMightBeImproved.ValidationResult validationResult) :
            base(element, $"{CheckId}: {GetMessage(validationResult)}")
        {
            ValidationResult = validationResult;
        }
    }

    [QuickFix]
    public class WebPartDefinitionMightBeImprovedFix : SPXmlQuickFix<WebPartDefinitionMightBeImprovedHighlighting, IXmlTag> 
    {
        public WebPartDefinitionMightBeImprovedFix([NotNull] WebPartDefinitionMightBeImprovedHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override bool IsAvailable(IUserDataHolder cache)
        {
            if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription)
            {
                return false;
            }
            
            return base.IsAvailable(cache);
        }

        public override string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl) ==
                WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl)
                {
                    sb.Append("Add CatalogIconImageUrl property ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl)
                {
                    sb.Append("Add TitleIconImageUrl property ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType)
                {
                    sb.Append("Add ChromeType property ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription)
                {
                    sb.Append("Add Description property ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription)
                {
                    sb.Append("No action ");
                }
                return CommonHelper.NormalizeMessage(sb);
            }
        }

        public override string ScopedText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl) ==
                WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl)
                {
                    sb.Append("Add all CatalogIconImageUrl properties ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl)
                {
                    sb.Append("Add all TitleIconImageUrl properties ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType)
                {
                    sb.Append("Add all ChromeType properties ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription)
                {
                    sb.Append("Add all Description properties ");
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.RestrictedDescription)
                {
                    sb.Append("No action ");
                }
                return CommonHelper.NormalizeMessage(sb);
            }
        }

        protected override void Fix(IXmlTag element)
        {
            XmlElementFactory elementFactory = XmlElementFactory.GetInstance(element);
            IXmlTag anchor = element.GetNestedTags<IXmlTag>("property").LastOrDefault();

            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingCatalogIconImageUrl)
                {
                    IXmlTag tag = elementFactory.CreateTagForTag(element,
                        "<property name=\"CatalogIconImageUrl\" type=\"string\">/_layouts/images/mscntvwl.gif</property>");
                    element.AddTagAfter(tag, anchor);
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingTitleIconImageUrl)
                {
                    IXmlTag tag = elementFactory.CreateTagForTag(element,
                        "<property name=\"TitleIconImageUrl\" type=\"string\">/_layouts/images/mscntvwl.gif</property>");
                    element.AddTagAfter(tag, anchor);
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingChromeType)
                {
                    IXmlTag tag = elementFactory.CreateTagForTag(element,
                        "<property name=\"ChromeType\" type=\"chrometype\">None</property>");
                    element.AddTagAfter(tag, anchor);
                }

                if ((_highlighting.ValidationResult & WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription) ==
                    WebPartDefinitionMightBeImproved.ValidationResult.MissingDescription)
                {
                    IXmlTag tag = elementFactory.CreateTagForTag(element,
                        "<property name=\"Description\" type=\"string\"></property>");
                    element.AddTagAfter(tag, anchor);
                }
            }
        }
    }
}
