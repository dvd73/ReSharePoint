using System;
using System.Text;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(DeployFieldsCorrectlyHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DeployFieldsCorrectlyHighlighting.CheckId + ": Deploy lookup fields correctly.",
  "Set of principal checks for lookup field provision.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeployFieldsCorrectly : SPXmlTagProblemAnalyzer
    {
        [Flags]
        public enum ValidationResult : uint
        {
            Valid = 0,
            Version = 1,
            ShowField = 2,
            WebId = 4,
            ListWebRelativeListUrl = 8,
            ListGuid = 16
        }

        ValidationResult _validationResult = ValidationResult.Valid;

        protected override bool IsInvalid(IXmlTag element)
        {
            _validationResult = ValidationResult.Valid;

            if (element.IsFieldDefinition())
            {
                if (element.AttributeExists("Version"))
                    _validationResult |= ValidationResult.Version;

                // check lookup field declaration
                if (element.CheckAttributeValue("Type", new[] { "lookup", "lookupmulti" }))
                {
                    if (!element.CheckAttributeValue("ShowField", new[] {"title"}, true))
                        _validationResult |= ValidationResult.ShowField;

                    if (element.AttributeExists("WebId") && !element.CheckAttributeValue("WebId", new[] {"~sitecollection"}))
                        _validationResult |= ValidationResult.WebId;

                    if (!element.AttributeExists("List"))
                    {
                        _validationResult |= ValidationResult.ListWebRelativeListUrl;
                    }
                    else
                    {
                        if (element.AttributeValueIsGuid("List"))
                        {
                            _validationResult |= ValidationResult.ListGuid;
                        }
                    }
                }
            }

            return (uint)_validationResult > 0;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DeployFieldsCorrectlyHighlighting(element, _validationResult);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class DeployFieldsCorrectlyHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DeployFieldsCorrectly;

        public DeployFieldsCorrectly.ValidationResult ValidationResult;

        public DeployFieldsCorrectlyHighlighting(IXmlTag element,
            DeployFieldsCorrectly.ValidationResult validationResult) :
            base(element, $"{CheckId}: {GetMessage(validationResult)}")
        {
            ValidationResult = validationResult;
        }

        private static string GetMessage(DeployFieldsCorrectly.ValidationResult validationResult)
        {
            StringBuilder sb = new StringBuilder();

            if ((validationResult & DeployFieldsCorrectly.ValidationResult.Version) ==
                DeployFieldsCorrectly.ValidationResult.Version)
            {
                sb.Append("Remove Version attribute from field. ");
            }

            if ((validationResult & DeployFieldsCorrectly.ValidationResult.ShowField) ==
                DeployFieldsCorrectly.ValidationResult.ShowField)
            {
                sb.Append("Add ShowField=\"Title\" attribute. ");
            }

            if ((validationResult & DeployFieldsCorrectly.ValidationResult.WebId) ==
                DeployFieldsCorrectly.ValidationResult.WebId)
            {
                sb.Append("Set WebId=\"~sitecollection\" or remove WebId attribute from field. ");
            }

            if ((validationResult & DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl) ==
                DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl)
            {
                sb.Append("Add List=\"{WebRelativeListUrl}\" attribute. ");
            }

            if ((validationResult & DeployFieldsCorrectly.ValidationResult.ListGuid) ==
                DeployFieldsCorrectly.ValidationResult.ListGuid)
            {
                sb.Append("Change List attribute from GUID to ListUrl. ");
            }

            return sb.ToString().Trim();
        }

    }

    [QuickFix]
    public class DeployFieldsCorrectlyFix : SPXmlQuickFix<DeployFieldsCorrectlyHighlighting, IXmlTag> 
    {
        public DeployFieldsCorrectlyFix([NotNull] DeployFieldsCorrectlyHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.Version) ==
                DeployFieldsCorrectly.ValidationResult.Version)
                {
                    sb.AppendLine("Remove Version attribute. ");
                }

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.ShowField) ==
                    DeployFieldsCorrectly.ValidationResult.ShowField)
                {
                    sb.AppendLine("Ensure ShowField=\"Title\" attribute. ");
                }

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl) ==
                    DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl)
                {
                    sb.AppendLine("Add List=\"{WebRelativeListUrl}\" attribute. ");
                }

                return CommonHelper.NormalizeMessage(sb);
            }
        }

        public override string ScopedText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.Version) ==
                DeployFieldsCorrectly.ValidationResult.Version)
                {
                    sb.AppendLine("Remove all Version attributes. ");
                }

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.ShowField) ==
                    DeployFieldsCorrectly.ValidationResult.ShowField)
                {
                    sb.AppendLine("Ensure all ShowField=\"Title\" attributes. ");
                }

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl) ==
                    DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl)
                {
                    sb.AppendLine("Add all List=\"{WebRelativeListUrl}\" attributes. ");
                }

                return CommonHelper.NormalizeMessage(sb);
            }
        }

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.Version) ==
                    DeployFieldsCorrectly.ValidationResult.Version)
                {
                    element.RemoveAttribute(element.GetAttribute("Version"));
                }

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.ShowField) ==
                    DeployFieldsCorrectly.ValidationResult.ShowField)
                {
                    element.EnsureAttribute("ShowField", "Title");
                }

                if ((_highlighting.ValidationResult & DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl) ==
                    DeployFieldsCorrectly.ValidationResult.ListWebRelativeListUrl)
                {
                    element.EnsureAttribute("List", "{WebRelativeListUrl}");
                }
            }
        }
    }
}
