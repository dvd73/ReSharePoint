using System;
using System.Linq;
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

[assembly: RegisterConfigurableSeverity(DeployTaxonomyFieldsCorrectlyHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DeployTaxonomyFieldsCorrectlyHighlighting.CheckId + ": Deploy taxonomy fields correctly.",
  "Set of principal checks for taxonomy field provision.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeployTaxonomyFieldsCorrectly : SPXmlTagProblemAnalyzer
    {
        [Flags]
        public enum ValidationResult : uint
        {
            Valid       = 0,
            ShowField   = 1,
            Mult        = 2,
            TextField   = 4
        }

        private IXmlTag _invalidProperty;
        private ValidationResult _validationResult = ValidationResult.Valid;
        public override void Init(IXmlFile file)
        {
            _invalidProperty = null;
            base.Init(file);
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            _validationResult = ValidationResult.Valid;

            if (element.IsFieldDefinition())
            {
                if (element.CheckAttributeValue("Type", new[] {"TaxonomyFieldType"}))
                {
                    if (element.AttributeExists("ShowField") && !element.CheckAttributeValue("ShowField", new[] {"Term$Resources:core,Language;"}, true))
                        _validationResult |= ValidationResult.ShowField;

                    if (element.CheckAttributeValue("Type", new[] {"TaxonomyFieldTypeMulti"}, true) &&
                        (!element.AttributeExists("Mult") || element.CheckAttributeValue("Mult", new[] {"false"}, true)))
                        _validationResult |= ValidationResult.Mult;
                    
                    IXmlTag tagCustomization = element.InnerTags.SingleOrDefault(_ => _.Header.Name.XmlName == "Customization");

                    IXmlTag tagArrayOfProperty =
                        tagCustomization?.InnerTags.SingleOrDefault(_ => _.Header.Name.XmlName == "ArrayOfProperty");

                    IXmlTag invalidProperty = tagArrayOfProperty?.InnerTags
                        .SingleOrDefault(
                            _ =>
                                _.Header.Name.XmlName == "Property" &&
                                _.InnerTags.Any(
                                    __ => __.Header.Name.XmlName == "Name" && __.InnerValue == "TextField"));
                                
                    if (invalidProperty != null)
                    {
                        _invalidProperty = invalidProperty;
                    }
                }
            }
            else if (element.Header.ContainerName == "Property")
            {
                
                if (element.Equals(_invalidProperty))
                    _validationResult |= ValidationResult.TextField;
            }

            return (uint)_validationResult > 0;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DeployTaxonomyFieldsCorrectlyHighlighting(element, _validationResult);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DeployTaxonomyFieldsCorrectlyHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DeployTaxonomyFieldsCorrectly;
        
        public DeployTaxonomyFieldsCorrectly.ValidationResult ValidationResult { get; }

        private static string GetMessage(DeployTaxonomyFieldsCorrectly.ValidationResult validationResult)
        {
            StringBuilder sb = new StringBuilder();

            if ((validationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField) ==
                DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField)
            {
                sb.Append("Remove ShowField attribute. ");
            }

            if ((validationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.Mult) ==
                DeployTaxonomyFieldsCorrectly.ValidationResult.Mult)
            {
                sb.Append("Add Mult=\"TRUE\" attribute. ");
            }

            if ((validationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.TextField) ==
                DeployTaxonomyFieldsCorrectly.ValidationResult.TextField)
            {
                sb.Append("Do not specify TextField property. ");
            }

            return sb.ToString().Trim();
        }

        public DeployTaxonomyFieldsCorrectlyHighlighting(IXmlTag element, DeployTaxonomyFieldsCorrectly.ValidationResult validationResult) :
            base(element, $"{CheckId}: {GetMessage(validationResult)}")
        {
            ValidationResult = validationResult;
        }
    }

    [QuickFix]
    public class DeployTaxonomyFieldsCorrectlyFix : SPXmlQuickFix<DeployTaxonomyFieldsCorrectlyHighlighting, IXmlTag> 
    {
        public DeployTaxonomyFieldsCorrectlyFix([NotNull] DeployTaxonomyFieldsCorrectlyHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField) ==
                    DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField)
                {
                    sb.AppendLine("Remove ShowField attribute. ");
                }

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.Mult) ==
                    DeployTaxonomyFieldsCorrectly.ValidationResult.Mult)
                {
                    sb.AppendLine("Add Mult=\"TRUE\" attribute. ");
                }

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.TextField) ==
                DeployTaxonomyFieldsCorrectly.ValidationResult.TextField)
                {
                    sb.AppendLine("Remove TextField property. ");
                }

                return CommonHelper.NormalizeMessage(sb); 
            }
        }

        public override string ScopedText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField) ==
                    DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField)
                {
                    sb.AppendLine("Remove all ShowField attributes. ");
                }

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.Mult) ==
                    DeployTaxonomyFieldsCorrectly.ValidationResult.Mult)
                {
                    sb.AppendLine("Add all Mult=\"TRUE\" attributes. ");
                }

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.TextField) ==
                DeployTaxonomyFieldsCorrectly.ValidationResult.TextField)
                {
                    sb.AppendLine("Remove all TextField properties. ");
                }

                return CommonHelper.NormalizeMessage(sb);
            }
        }

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField) ==
                    DeployTaxonomyFieldsCorrectly.ValidationResult.ShowField)
                {
                    element.RemoveAttribute(element.GetAttribute("ShowField"));
                }

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.Mult) ==
                    DeployTaxonomyFieldsCorrectly.ValidationResult.Mult)
                {
                    element.EnsureAttribute("Mult", "TRUE");
                }

                if ((_highlighting.ValidationResult & DeployTaxonomyFieldsCorrectly.ValidationResult.TextField) ==
                    DeployTaxonomyFieldsCorrectly.ValidationResult.TextField)
                {
                    element.Remove();
                }
            }
        }
    }
}
