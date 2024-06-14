using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using ReSharePoint.Common.Consts;

[assembly: RegisterConfigurableHighlightingsGroup(Consts.CORRECTNESS_GROUP, Consts.CORRECTNESS_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.BEST_PRACTICE_GROUP, Consts.BEST_PRACTICE_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.SECURITY_GROUP, Consts.SECURITY_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.SHAREPOINT_SUPPORTABILITY_GROUP, Consts.SHAREPOINT_SUPPORTABILITY_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.DESIGN_GROUP, Consts.DESIGN_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.MEMORY_DISPOSAL_GROUP, Consts.MEMORY_DISPOSAL_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.SHAREPOINT_2013_COMPATIBILITY_GROUP, Consts.SHAREPOINT_2013_COMPATIBILITY_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.SANDBOX_COMPATIBILITY_GROUP, Consts.SANDBOX_COMPATIBILITY_GROUP)]
[assembly: RegisterConfigurableHighlightingsGroup(Consts.TOOLTIP_GROUP, Consts.TOOLTIP_GROUP)]

namespace ReSharePoint.Common.Consts
{
    public sealed class Consts
    {
        public const string CORRECTNESS_GROUP = "reSP. Correctness";
        public const string BEST_PRACTICE_GROUP = "reSP. Best practices";
        public const string SECURITY_GROUP = "reSP. Security";
        public const string SHAREPOINT_SUPPORTABILITY_GROUP = "reSP. SharePoint Supportability";
        public const string DESIGN_GROUP = "reSP. Design";
        public const string MEMORY_DISPOSAL_GROUP = "reSP. Memory Disposal";
        public const string SHAREPOINT_2013_COMPATIBILITY_GROUP = "reSP. SharePoint 2013 Compatibility";
        public const string SANDBOX_COMPATIBILITY_GROUP = "reSP. Sandbox compatibility";
        public const string TOOLTIP_GROUP = "reSP. Highlightings";

        public static Guid[] SPFarmSolutionProjectTypeGuids =
        {
            Guid.Parse("{BB1F664B-9266-4fd6-B973-E1E44974B511}"),
            Guid.Parse("{C1CDDADD-2546-481F-9697-4EA41081F2FC}"),
            Guid.Parse("{14822709-B5A1-4724-98CA-57A101D1B079}"), Guid.Parse("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}")
        };

        public static Guid[] SPAppsSolutionProjectTypeGuids =
        {
            Guid.Parse("{349C5851-65DF-11DA-9384-00065B846F21}"),
            Guid.Parse("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}")
        };

        public static char[] ResourceStringChars = {'$', ':', ',', '_', ';', '.', ' '};
    }
}

