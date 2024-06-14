using System;
using System.Collections.Generic;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public static readonly Dictionary<string, string> SPContentTypes = new Dictionary<string, string>()
        {
            {"0x", "System"},
            {"0x01", "Item"},
            {"0x0101", "Document"},
            {"0x010101", "XML Document"},
            {"0x010102", "Picture"},
            {"0x010104", "Unknown Document Type"},
            {"0x010105", "Master Page"},
            {"0x010108", "Wiki Document"},
            {"0x010107", "Document Workflow Item"},
            {"0x0102", "Event"},
            {"0x0103", "Issue"},
            {"0x0104", "Announcement"},
            {"0x0105", "Link"},
            {"0x0106", "Contact"},
            {"0x0107", "Message"},
            {"0x0108", "Task"},
            {"0x0109", "Workflow History"},
            {"0x0110", "BlogPost"},
            {"0x0111", "BlogComment"},
            {"0x012002", "Discussion"},
            {"0x010801", "WorkflowTask"},
            {"0x010802", "Administrative Task"},
            {"0x0120", "Folder"},
            {"0x012001", "RootOfList"},
            {"0x010A", "Person"},
            {"0x010B", "SharePointGroup"},
            {"0x010C", "DomainGroup"},
            {"0x010109", "Basic Page"},
            {"0x01010901", "Web Part Page"},
            {"0x01010A", "Link to a Document"},
            {"0x0116", "Far East Contact"},
            {"0x01010B", "Dublin Core Columns"},
            {"0x010100629D00608F814DD6AC8A86903AEE72AA", "Office Data Connection File"},
            {"0x010100B4CBD48E029A4AD8B62CB0E41868F2B0", "Universal Data Connection File"},
            {"0x01003A8AA7A4F53046158C5ABD98036A01D5", "Health Rule Definition"},
            {"0x0100F95DB3A97E8046b58C6A54FB31F2BD46", "Health Report"},
            {"0x012004", "Summary Task"},
            {"0x0120D5", "Document Set"},
            {"0x0102007DBDC1392EAF4EBBBF99E41D8922B264", "Schedule"},
            {"0x0102004F51EFDEA49C49668EF9C6744C8CF87D", "Resource Reservation"},
            {"0x01020072BB2A38F0DB49C3A96CF4FA85529956", "Schedule And Resource Reservation"},
            {"0x01000F389E14C9CE4CE486270B9D4713A5D6", "GbwCirculationCTName"},
            {"0x01007CE30DD1206047728BAFD1C39A850120", "GbwOfficialNoticeCTName"},
            {"0x0100807FBAC5EB8A4653B8D24775195B5463", "Call Tracking"},
            {"0x01004C9F4486FBF54864A7B0A33D02AD19B1", "Resource"},
            {"0x0100CA13F2F8D61541B180952DFB25E3E8E4", "Resource Group"},
            {"0x01009BE2AB5291BF4C1A986910BD278E4F18", "Holiday"},
            {"0x0100C30DDA8EDB2E434EA22D793D9EE42058", "Timecard"},
            {"0x0100A2CA87FF01B442AD93F37CD7DD0943EB", "Whats New"},
            {"0x0100FBEEE6F0C500489B99CDA6BB16C398F7", "Whereabouts"},
            {"0x010018F21907ED4E401CB4F14422ABC65304", "IMEDictionaryItem"},
            {"0x010100734778F2B7DF462491FC91844AE431CF", "XSLStyle"},
            {"0x010106", "Master Page Preview"},
            {"0x0101002039C03B61C64EC4A04F5361F3851068", "Display Template JS"},
            {"0x010100C568DB52D9D0A14D9B2FDCC96666E9F2", "System Page"},
            {"0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF39", "Page"},
            {"0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF3900242457EFB8B24247815D688C526CD44D", "Article Page"},
            {"0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF390064DEA0F50FC8C147B0B6EA0636C4A7D4", "Welcome Page"},
            {"0x010100C568DB52D9D0A14D9B2FDCC96666E9F2007948130EC3DB064584E219954237AF3900FD0E870BA06948879DBD5F9813CD8799", "Redirect Page"},
            {"0x01002CF74A4DAE39480396EEA7A4BA2BE5FB", "Reusable HTML"},
            {"0x01004D5A79BAFA4A4576B79C56FF3D0D662D", "Reusable Text"},
            {"0x010087D89D279834C94E98E5E1B4A913C67E", "Page Output Cache"},
            {"0x01010007FF3E057FA8AB4AA42FCB67B453FFC1", "System Page Layout"},
            {"0x01010007FF3E057FA8AB4AA42FCB67B453FFC100E214EEE741181F4E9F7ACC43278EE811", "Page Layout"},
            {"0x0101000F1C8B9E0EB4BE489F09807B2C53288F", "System Master Page"},
            {"0x0101000F1C8B9E0EB4BE489F09807B2C53288F0054AD6EF48B9F7B45A142F8173F171BD1", "Publishing Master Page"},
            {"0x01080100C9C9515DE4E24001905074F980F93160", "Office SharePoint Server Workflow Task"}
        };

        public static string GetBuiltInContentTypeName(string value)
        {
            string result = String.Empty;
            if (value.StartsWith("0x"))
                SPContentTypes.TryGetValue(value.ToUpper().Replace("0X", "0x").Trim(), out result);
            return result;
        }
    }
}

