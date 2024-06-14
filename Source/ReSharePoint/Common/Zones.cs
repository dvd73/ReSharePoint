using JetBrains.Application.BuildScript.Application.Zones;
using ReSharePoint.Common.Assets;

namespace ReSharePoint.Common
{
    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("reSP Basic", "Validate your SharePoint C# and Xml code with 200+ rules.", true)]
    public interface IBasicProductZone : IZone
    {
    }

    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("reSP Pro", "SharePoint productivity extension.", true)]
    [ZoneDefinitionProduct(CompanyName = "SubPoint Solutions", CompanyNameLegal = "SubPoint Solutions",
        ProductPresentableName = "reSP Pro", ProductTechnicalName = "ReSharePoint Pro",
        Version = "1.0.0.0",
        ProductUrl = "http://docs.subpointsolutions.com/resp/",
        ProductIcon = typeof(AssetsThemedIcons.ReSPLogo16))]
    public interface IProProductZone : IBasicProductZone
    {
    }

    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("reSP Code Completion", "Is used to provide various helpers when typing SharePoint code.", false)]
    public interface ICodeCompletionZone : IProProductZone
    {
    }

    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("reSP Live Templates", "Great way to quickly create SharePoint code from small snippets.", false)]
    public interface ILiveTemplatesZone : IProProductZone
    {
    }

    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("reSP Highlightings", "Tips to understand sense of SharePoint terms and constants", false)]
    public interface ITooltipZone : IProProductZone
    {
    }

    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("reSP Navigation", "Navigate to declaration in Xml files", false)]
    public interface INavigationZone : IProProductZone
    {
    }

    [ZoneDefinition]
    [ZoneDefinitionConfigurableFeature("reSP Code Generation", "Set of SharePoint code generators", false)]
    public interface IGenerateItemsZone : IProProductZone
    {
    }
}
