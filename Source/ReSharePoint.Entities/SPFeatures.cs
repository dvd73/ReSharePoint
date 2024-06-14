using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public class SPFeature
        {
            public string Title { get; set; }
            public Guid Id { get; set; }
            public bool FromFeatureIds { get; set; }
        }

        public static List<SPFeature> SPFeatures = new List<SPFeature>()
        {
            new SPFeature {Title = "AbuseReportsList", Id = new Guid("c6a92dbf-6441-4b8b-882f-8d97cb12c83a")},
            new SPFeature {Title = "AccessRequests", Id = new Guid("a0f12ee4-9b60-4ba4-81f6-75724f4ca973")},
            new SPFeature {Title = "AccSrvApplication", Id = new Guid("1cc4b32c-299b-41aa-9770-67715ea05f25")},
            new SPFeature {Title = "AccSrvMSysAso", Id = new Guid("29ea7495-fca1-4dc6-8ac1-500c247a036e")},
            new SPFeature {Title = "AccSrvRestrictedList", Id = new Guid("a4d4ee2c-a6cb-4191-ab0a-21bb5bde92fb")},
            new SPFeature {Title = "AccSrvShell", Id = new Guid("bcf89eb7-bca1-4468-bdb4-ca27f61a2292")},
            new SPFeature {Title = "AccSrvSolutionGallery", Id = new Guid("744b5fd3-3b09-4da6-9bd1-de18315b045d")},
            new SPFeature
            {
                Title = "AccSrvSolutionGalleryStapler",
                Id = new Guid("d5ff2d2c-8571-4c3c-87bc-779111979811")
            },
            new SPFeature {Title = "AccSrvUserTemplate", Id = new Guid("1a8251a0-47ab-453d-95d4-07d7ca4f8166")},
            new SPFeature {Title = "AccSrvUSysAppLog", Id = new Guid("28101b19-b896-44f4-9264-db028f307a62")},
            new SPFeature {Title = "AccSvcAddAccessApp", Id = new Guid("d2b9ec23-526b-42c5-87b6-852bd83e0364")},
            new SPFeature {Title = "AccSvcAddAccessAppStapling", Id = new Guid("3d7415e4-61ba-4669-8d78-213d374d9825")},
            new SPFeature {Title = "AccSvcApplication", Id = new Guid("5094e988-524b-446c-b2f6-040b5be46297")},
            new SPFeature {Title = "AccSvcShell", Id = new Guid("7ffd6d57-4b10-4edb-ac26-c2cfbf8173ab")},
            new SPFeature {Title = "AddDashboard", Id = new Guid("d250636f-0a26-4019-8425-a5232d592c09")},
            new SPFeature {Title = "AdminLinks", Id = new Guid("fead7313-ae6d-45dd-8260-13b563cb4c71")},
            new SPFeature {Title = "AdminReportCore", Id = new Guid("b8f36433-367d-49f3-ae11-f7d76b51d251")},
            new SPFeature {Title = "AdminReportCorePushdown", Id = new Guid("55312854-855b-4088-b09d-c5efe0fbf9d2")},
            new SPFeature {Title = "AnnouncementsList", Id = new Guid("00bfea71-d1ce-42de-9c63-a44004ce0104")},
            new SPFeature {Title = "AppLockdown", Id = new Guid("23330bdb-b83e-4e09-8770-8155aa5e87fd")},
            new SPFeature {Title = "AppRegistration", Id = new Guid("fdc6383e-3f1d-4599-8b7c-c515e99cbf18")},
            new SPFeature {Title = "AppRequestsList", Id = new Guid("334dfc83-8655-48a1-b79d-68b7f6c63222")},
            new SPFeature {Title = "AssetLibrary", Id = new Guid("4bcccd62-dcaf-46dc-a7d4-e38277ef33f4"), FromFeatureIds = true},
            new SPFeature {Title = "AutohostedAppLicensing", Id = new Guid("fa7cefd8-5595-4d68-84fa-fe2d9e693de7")},
            new SPFeature
            {
                Title = "AutohostedAppLicensingStapling",
                Id = new Guid("013a0db9-1607-4c42-8f71-08d821d395c2")
            },
            new SPFeature {Title = "BaseSite", Id = new Guid("b21b090c-c796-4b0f-ac0f-7ef1659c20ae"), FromFeatureIds = true},
            new SPFeature {Title = "BaseSiteStapling", Id = new Guid("97a2485f-ef4b-401f-9167-fa4fe177c6f6")},
            new SPFeature {Title = "BaseWeb", Id = new Guid("99fe402e-89a0-45aa-9163-85342e865dc8"), FromFeatureIds = true},
            new SPFeature {Title = "BaseWebApplication", Id = new Guid("4f56f9fa-51a0-420c-b707-63ecbb494db1")},
            new SPFeature {Title = "BasicWebParts", Id = new Guid("00bfea71-1c5e-4a24-b310-ba51c3eb7a57")},
            new SPFeature {Title = "BcsEvents", Id = new Guid("60c8481d-4b54-4853-ab9f-ed7e1c21d7e4")},
            new SPFeature {Title = "BDR", Id = new Guid("3f59333f-4ce1-406d-8a97-9ecb0ff0337f")},
            new SPFeature {Title = "BICenterDashboardsLib", Id = new Guid("f979e4dc-1852-4f26-ab92-d1b2a190afc9")},
            new SPFeature {Title = "BICenterDataConnections", Id = new Guid("3d8210e9-1e89-4f12-98ef-643995339ed4")},
            new SPFeature {Title = "BICenterDataconnectionsLib", Id = new Guid("26676156-91a0-49f7-87aa-37b1d5f0c4d0")},
            new SPFeature
            {
                Title = "BICenterDataConnectionsListInstance",
                Id = new Guid("a64c4402-7037-4476-a290-84cfd56ca01d")
            },
            new SPFeature {Title = "BICenterFeatureStapler", Id = new Guid("3a027b18-36e4-4005-9473-dd73e6756a73")},
            new SPFeature {Title = "BICenterPPSContentPages", Id = new Guid("a354e6b3-6015-4744-bdc2-2fc1e4769e65")},
            new SPFeature {Title = "BICenterPPSNavigationLink", Id = new Guid("faf31b50-880a-4e4f-a21b-597f6b4d6478")},
            new SPFeature
            {
                Title = "BICenterPPSWorkspaceListInstance",
                Id = new Guid("f9c216ad-35c7-4538-abb8-8ec631a5dff7")
            },
            new SPFeature {Title = "BICenterSampleData", Id = new Guid("3992d4ab-fa9e-4791-9158-5ee32178e88a")},
            new SPFeature {Title = "BizAppsCTypes", Id = new Guid("43f41342-1a37-4372-8ca0-b44d881e4434")},
            new SPFeature {Title = "BizAppsFields", Id = new Guid("5a979115-6b71-45a5-9881-cdc872051a69")},
            new SPFeature {Title = "BizAppsListTemplates", Id = new Guid("065c78be-5231-477e-a972-14177cc5b3c7")},
            new SPFeature {Title = "BizAppsSiteTemplates", Id = new Guid("4248e21f-a816-4c88-8cab-79d82201da7b")},
            new SPFeature {Title = "BlogContent", Id = new Guid("0d1c50f7-0309-431c-adfb-b777d5473a65")},
            new SPFeature {Title = "BlogHomePage", Id = new Guid("e4639bb7-6e95-4e2f-b562-03b832dd4793")},
            new SPFeature {Title = "BlogSiteTemplate", Id = new Guid("faf00902-6bab-4583-bd02-84db191801d8")},
            new SPFeature {Title = "BulkWorkflow", Id = new Guid("aeef8777-70c0-429f-8a13-f12db47a6d47")},
            new SPFeature {Title = "BulkWorkflowTimerJob", Id = new Guid("d992aeca-3802-483a-ab40-6c9376300b61")},
            new SPFeature {Title = "CallTrackList", Id = new Guid("239650e3-ee0b-44a0-a22a-48292402b8d8")},
            new SPFeature {Title = "CategoriesList", Id = new Guid("d32700c7-9ec5-45e6-9c89-ea703efca1df")},
            new SPFeature {Title = "CirculationList", Id = new Guid("a568770a-50ba-4052-ab48-37d8029b3f47")},
            new SPFeature {Title = "CmisProducer", Id = new Guid("1fce0577-1f58-4fc2-a996-6c4bcf59eeed")},
            new SPFeature {Title = "CollaborationMailbox", Id = new Guid("502a2d54-6102-4757-aaa0-a90586106368")},
            new SPFeature {Title = "CollaborationMailboxFarm", Id = new Guid("3a11d8ef-641e-4c79-b4d9-be3b17f9607c")},
            new SPFeature {Title = "CommunityPortal", Id = new Guid("2b03956c-9271-4d1c-868a-07df2971ed30")},
            new SPFeature {Title = "CommunitySite", Id = new Guid("961d6a9c-4388-4cf2-9733-38ee8c89afd4")},
            new SPFeature {Title = "ContactsList", Id = new Guid("00bfea71-7e6d-4186-9ba8-c047ac750105")},
            new SPFeature {Title = "ContentDeploymentSource", Id = new Guid("cd1a49b0-c067-4fdd-adfe-69e6f5022c1a"), FromFeatureIds = true},
            new SPFeature {Title = "ContentFollowing", Id = new Guid("7890e045-6c96-48d8-96e7-6a1d63737d71")},
            new SPFeature {Title = "ContentFollowingList", Id = new Guid("a34e5458-8d20-4c0d-b137-e1390f5824a1")},
            new SPFeature {Title = "ContentFollowingStapling", Id = new Guid("e1580c3c-c510-453b-be15-35feb0ddb1a5")},
            new SPFeature {Title = "ContentLightup", Id = new Guid("0f121a23-c6bc-400f-87e4-e6bbddf6916d")},
            new SPFeature {Title = "ContentTypeHub", Id = new Guid("9a447926-5937-44cb-857a-d3829301c73b")},
            new SPFeature {Title = "ContentTypePublish", Id = new Guid("dd903064-c9d8-4718-b4e7-8ab9bd039fff")},
            new SPFeature {Title = "ContentTypeSettings", Id = new Guid("fead7313-4b9e-4632-80a2-ff00a2d83297")},
            new SPFeature {Title = "ContentTypeSyndication", Id = new Guid("34339dc9-dec4-4256-b44a-b30ff2991a64")},
            new SPFeature {Title = "CorporateCatalog", Id = new Guid("0ac11793-9c2f-4cac-8f22-33f93fac18f2")},
            new SPFeature
            {
                Title = "CorporateCuratedGallerySettings",
                Id = new Guid("f8bea737-255e-4758-ab82-e34bb46f5828")
            },
            new SPFeature {Title = "CrossFarmSitePermissions", Id = new Guid("a5aedf1a-12e5-46b4-8348-544386d5312d")},
            new SPFeature
            {
                Title = "CrossSiteCollectionPublishing",
                Id = new Guid("151d22d9-95a8-4904-a0a3-22e4db85d1e0"),
                FromFeatureIds = true
            },
            new SPFeature {Title = "CTypes", Id = new Guid("695b6570-a48b-4a8e-8ea5-26ea7fc1d162")},
            new SPFeature {Title = "CustomList", Id = new Guid("00bfea71-de22-43b2-a848-c05709900100")},
            new SPFeature {Title = "DataConnectionLibrary", Id = new Guid("00bfea71-dbd7-4f72-b8cb-da7ac0440130")},
            new SPFeature
            {
                Title = "DataConnectionLibraryStapling",
                Id = new Guid("cdfa39c6-6413-4508-bccf-bf30368472b3")
            },
            new SPFeature {Title = "DataSourceLibrary", Id = new Guid("00bfea71-f381-423d-b9d1-da7a54c50110")},
            new SPFeature {Title = "DeploymentLinks", Id = new Guid("ca2543e6-29a1-40c1-bba9-bd8510a4c17b"), FromFeatureIds = true},
            new SPFeature {Title = "Developer", Id = new Guid("e374875e-06b6-11e0-b0fa-57f5dfd72085")},
            new SPFeature {Title = "DiscussionsList", Id = new Guid("00bfea71-6a49-43fa-b535-d15c05500108")},
            new SPFeature {Title = "DMContentTypeSettings", Id = new Guid("1ec2c859-e9cb-4d79-9b2b-ea8df09ede22")},
            new SPFeature {Title = "DocId", Id = new Guid("b50e3104-6812-424f-a011-cc90e6327318")},
            new SPFeature {Title = "docmarketplace", Id = new Guid("184c82e7-7eb1-4384-8e8c-62720ef397a0")},
            new SPFeature {Title = "docmarketplacesafecontrols", Id = new Guid("5690f1a0-22b6-4262-b1c2-74f505bc0670")},
            new SPFeature {Title = "docmarketplacesampledata", Id = new Guid("1dfd85c5-feff-489f-a71f-9322f8b13902")},
            new SPFeature {Title = "DocumentLibrary", Id = new Guid("00bfea71-e717-4e80-aa17-d0c71b360101")},
            new SPFeature {Title = "DocumentManagement", Id = new Guid("3a4ce811-6fe0-4e97-a6ae-675470282cf2")},
            new SPFeature {Title = "DocumentRouting", Id = new Guid("7ad5272a-2694-4349-953e-ea5ef290e97c"), FromFeatureIds = true},
            new SPFeature {Title = "DocumentRoutingResources", Id = new Guid("0c8a9a47-22a9-4798-82f1-00e62a96006e"), FromFeatureIds = true},
            new SPFeature {Title = "DocumentSet", Id = new Guid("3bae86a2-776d-499d-9db8-fa4cdc7884f8")},
            new SPFeature {Title = "DownloadFromOfficeDotCom", Id = new Guid("a140a1ac-e757-465d-94d4-2ca25ab2c662")},
            new SPFeature {Title = "EDiscoveryCaseResources", Id = new Guid("e8c02a2a-9010-4f98-af88-6668d59f91a7")},
            new SPFeature {Title = "EDiscoveryConsole", Id = new Guid("250042b9-1aad-4b56-a8a6-69d9fe1c8c2c")},
            new SPFeature {Title = "EduAdminLinks", Id = new Guid("03509cfb-8b2f-4f46-a4c9-8316d1e62a4b")},
            new SPFeature {Title = "EduAdminPages", Id = new Guid("c1b78fe6-9110-42e8-87cb-5bd1c8ab278a")},
            new SPFeature {Title = "EduCommunity", Id = new Guid("bf76fc2c-e6c9-11df-b52f-cb00e0d72085")},
            new SPFeature
            {
                Title = "EduCommunityCustomSiteActions",
                Id = new Guid("739ec067-2b57-463e-a986-354be77bb828")
            },
            new SPFeature {Title = "EduCommunitySite", Id = new Guid("2e030413-c4ff-41a4-8ee0-f6688950b34a")},
            new SPFeature {Title = "EduCourseCommunity", Id = new Guid("a16e895c-e61a-11df-8f6e-103edfd72085")},
            new SPFeature {Title = "EduCourseCommunitySite", Id = new Guid("824a259f-2cce-4006-96cd-20c806ee9cfd")},
            new SPFeature {Title = "EduDashboard", Id = new Guid("5025492c-dae2-4c00-8f34-cd08f7c7c294")},
            new SPFeature {Title = "EduFarmWebApplication", Id = new Guid("cb869762-c694-439e-8d05-cf5ca066f271")},
            new SPFeature {Title = "EduInstitutionAdmin", Id = new Guid("41bfb21c-0447-4c97-bc62-0b07bec262a1")},
            new SPFeature
            {
                Title = "EduInstitutionSiteCollection",
                Id = new Guid("978513c0-1e6c-4efb-b12e-7698963bfd05")
            },
            new SPFeature {Title = "EduMembershipUI", Id = new Guid("bd012a1f-c69b-4a13-b6a4-f8bc3e59760e")},
            new SPFeature {Title = "EduMySiteCommunity", Id = new Guid("abf1a85c-e91a-11df-bf2e-f7acdfd72085")},
            new SPFeature {Title = "EduMySiteHost", Id = new Guid("932f5bb1-e815-4c14-8917-c2bae32f70fe")},
            new SPFeature {Title = "EduSearchDisplayTemplates", Id = new Guid("8d75610e-5ff9-4cd1-aefc-8b926f2af771")},
            new SPFeature {Title = "EduShared", Id = new Guid("08585e12-4762-4cc9-842a-a8d7b074bdb7")},
            new SPFeature {Title = "EduStudyGroupCommunity", Id = new Guid("a46935c3-545f-4c15-a2fd-3a19b62d8a02")},
            new SPFeature {Title = "EduUserCache", Id = new Guid("7f52c29e-736d-11e0-80b8-9edd4724019b")},
            new SPFeature {Title = "EduWebApplication", Id = new Guid("7de489aa-2e4a-46ff-88f0-d1b5a9d43709")},
            new SPFeature {Title = "EMailRouting", Id = new Guid("d44a1358-e800-47e8-8180-adf2d0f77543")},
            new SPFeature {Title = "EmailTemplates", Id = new Guid("397942ec-14bf-490e-a983-95b87d0d29d1")},
            new SPFeature {Title = "EnableAppSideLoading", Id = new Guid("ae3a1339-61f5-4f8f-81a7-abd2da956a7d")},
            new SPFeature {Title = "EnhancedHtmlEditing", Id = new Guid("81ebc0d6-8fb2-4e3f-b2f8-062640037398"), FromFeatureIds = true},
            new SPFeature {Title = "EnhancedTheming", Id = new Guid("068bc832-4951-11dc-8314-0800200c9a66"), FromFeatureIds = true},
            new SPFeature {Title = "EnterpriseWiki", Id = new Guid("76d688ad-c16e-4cec-9b71-7b7f0d79b9cd")},
            new SPFeature {Title = "EnterpriseWikiLayouts", Id = new Guid("a942a218-fa43-4d11-9d85-c01e3e3a37cb")},
            new SPFeature {Title = "EventsList", Id = new Guid("00bfea71-ec85-4903-972d-ebe475780106")},
            new SPFeature {Title = "ExcelServer", Id = new Guid("e4e6a041-bc5b-45cb-beab-885a27079f74")},
            new SPFeature {Title = "ExcelServerEdit", Id = new Guid("b3da33d0-5e51-4694-99ce-705a3ac80dc5")},
            new SPFeature {Title = "ExcelServerSite", Id = new Guid("3cb475e7-4e87-45eb-a1f3-db96ad7cf313")},
            new SPFeature {Title = "ExcelServerWebPart", Id = new Guid("4c42ab64-55af-4c7c-986a-ac216a6e0c0e")},
            new SPFeature {Title = "ExcelServerWebPartStapler", Id = new Guid("c6ac73de-1936-47a4-bdff-19a6fc3ba490")},
            new SPFeature {Title = "ExchangeSync", Id = new Guid("5f68444a-0131-4bb0-b013-454d925681a2")},
            new SPFeature
            {
                Title = "ExchangeSyncSiteSubscription",
                Id = new Guid("7cd95467-1777-4b6b-903e-89e253edc1f7")
            },
            new SPFeature {Title = "ExpirationWorkflow", Id = new Guid("c85e5759-f323-4efb-b548-443d2216efb5")},
            new SPFeature {Title = "ExternalList", Id = new Guid("00bfea71-9549-43f8-b978-e47e54a10600")},
            new SPFeature {Title = "ExternalSubscription", Id = new Guid("5b10d113-2d0d-43bd-a2fd-f8bc879f5abd")},
            new SPFeature {Title = "FacilityList", Id = new Guid("58160a6b-4396-4d6e-867c-65381fb5fbc9")},
            new SPFeature
            {
                Title = "FastCentralAdminHelpCollection",
                Id = new Guid("38969baa-3590-4635-81a4-2049d982adfa")
            },
            new SPFeature {Title = "FastEndUserHelpCollection", Id = new Guid("6e8f2b8d-d765-4e69-84ea-5702574c11d6")},
            new SPFeature {Title = "FastFarmFeatureActivation", Id = new Guid("d2d98dc8-c7e9-46ec-80a5-b38f039c16be")},
            new SPFeature {Title = "FCGroupsList", Id = new Guid("08386d3d-7cc0-486b-a730-3b4cfe1b5509")},
            new SPFeature {Title = "FeaturePushdown", Id = new Guid("0125140f-7123-4657-b70a-db9aa1f209e5")},
            new SPFeature {Title = "Fields", Id = new Guid("ca7bd552-10b1-4563-85b9-5ed1d39c962a")},
            new SPFeature {Title = "FollowingContent", Id = new Guid("a7a2793e-67cd-4dc1-9fd0-43f61581207a")},
            new SPFeature {Title = "GanttTasksList", Id = new Guid("00bfea71-513d-4ca0-96c2-6a47775c0119")},
            new SPFeature {Title = "GBWProvision", Id = new Guid("6e8a2add-ed09-4592-978e-8fa71e6f117c")},
            new SPFeature {Title = "GBWWebParts", Id = new Guid("3d25bd73-7cd4-4425-b8fb-8899977f73de")},
            new SPFeature {Title = "GettingStarted", Id = new Guid("4aec7207-0d02-4f4f-aa07-b370199cd0c7")},
            new SPFeature
            {
                Title = "GettingStartedWithAppCatalogSite",
                Id = new Guid("4ddc5942-98b0-4d70-9f7f-17acfec010e5")
            },
            new SPFeature {Title = "GlobalWebParts", Id = new Guid("319d8f70-eb3a-4b44-9c79-2087a87799d6")},
            new SPFeature {Title = "GridList", Id = new Guid("00bfea71-3a1d-41d3-a0ee-651d11570120")},
            new SPFeature {Title = "GroupWork", Id = new Guid("9c03e124-eef7-4dc6-b5eb-86ccd207cb87")},
            new SPFeature {Title = "HelpLibrary", Id = new Guid("071de60d-4b02-4076-b001-b456e93146fe")},
            new SPFeature {Title = "HierarchyTasksList", Id = new Guid("f9ce21f8-f437-4f7e-8bc6-946378c850f0")},
            new SPFeature {Title = "Hold", Id = new Guid("9e56487c-795a-4077-9425-54a1ecb84282"), FromFeatureIds = true},
            new SPFeature {Title = "HolidaysList", Id = new Guid("9ad4c2d4-443b-4a94-8534-49a23f20ba3c")},
            new SPFeature {Title = "HtmlDesign", Id = new Guid("a4c654e4-a8da-4db3-897c-a386048f7157"), FromFeatureIds = true},
            new SPFeature {Title = "IfeDependentApps", Id = new Guid("7877bbf6-30f5-4f58-99d9-a0cc787c1300")},
            new SPFeature {Title = "IMEDicList", Id = new Guid("1c6a572c-1b58-49ab-b5db-75caf50692e6")},
            new SPFeature {Title = "InPlaceRecords", Id = new Guid("da2e115b-07e4-49d9-bb2c-35e93bb9fca9")},
            new SPFeature {Title = "ipfsAdminLinks", Id = new Guid("a10b6aa4-135d-4598-88d1-8d4ff5691d13")},
            new SPFeature {Title = "IPFSAdminWeb", Id = new Guid("750b8e49-5213-4816-9fa2-082900c0201a")},
            new SPFeature {Title = "IPFSSiteFeatures", Id = new Guid("c88c4ff1-dbf5-4649-ad9f-c6c426ebcbf5")},
            new SPFeature {Title = "IPFSTenantFormsConfig", Id = new Guid("15845762-4ec4-4606-8993-1c0512a98680")},
            new SPFeature {Title = "IPFSTenantWebProxyConfig", Id = new Guid("3c577815-7658-4d4f-a347-cfbb370700a7")},
            new SPFeature {Title = "IPFSWebFeatures", Id = new Guid("a0e5a010-1329-49d4-9e09-f280cdbed37d")},
            new SPFeature {Title = "IssuesList", Id = new Guid("00bfea71-5932-4f9c-ad71-1557e5751100")},
            new SPFeature {Title = "IssueTrackingWorkflow", Id = new Guid("fde5d850-671e-4143-950a-87b473922dc7")},
            new SPFeature {Title = "ItemFormRecommendations", Id = new Guid("39d18bbf-6e0f-4321-8f16-4e3b51212393")},
            new SPFeature {Title = "LegacyDocumentLibrary", Id = new Guid("6e53dd27-98f2-4ae5-85a0-e9a8ef4aa6df")},
            new SPFeature {Title = "LegacyWorkflows", Id = new Guid("c845ed8d-9ce5-448c-bd3e-ea71350ce45b")},
            new SPFeature {Title = "LinksList", Id = new Guid("00bfea71-2062-426c-90bf-714c59600103")},
            new SPFeature {Title = "ListTargeting", Id = new Guid("fc33ba3b-7919-4d7e-b791-c6aeccf8f851")},
            new SPFeature {Title = "LocalSiteDirectoryControl", Id = new Guid("14aafd3a-fcb9-4bb7-9ad7-d8e36b663bbd")},
            new SPFeature {Title = "LocalSiteDirectoryMetaData", Id = new Guid("8f15b342-80b1-4508-8641-0751e2b55ca6")},
            new SPFeature
            {
                Title = "LocalSiteDirectorySettingsLink",
                Id = new Guid("e978b1a6-8de7-49d0-8600-09a250354e14")
            },
            new SPFeature {Title = "LocationBasedPolicy", Id = new Guid("063c26fa-3ccc-4180-8a84-b6f98e991df3")},
            new SPFeature {Title = "MaintenanceLogs", Id = new Guid("8c6f9096-388d-4eed-96ff-698b3ec46fc4")},
            new SPFeature
            {
                Title = "ManageUserProfileServiceApplication",
                Id = new Guid("c59dbaa9-fa01-495d-aaa3-3c02cc2ee8ff")
            },
            new SPFeature {Title = "MasterSiteDirectoryControl", Id = new Guid("8a663fe0-9d9c-45c7-8297-66365ad50427")},
            new SPFeature {Title = "MBrowserRedirect", Id = new Guid("d95c97f3-e528-4da2-ae9f-32b3535fbb59")},
            new SPFeature {Title = "MBrowserRedirectStapling", Id = new Guid("2dd8788b-0e6b-4893-b4c0-73523ac261b1")},
            new SPFeature {Title = "MDSFeature", Id = new Guid("87294c72-f260-42f3-a41b-981a2ffce37a")},
            new SPFeature {Title = "MediaWebPart", Id = new Guid("5b79b49a-2da6-4161-95bd-7375c1995ef9"), FromFeatureIds = true},
            new SPFeature {Title = "MembershipList", Id = new Guid("947afd14-0ea1-46c6-be97-dea1bf6f5bae")},
            new SPFeature {Title = "MetaDataNav", Id = new Guid("7201d6a4-a5d3-49a1-8c19-19c4bac6e668"), FromFeatureIds = true},
            new SPFeature {Title = "MobileEwaFarm", Id = new Guid("5a020a4f-c449-4a65-b07d-f2cc2d8778dd")},
            new SPFeature {Title = "MobileExcelWebAccess", Id = new Guid("e995e28b-9ba8-4668-9933-cf5c146d7a9f")},
            new SPFeature {Title = "MobilityRedirect", Id = new Guid("f41cc668-37e5-4743-b4a8-74d1db3fd8a4")},
            new SPFeature {Title = "MonitoredApps", Id = new Guid("345ff4f9-f706-41e1-92bc-3f0ec2d9f6ea")},
            new SPFeature {Title = "MonitoredAppsUI", Id = new Guid("1b811cfe-8c78-4982-aad7-e5c112e397d1")},
            new SPFeature {Title = "MossChart", Id = new Guid("875d1044-c0cf-4244-8865-d2a0039c2a49")},
            new SPFeature {Title = "MpsWebParts", Id = new Guid("39dd29fb-b6f5-4697-b526-4d38de4893e5")},
            new SPFeature {Title = "MruDocsWebPart", Id = new Guid("1eb6a0c1-5f08-4672-b96f-16845c2448c6")},
            new SPFeature {Title = "MySite", Id = new Guid("69cc9662-d373-47fc-9449-f18d11ff732c")},
            new SPFeature {Title = "MySiteBlog", Id = new Guid("863da2ac-3873-4930-8498-752886210911")},
            new SPFeature {Title = "MySiteCleanup", Id = new Guid("0faf7d1b-95b1-4053-b4e2-19fd5c9bbc88")},
            new SPFeature {Title = "MySiteDocumentLibrary", Id = new Guid("e9c0ff81-d821-4771-8b4c-246aa7e5e9eb")},
            new SPFeature {Title = "MySiteHost", Id = new Guid("49571cd1-b6a1-43a3-bf75-955acc79c8d8")},
            new SPFeature {Title = "MySiteHostPictureLibrary", Id = new Guid("5ede0a86-c772-4f1d-a120-72e734b3400c")},
            new SPFeature {Title = "MySiteInstantiationQueues", Id = new Guid("65b53aaf-4754-46d7-bb5b-7ed4cf5564e1")},
            new SPFeature {Title = "MySiteLayouts", Id = new Guid("6928b0e5-5707-46a1-ae16-d6e52522d52b")},
            new SPFeature {Title = "MySiteMaster", Id = new Guid("fb01ca75-b306-4fc2-ab27-b4814bf823d1")},
            new SPFeature {Title = "MySiteMicroBlog", Id = new Guid("ea23650b-0340-4708-b465-441a41c37af7")},
            new SPFeature {Title = "MySiteMicroBlogCtrl", Id = new Guid("dfa42479-9531-4baf-8873-fc65b22c9bd4")},
            new SPFeature {Title = "MySiteNavigation", Id = new Guid("6adff05c-d581-4c05-a6b9-920f15ec6fd9")},
            new SPFeature {Title = "MySitePersonalSite", Id = new Guid("f661430e-c155-438e-a7c6-c68648f1b119")},
            new SPFeature {Title = "MySiteQuickLaunch", Id = new Guid("034947cc-c424-47cd-a8d1-6014f0e36925")},
            new SPFeature {Title = "MySiteSocialDeployment", Id = new Guid("b2741073-a92b-4836-b1d8-d5e9d73679bb")},
            new SPFeature {Title = "MySiteStorageDeployment", Id = new Guid("0ee1129f-a2f3-41a9-9e9c-c7ee619a8c33")},
            new SPFeature {Title = "MySiteUnifiedNavigation", Id = new Guid("41baa678-ad62-41ef-87e6-62c8917fc0ad")},
            new SPFeature {Title = "MySiteUnifiedQuickLaunch", Id = new Guid("eaa41f18-8e4a-4894-baee-60a87f026e42")},
            new SPFeature {Title = "MyTasksDashboard", Id = new Guid("89d1184c-8191-4303-a430-7a24291531c9")},
            new SPFeature
            {
                Title = "MyTasksDashboardCustomRedirect",
                Id = new Guid("04a98ac6-82d5-4e01-80ea-c0b7d9699d94")
            },
            new SPFeature {Title = "MyTasksDashboardStapling", Id = new Guid("4cc8aab8-5af0-45d7-a170-169ea583866e")},
            new SPFeature {Title = "Navigation", Id = new Guid("89e0306d-453b-4ec5-8d68-42067cdbf98e"), FromFeatureIds = true},
            new SPFeature {Title = "NavigationProperties", Id = new Guid("541f5f57-c847-4e16-b59a-b31e90e6f9ea"), FromFeatureIds = true},
            new SPFeature {Title = "NoCodeWorkflowLibrary", Id = new Guid("00bfea71-f600-43f6-a895-40c0de7b0117")},
            new SPFeature {Title = "ObaProfilePages", Id = new Guid("683df0c0-20b7-4852-87a3-378945158fab")},
            new SPFeature
            {
                Title = "ObaProfilePagesTenantStapling",
                Id = new Guid("90c6c1e5-3719-4c52-9f36-34a97df596f7")
            },
            new SPFeature {Title = "ObaSimpleSolution", Id = new Guid("d250636f-0a26-4019-8425-a5232d592c01")},
            new SPFeature {Title = "ObaStaple", Id = new Guid("f9cb1a2a-d285-465a-a160-7e3e95af1fdd")},
            new SPFeature {Title = "OfficeExtensionCatalog", Id = new Guid("61e874cd-3ac3-4531-8628-28c3acb78279")},
            new SPFeature {Title = "OfficeWebApps", Id = new Guid("0c504a5c-bcea-4376-b05e-cbca5ced7b4f")},
            new SPFeature {Title = "OffWFCommon", Id = new Guid("c9c9515d-e4e2-4001-9050-74f980f93160")},
            new SPFeature {Title = "OnenoteServerViewing", Id = new Guid("3d433d02-cf49-4975-81b4-aede31e16edf")},
            new SPFeature {Title = "OpenInClient", Id = new Guid("8a4b8de2-6fd8-41e9-923c-c7c3c00f8295")},
            new SPFeature
            {
                Title = "OrganizationsClaimHierarchyProvider",
                Id = new Guid("9b0293a7-8942-46b0-8b78-49d29a9edd53")
            },
            new SPFeature {Title = "OSearchBasicFeature", Id = new Guid("bc29e863-ae07-4674-bd83-2c6d0aa5623f")},
            new SPFeature {Title = "OSearchCentralAdminLinks", Id = new Guid("c922c106-7d0a-4377-a668-7f13d52cb80f")},
            new SPFeature {Title = "OSearchEnhancedFeature", Id = new Guid("4750c984-7721-4feb-be61-c660c6190d43")},
            new SPFeature {Title = "OSearchHealthReports", Id = new Guid("e792e296-5d7f-47c7-9dfa-52eae2104c3b")},
            new SPFeature
            {
                Title = "OSearchHealthReportsPushdown",
                Id = new Guid("09fe98f3-3324-4747-97e5-916a28a0c6c0")
            },
            new SPFeature {Title = "OSearchPortalAdminLinks", Id = new Guid("edf48246-e4ee-4638-9eed-ef3d0aee7597")},
            new SPFeature {Title = "OsrvLinks", Id = new Guid("068f8656-bea6-4d60-a5fa-7f077f8f5c20")},
            new SPFeature {Title = "OssNavigation", Id = new Guid("10bdac29-a21a-47d9-9dff-90c7cae1301e")},
            new SPFeature {Title = "OSSSearchEndUserHelpFeature", Id = new Guid("03b0a3dc-93dd-4c68-943e-7ec56e65ed4d")},
            new SPFeature
            {
                Title = "OSSSearchSearchCenterUrlFeature",
                Id = new Guid("7acfcb9d-8e8f-4979-af7e-8aed7e95245e")
            },
            new SPFeature
            {
                Title = "OSSSearchSearchCenterUrlSiteFeature",
                Id = new Guid("7ac8cc56-d28e-41f5-ad04-d95109eb987a")
            },
            new SPFeature {Title = "PageConverters", Id = new Guid("14173c38-5e2d-4887-8134-60f9df889bad")},
            new SPFeature {Title = "PersonalizationSite", Id = new Guid("ed5e77f7-c7b1-4961-a659-0de93080fa36")},
            new SPFeature {Title = "PhonePNSubscriber", Id = new Guid("41e1d4bf-b1a2-47f7-ab80-d5d6cbba3092")},
            new SPFeature {Title = "PictureLibrary", Id = new Guid("00bfea71-52d4-45b3-b544-b1c71b620109")},
            new SPFeature {Title = "PortalLayouts", Id = new Guid("5f3b0127-2f1d-4cfd-8dd2-85ad1fb00bfc")},
            new SPFeature {Title = "PowerView", Id = new Guid("bf8b58f5-ebae-4a70-9848-622beaaf2043")},
            new SPFeature {Title = "PowerViewStapling", Id = new Guid("3b5dc9dd-896c-4d6b-8c73-8f854b3a652b")},
            new SPFeature {Title = "PPSDatasourceLib", Id = new Guid("5d220570-df17-405e-b42d-994237d60ebf")},
            new SPFeature {Title = "PPSMonDatasourceCtype", Id = new Guid("05891451-f0c4-4d4e-81b1-0dabd840bad4")},
            new SPFeature {Title = "PPSRibbon", Id = new Guid("ae31cd14-a866-4834-891a-97c9d37662a2")},
            new SPFeature {Title = "PPSSiteCollectionMaster", Id = new Guid("a1cb5b7f-e5e9-421b-915f-bf519b0760ef")},
            new SPFeature {Title = "PPSSiteMaster", Id = new Guid("0b07a7f4-8bb8-4ec0-a31b-115732b9584d")},
            new SPFeature {Title = "PPSSiteStapling", Id = new Guid("8472208f-5a01-4683-8119-3cea50bea072")},
            new SPFeature {Title = "PPSWebParts", Id = new Guid("ee9dbf20-1758-401e-a169-7db0a6bbccb2")},
            new SPFeature {Title = "PPSWorkspaceCtype", Id = new Guid("f45834c7-54f6-48db-b7e4-a35fa470fc9b")},
            new SPFeature {Title = "PPSWorkspaceList", Id = new Guid("481333e1-a246-4d89-afab-d18c6fe344ce")},
            new SPFeature {Title = "PremiumSearchVerticals", Id = new Guid("9e99f7d7-08e9-455c-b3aa-fc71b9210027")},
            new SPFeature {Title = "PremiumSite", Id = new Guid("8581a8a7-cf16-4770-ac54-260265ddb0b2")},
            new SPFeature {Title = "PremiumSiteStapling", Id = new Guid("a573867a-37ca-49dc-86b0-7d033a7ed2c8")},
            new SPFeature {Title = "PremiumWeb", Id = new Guid("0806d127-06e6-447a-980e-2e90b03101b8")},
            new SPFeature {Title = "PremiumWebApplication", Id = new Guid("0ea1c3b6-6ac0-44aa-9f3f-05e8dbe6d70b")},
            new SPFeature {Title = "Preservation", Id = new Guid("bfc789aa-87ba-4d79-afc7-0c7e45dae01a")},
            new SPFeature {Title = "ProductCatalogListTemplate", Id = new Guid("dd926489-fc66-47a6-ba00-ce0e959c9b41")},
            new SPFeature {Title = "ProductCatalogResources", Id = new Guid("409d2feb-3afb-4642-9462-f7f426a0f3e9"), FromFeatureIds = true},
            new SPFeature {Title = "ProfileSynch", Id = new Guid("af847aa9-beb6-41d4-8306-78e41af9ce25")},
            new SPFeature {Title = "ProjectBasedPolicy", Id = new Guid("2fcd5f8a-26b7-4a6a-9755-918566dba90a")},
            new SPFeature {Title = "ProjectDiscovery", Id = new Guid("4446ee9b-227c-4f1a-897d-d78ecdd6a824")},
            new SPFeature {Title = "ProjectFunctionality", Id = new Guid("e2f2bb18-891d-4812-97df-c265afdba297")},
            new SPFeature {Title = "PromotedLinksList", Id = new Guid("192efa95-e50c-475e-87ab-361cede5dd7f")},
            new SPFeature {Title = "Publishing", Id = new Guid("22a9ef51-737b-4ff2-9346-694633fe4416"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingLayouts", Id = new Guid("d3f51be2-38a8-4e44-ba84-940d35be1566"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingMobile", Id = new Guid("57cc6207-aebf-426e-9ece-45946ea82e4a"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingPrerequisites", Id = new Guid("a392da98-270b-4e85-9769-04c0fde267aa"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingResources", Id = new Guid("aebc918d-b20f-4a11-a1db-9ed84d79c87e"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingSite", Id = new Guid("f6924d36-2fa8-4f0b-b16d-06b7250180fa"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingStapling", Id = new Guid("001f4bd7-746d-403b-aa09-a6cc43de7942")},
            new SPFeature {Title = "PublishingTimerJobs", Id = new Guid("20477d83-8bdb-414e-964b-080637f7d99b"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingWeb", Id = new Guid("94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingB2TRSiteFilesUpgrade", Id = new Guid("FD3DD145-E35E-4871-9A6D-BF17F28A1C19"), FromFeatureIds = true},
            new SPFeature {Title = "PublishingB2TRHop2SiteFilesUpgrade", Id = new Guid("24D7018D-BF48-4813-A28D-DBF3DBA173B1"), FromFeatureIds = true},
            new SPFeature {Title = "QueryBasedPreservation", Id = new Guid("d9742165-b024-4713-8653-851573b9dfbd")},
            new SPFeature {Title = "Ratings", Id = new Guid("915c240e-a6cc-49b8-8b2c-0bff8b553ed3")},
            new SPFeature {Title = "RecordResources", Id = new Guid("5bccb9a4-b903-4fd1-8620-b795fa33c9ba")},
            new SPFeature {Title = "RecordsCenter", Id = new Guid("e0a45587-1069-46bd-bf05-8c8db8620b08")},
            new SPFeature {Title = "RecordsManagement", Id = new Guid("6d127338-5e7d-4391-8f62-a11e43b1d404")},
            new SPFeature
            {
                Title = "RecordsManagementTenantAdmin",
                Id = new Guid("b5ef96cb-d714-41da-b66c-ce3517034c21")
            },
            new SPFeature
            {
                Title = "RecordsManagementTenantAdminStapling",
                Id = new Guid("8c54e5d3-4635-4dff-a533-19fe999435dc")
            },
            new SPFeature
            {
                Title = "RedirectPageContentTypeBinding",
                Id = new Guid("306936fd-9806-4478-80d1-7e397bfa6474")
            },
            new SPFeature
            {
                Title = "RelatedLinksScopeSettingsLink",
                Id = new Guid("e8734bb6-be8e-48a1-b036-5a40ff0b8a81")
            },
            new SPFeature {Title = "ReportAndDataSearch", Id = new Guid("b9455243-e547-41f0-80c1-d5f6ce6a19e5")},
            new SPFeature {Title = "ReportCenterSampleData", Id = new Guid("c5d947d6-b0a2-4e07-9929-8e54f5a9fff9")},
            new SPFeature {Title = "Reporting", Id = new Guid("7094bd89-2cfe-490a-8c7e-fbace37b4a34")},
            new SPFeature {Title = "ReportListTemplate", Id = new Guid("2510d73f-7109-4ccc-8a1c-314894deeb3a")},
            new SPFeature {Title = "ReportsAndDataCTypes", Id = new Guid("e0a9f213-54f5-4a5a-81d5-f5f3dbe48977")},
            new SPFeature {Title = "ReportsAndDataFields", Id = new Guid("365356ee-6c88-4cf1-92b8-fa94a8b8c118")},
            new SPFeature {Title = "ReportsAndDataListTemplates", Id = new Guid("b435069a-e096-46e0-ae30-899daca4b304")},
            new SPFeature {Title = "ReportServer", Id = new Guid("e8389ec7-70fd-4179-a1c4-6fcb4342d7a0")},
            new SPFeature {Title = "ReportServerCentralAdmin", Id = new Guid("5f2e3537-91b5-4341-86ff-90c6a2f99aae")},
            new SPFeature {Title = "ReportServerItemSync", Id = new Guid("c769801e-2387-47ef-a810-2d292d4cb05d")},
            new SPFeature {Title = "ReportServerStapling", Id = new Guid("6bcbccc3-ff47-47d3-9468-572bf2ab9657")},
            new SPFeature {Title = "ReviewPublishingSPD", Id = new Guid("a44d2aa3-affc-4d58-8db4-f4a3af053188")},
            new SPFeature {Title = "ReviewWorkflows", Id = new Guid("02464c6a-9d07-4f30-ba04-e9035cf54392")},
            new SPFeature {Title = "ReviewWorkflowsSPD", Id = new Guid("b5934f65-a844-4e67-82e5-92f66aafe912")},
            new SPFeature {Title = "RollupPageLayouts", Id = new Guid("588b23d5-8e23-4b1b-9fe3-2f2f62965f2d")},
            new SPFeature {Title = "RollupPages", Id = new Guid("dffaae84-60ee-413a-9600-1cf431cf0560"), FromFeatureIds = true},
            new SPFeature {Title = "ScheduleList", Id = new Guid("636287a7-7f62-4a6e-9fcc-081f4672cbf8")},
            new SPFeature {Title = "SearchAdminWebParts", Id = new Guid("c65861fa-b025-4634-ab26-22a23e49808f")},
            new SPFeature {Title = "SearchAndProcess", Id = new Guid("1dbf6063-d809-45ea-9203-d3ba4a64f86d")},
            new SPFeature {Title = "SearchCenterFiles", Id = new Guid("6077b605-67b9-4937-aeb6-1d41e8f5af3b")},
            new SPFeature {Title = "SearchCenterLiteFiles", Id = new Guid("073232a0-1868-4323-a144-50de99c70efc")},
            new SPFeature {Title = "SearchCenterLiteUpgrade", Id = new Guid("fbbd1168-3b17-4f29-acb4-ef2d34c54cfb")},
            new SPFeature {Title = "SearchCenterUpgrade", Id = new Guid("372b999f-0807-4427-82dc-7756ae73cb74")},
            new SPFeature {Title = "SearchConfigContentType", Id = new Guid("48a243cb-7b16-4b5a-b1b5-07b809b43f47")},
            new SPFeature {Title = "SearchConfigFields", Id = new Guid("41dfb393-9eb6-4fe4-af77-28e4afce8cdc")},
            new SPFeature {Title = "SearchConfigList", Id = new Guid("acb15743-f07b-4c83-8af3-ffcfdf354965")},
            new SPFeature {Title = "SearchConfigListTemplate", Id = new Guid("e47705ec-268d-4c41-aa4e-0d8727985ebc")},
            new SPFeature {Title = "SearchConfigTenantStapler", Id = new Guid("9fb35ca8-824b-49e6-a6c5-cba4366444ab")},
            new SPFeature {Title = "SearchDrivenContent", Id = new Guid("592ccb4a-9304-49ab-aab1-66638198bb58"), FromFeatureIds = true},
            new SPFeature {Title = "SearchEngineOptimization", Id = new Guid("17415b1d-5339-42f9-a10b-3fef756b84d1"), FromFeatureIds = true},
            new SPFeature {Title = "SearchExtensions", Id = new Guid("5eac763d-fbf5-4d6f-a76b-eded7dd7b0a5")},
            new SPFeature {Title = "SearchMaster", Id = new Guid("9c0834e1-ba47-4d49-812b-7d4fb6fea211")},
            new SPFeature {Title = "SearchServerWizardFeature", Id = new Guid("e09cefae-2ada-4a1d-aee6-8a8398215905")},
            new SPFeature
            {
                Title = "SearchTaxonomyRefinementWebParts",
                Id = new Guid("67ae7d04-6731-42dd-abe1-ba2a5eaa3b48"), 
                FromFeatureIds = true
            },
            new SPFeature
            {
                Title = "SearchTaxonomyRefinementWebPartsHtml",
                Id = new Guid("8c34f59f-8dfb-4a39-9a08-7497237e3dc4"), 
                FromFeatureIds = true
            },
            new SPFeature {Title = "SearchTemplatesandResources", Id = new Guid("8b2c6bcb-c47f-4f17-8127-f8eae47a44dd")},
            new SPFeature {Title = "SearchWebParts", Id = new Guid("eaf6a128-0482-4f71-9a2f-b1c650680e77")},
            new SPFeature {Title = "SearchWebPartsStapler", Id = new Guid("922ed989-6eb4-4f5e-a32e-27f31f93abfa")},
            new SPFeature {Title = "SharedServices", Id = new Guid("f324259d-393d-4305-aa48-36e8d9a7a0d6")},
            new SPFeature {Title = "ShareWithEveryone", Id = new Guid("10f73b29-5779-46b3-85a8-4817a6e9a6c2")},
            new SPFeature {Title = "ShareWithEveryoneStapling", Id = new Guid("87866a72-efcf-4993-b5b0-769776b5283f")},
            new SPFeature {Title = "SignaturesWorkflow", Id = new Guid("6c09612b-46af-4b2f-8dfc-59185c962a29")},
            new SPFeature {Title = "SignaturesWorkflowSPD", Id = new Guid("c4773de6-ba70-4583-b751-2a7b1dc67e3a")},
            new SPFeature {Title = "SiteAssets", Id = new Guid("98d11606-9a9b-4f44-b4c2-72d72f867da9")},
            new SPFeature {Title = "SiteFeed", Id = new Guid("15a572c6-e545-4d32-897a-bab6f5846e18")},
            new SPFeature {Title = "SiteFeedController", Id = new Guid("5153156a-63af-4fac-b557-91bd8c315432")},
            new SPFeature {Title = "SiteFeedStapling", Id = new Guid("6301cbb8-9396-45d1-811a-757567d35e91")},
            new SPFeature {Title = "SiteHelp", Id = new Guid("57ff23fc-ec05-4dd8-b7ed-d93faa7c795d")},
            new SPFeature {Title = "SiteNotebook", Id = new Guid("f151bb39-7c3b-414f-bb36-6bf18872052f")},
            new SPFeature {Title = "SiteServicesAddins", Id = new Guid("b21c5a20-095f-4de2-8935-5efde5110ab3"), FromFeatureIds = true},
            new SPFeature {Title = "SiteSettings", Id = new Guid("fead7313-4b9e-4632-80a2-98a2a2d83297")},
            new SPFeature {Title = "SitesList", Id = new Guid("a311bf68-c990-4da3-89b3-88989a3d7721")},
            new SPFeature {Title = "SiteStatusBar", Id = new Guid("001f4bd7-746d-403b-aa09-a6cc43de7999")},
            new SPFeature {Title = "SiteUpgrade", Id = new Guid("b63ef52c-1e99-455f-8511-6a706567740f")},
            new SPFeature {Title = "SkuUpgradeLinks", Id = new Guid("937f97e9-d7b4-473d-af17-b03951b2c66b")},
            new SPFeature {Title = "SlideLibrary", Id = new Guid("0be49fe9-9bc9-409d-abf9-702753bd878d")},
            new SPFeature {Title = "SlideLibraryActivation", Id = new Guid("65d96c6b-649a-4169-bf1d-b96505c60375")},
            new SPFeature {Title = "SmallBusinessWebsite", Id = new Guid("48c33d5d-acff-4400-a684-351c2beda865"), FromFeatureIds = true},
            new SPFeature {Title = "SocialDataStore", Id = new Guid("fa8379c9-791a-4fb0-812e-d0cfcac809c8")},
            new SPFeature {Title = "SocialRibbonControl", Id = new Guid("756d8a58-4e24-4288-b981-65dc93f9c4e5")},
            new SPFeature {Title = "SocialSite", Id = new Guid("4326e7fc-f35a-4b0f-927c-36264b0a4cf0")},
            new SPFeature {Title = "SPAppAnalyticsUploaderJob", Id = new Guid("abf42bbb-cd9b-4313-803b-6f4a7bd4898f")},
            new SPFeature {Title = "SpellChecking", Id = new Guid("612d671e-f53d-4701-96da-c3a4ee00fdc5"), FromFeatureIds = true},
            new SPFeature {Title = "SPSBlog", Id = new Guid("d97ded76-7647-4b1e-b868-2af51872e1b3")},
            new SPFeature {Title = "SPSBlogStapling", Id = new Guid("6d503bb6-027e-44ea-b54c-a53eac3dfed8")},
            new SPFeature {Title = "SPSDisco", Id = new Guid("713a65a1-2bc7-4e62-9446-1d0b56a8bf7f")},
            new SPFeature {Title = "SPSearchFeature", Id = new Guid("2ac1da39-c101-475c-8601-122bc36e3d67")},
            new SPFeature {Title = "SRPProfileAdmin", Id = new Guid("c43a587e-195b-4d29-aba8-ebb22b48eb1a")},
            new SPFeature {Title = "SSSvcAdmin", Id = new Guid("35f680d4-b0de-4818-8373-ee0fca092526")},
            new SPFeature {Title = "StapledWorkflows", Id = new Guid("ee21b29b-b0d0-42c6-baff-c97fd91786e6")},
            new SPFeature {Title = "SurveysList", Id = new Guid("00bfea71-eb8a-40b1-80c7-506be7590102")},
            new SPFeature {Title = "TaskListNewsFeed", Id = new Guid("ff13819a-a9ac-46fb-8163-9d53357ef98d")},
            new SPFeature {Title = "TasksList", Id = new Guid("00bfea71-a83e-497e-9ba0-7a5c597d0107")},
            new SPFeature {Title = "TaxonomyFeatureStapler", Id = new Guid("415780bf-f710-4e2c-b7b0-b463c7992ef0")},
            new SPFeature {Title = "TaxonomyFieldAdded", Id = new Guid("73ef14b1-13a9-416b-a9b5-ececa2b0604c")},
            new SPFeature {Title = "TaxonomyTenantAdmin", Id = new Guid("7d12c4c3-2321-42e8-8fb6-5295a849ed08")},
            new SPFeature {Title = "TaxonomyTenantAdminStapler", Id = new Guid("8fb893d6-93ee-4763-a046-54f9e640368d")},
            new SPFeature {Title = "TaxonomyTimerJobs", Id = new Guid("48ac883d-e32e-4fd6-8499-3408add91b53")},
            new SPFeature {Title = "TeamCollab", Id = new Guid("00bfea71-4ea5-48d4-a4ad-7ea5c011abe5")},
            new SPFeature {Title = "TemplateDiscovery", Id = new Guid("ff48f7e6-2fa1-428d-9a15-ab154762043d")},
            new SPFeature {Title = "TenantAdminBDC", Id = new Guid("0a0b2e8f-e48e-4367-923b-33bb86c1b398")},
            new SPFeature {Title = "TenantAdminBDCStapling", Id = new Guid("b5d169c9-12db-4084-b68d-eef9273bd898")},
            new SPFeature {Title = "TenantAdminDeploymentLinks", Id = new Guid("99f380b4-e1aa-4db0-92a4-32b15e35b317"), FromFeatureIds = true},
            new SPFeature {Title = "TenantAdminLinks", Id = new Guid("98311581-29c5-40e8-9347-bd5732f0cb3e")},
            new SPFeature {Title = "TenantAdminSecureStore", Id = new Guid("b738400a-f08a-443d-96fa-a852d0356bba")},
            new SPFeature
            {
                Title = "TenantAdminSecureStoreStapling",
                Id = new Guid("6361e2a8-3bc4-4ca4-abbb-3dfbb727acd7")
            },
            new SPFeature {Title = "TenantProfileAdmin", Id = new Guid("32ff5455-8967-469a-b486-f8eaf0d902f9")},
            new SPFeature {Title = "TenantProfileAdminStapling", Id = new Guid("3d4ea296-0b35-4a08-b2bf-f0a8cabd1d7f")},
            new SPFeature {Title = "TenantSearchAdmin", Id = new Guid("983521d7-9c04-4db0-abdc-f7078fc0b040")},
            new SPFeature {Title = "TenantSearchAdminStapling", Id = new Guid("08ee8de1-8135-4ef9-87cb-a4944f542ba3")},
            new SPFeature {Title = "TimeCardList", Id = new Guid("d5191a77-fa2d-4801-9baf-9f4205c9e9d2")},
            new SPFeature {Title = "TopicPageLayouts", Id = new Guid("742d4c0e-303b-41d7-8015-aad1dfd54cbd")},
            new SPFeature {Title = "TopicPages", Id = new Guid("5ebe1445-5910-4c6e-ac27-da2e93b60f48")},
            new SPFeature {Title = "Translation", Id = new Guid("4e7276bc-e7ab-4951-9c4b-a74d44205c32")},
            new SPFeature {Title = "TranslationTimerJobs", Id = new Guid("d085b8dc-9205-48a4-96ea-b40782abba02")},
            new SPFeature {Title = "TranslationWorkflow", Id = new Guid("c6561405-ea03-40a9-a57f-f25472942a22")},
            new SPFeature {Title = "TransMgmtFunc", Id = new Guid("82e2ea42-39e2-4b27-8631-ed54c1cfc491")},
            new SPFeature {Title = "TransMgmtLib", Id = new Guid("29d85c25-170c-4df9-a641-12db0b9d4130")},
            new SPFeature {Title = "UPAClaimProvider", Id = new Guid("5709886f-13cc-4ffc-bfdc-ec8ab7f77191")},
            new SPFeature {Title = "UpgradeOnlyFile", Id = new Guid("2fa4db13-4109-4a1d-b47c-c7991d4cc934")},
            new SPFeature {Title = "UserMigrator", Id = new Guid("f0deabbb-b0f6-46ba-8e16-ff3b44461aeb")},
            new SPFeature
            {
                Title = "UserProfileUserSettingsProvider",
                Id = new Guid("0867298a-70e0-425f-85df-7f8bd9e06f15")
            },
            new SPFeature {Title = "V2VPublishedLinks", Id = new Guid("f63b7696-9afc-4e51-9dfd-3111015e9a60"), FromFeatureIds = true},
            new SPFeature {Title = "V2VPublishingLayouts", Id = new Guid("2fbbe552-72ac-11dc-8314-0800200c9a66")},
            new SPFeature {Title = "VideoAndRichMedia", Id = new Guid("6e1e5426-2ebd-4871-8027-c5ca86371ead"), FromFeatureIds = true},
            new SPFeature {Title = "ViewFormPagesLockDown", Id = new Guid("7c637b23-06c4-472d-9a9a-7c175762c5c4"), FromFeatureIds = true},
            new SPFeature {Title = "VisioProcessRepository", Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d01")},
            new SPFeature
            {
                Title = "VisioProcessRepositoryContentTypes",
                Id = new Guid("12e4f16b-8b04-42d2-90f2-aef1cc0b65d9")
            },
            new SPFeature
            {
                Title = "VisioProcessRepositoryContentTypesUs",
                Id = new Guid("b1f70691-6170-4cae-bc2e-4f7011a74faa")
            },
            new SPFeature
            {
                Title = "VisioProcessRepositoryFeatureStapling",
                Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d00")
            },
            new SPFeature {Title = "VisioProcessRepositoryUs", Id = new Guid("7e0aabee-b92b-4368-8742-21ab16453d02")},
            new SPFeature {Title = "VisioServer", Id = new Guid("5fe8e789-d1b7-44b3-b634-419c531cfdca")},
            new SPFeature {Title = "VisioWebAccess", Id = new Guid("9fec40ea-a949-407d-be09-6cba26470a0c")},
            new SPFeature {Title = "WAWhatsPopularWebPart", Id = new Guid("8e947bf0-fe40-4dff-be3d-a8b88112ade6")},
            new SPFeature {Title = "WebPageLibrary", Id = new Guid("00bfea71-c796-4402-9f2f-0eb9a6e71b18")},
            new SPFeature {Title = "WebPartAdderGroups", Id = new Guid("2ed1c45e-a73b-4779-ae81-1524e4de467a")},
            new SPFeature {Title = "WhatsNewList", Id = new Guid("d7670c9c-1c29-4f44-8691-584001968a74")},
            new SPFeature {Title = "WhereaboutsList", Id = new Guid("9c2ef9dc-f733-432e-be1c-2e79957ea27b")},
            new SPFeature {Title = "WikiPageHomePage", Id = new Guid("00bfea71-d8fe-4fec-8dad-01c19a6e4053")},
            new SPFeature {Title = "WikiWelcome", Id = new Guid("8c6a6980-c3d9-440e-944c-77f93bc65a7e")},
            new SPFeature {Title = "WordServerViewing", Id = new Guid("1663ee19-e6ab-4d47-be1b-adeb27cfd9d2")},
            new SPFeature
            {
                Title = "WorkflowAppOnlyPolicyManager",
                Id = new Guid("ec918931-c874-4033-bd09-4f36b2e31fef")
            },
            new SPFeature {Title = "WorkflowHistoryList", Id = new Guid("00bfea71-4ea5-48d4-a4ad-305cf7030140")},
            new SPFeature {Title = "workflowProcessList", Id = new Guid("00bfea71-2d77-4a75-9fca-76516689e21a")},
            new SPFeature {Title = "Workflows", Id = new Guid("0af5989a-3aea-4519-8ab0-85d91abe39ff")},
            new SPFeature {Title = "WorkflowServiceStapler", Id = new Guid("8b82e40f-2001-4f0e-9ce3-0b27d1866dff")},
            new SPFeature {Title = "WorkflowServiceStore", Id = new Guid("2c63df2b-ceab-42c6-aeff-b3968162d4b1")},
            new SPFeature {Title = "WorkflowTask", Id = new Guid("57311b7a-9afd-4ff0-866e-9393ad6647b1")},
            new SPFeature {Title = "XmlFormLibrary", Id = new Guid("00bfea71-1e1d-4562-b56a-f05371bb0115")},
            new SPFeature {Title = "XmlSitemap", Id = new Guid("77fc9e13-e99a-4bd3-9438-a3f69670ed97"), FromFeatureIds = true}

        };

        public static string GetBuiltInFeatureName(Guid value)
        {
            string result = String.Empty;

            var feature = SPFeatures.FirstOrDefault(key => key.Id == value);

            if (feature != null)
                result = feature.Title;

            return result;
        }

        public static string GetFeatureIds(Guid value)
        {
            string result = String.Empty;

            var feature = SPFeatures.FirstOrDefault(key => key.Id == value && key.FromFeatureIds);

            if (feature != null)
                result = feature.Title;

            return result;
        }
    }
}
