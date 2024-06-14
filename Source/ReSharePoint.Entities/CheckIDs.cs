namespace ReSharePoint.Entities
{
    public static class CheckIDs
    {
        #region properties

        public const string ReSPRulePrefix = "RESP";
        public const string SPCAFRulePrefix = "RESP";
        public const string SPMETARulePrefix = "SPM";

        public const string ReSPCategory = "51";
        public const string SPCAFCategory = "52";
        public const string SPMETACategory = "";

        #endregion

        public static class Rules
        {
            public static class TemplateFiles
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.TemplateFiles;

                // not used: public const string UnresolvedTokenAssemblyFullName = Prefix + "01";
            }

            public static class ListInstance
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.ListInstance;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.ListInstance;

                // not used: public const string AvoidDataRowsInListInstances = Prefix + "01";
                public const string UniqueListInstanceUrl = Prefix + "02";
                public const string DoNotUseSystemListNames = Prefix + "03";
                public const string UniqueListInstanceTitle = Prefix + "04";
                public const string WrongFeatureIdInListInstance = Prefix + "05";
                public const string FeatureIdTooltip = Prefix + "06";
                public const string FeatureIdTooltip2 = Prefix + "07";
                public const string ListTypeTooltip1 = Prefix + "08";
                public const string ListTypeTooltip2 = Prefix + "09";
                public const string ContentTypeTooltip = Prefix + "10";
                public const string ContentType2Tooltip = Prefix + "11";
                public const string WebTemplateConfigurationTooltip = Prefix + "12";
                public const string SPC015402 = SPCAFPrefix + "11";
                public const string SPC045401 = SPCAFPrefix + "12";
                public const string SPC015404 = SPCAFPrefix + "13";
                public const string SPC015401 = SPCAFPrefix + "14";
                public const string SPC015405 = SPCAFPrefix + "15";

            }

            public static class ASPXMasterPage
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.MasterPage;

                public const string AvoidDollarGlobalVariableInMasterPage = Prefix + "01";
                // not used: public const string SPDataSourceScopeDoesNotDefinedInMasterPage = Prefix + "02";
                public const string AvoidJQueryDocumentReadyInMasterPage = Prefix + "03";
                // not used: public const string AvoidUsingUpdatePanelInMasterPage = Prefix + "04";
                // not used: public const string SetAutoGenerateColumnsForSPGridViewInMasterPage = Prefix + "05";
            }

            public static class ASPXPage
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.ASPXPage;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.ASPXPage;

                public const string AvoidDollarGlobalVariableInPage = Prefix + "01";
                public const string SPDataSourceScopeDoesNotDefinedInPage = Prefix + "02";
                public const string AvoidJQueryDocumentReadyInPage = Prefix + "03";
                // not used: public const string AvoidUsingUpdatePanelInPage = Prefix + "04";
                public const string SetAutoGenerateColumnsForSPGridViewInPage = Prefix + "05";
                public const string SPC026901 = SPCAFPrefix + "06";
                public const string SPC046902 = SPCAFPrefix + "07";
                public const string SPC046903 = SPCAFPrefix + "08";
            }

            public static class ASCXControl
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.ASCXControl;

                public const string AvoidDollarGlobalVariableInControl = Prefix + "01";
                // not used: public const string SPDataSourceScopeDoesNotDefinedInControl = Prefix + "02";
                // not used: public const string AvoidUsingRenderingTemplate = Prefix + "03";
                public const string AvoidJQueryDocumentReadyInControl = Prefix + "04";
                // not used: public const string AvoidUsingUpdatePanelInControl = Prefix + "05";
                // not used: public const string SetAutoGenerateColumnsForSPGridViewInControl = Prefix + "06";
            }

            public static class General
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.General;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.General;

                // not used: public const string PowerShellHostAssemblyFileReferenceRule = Prefix + "01";
                // not used: public const string LoggerStatisticCollector = Prefix + "03";
                public const string NotProvisionedEntity = Prefix + "04";
                public const string SPC019902 = SPCAFPrefix + "05";
                public const string SPC019903 = SPCAFPrefix + "06";
            }

            public static class ContentType
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.ContentType;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.ContentType;

                public const string AvoidListContentTypes = Prefix + "01";
                public const string DoNotDefineMultipleContentTypeGroupInOneElementFile = Prefix + "02";
                public const string DeployContentTypesCorrectly = Prefix + "03";
                public const string ConsiderOverwriteAttributeForContentType = Prefix + "04";
                public const string AvoidCommentsForContentType = Prefix + "05";
                public const string SPC045201 = SPCAFPrefix + "06";
                public const string SPC045203 = SPCAFPrefix + "07";
                public const string SPC015203 = SPCAFPrefix + "08";
                public const string SPC015210 = SPCAFPrefix + "09";
                public const string SPC015211 = SPCAFPrefix + "10";
                public const string SPC015212 = SPCAFPrefix + "11";
                public const string SPC015207 = SPCAFPrefix + "12";
                public const string SPC015208 = SPCAFPrefix + "13";
                public const string SPC015201 = SPCAFPrefix + "14";
                public const string SPC015202 = SPCAFPrefix + "15";
                public const string SPC015205 = SPCAFPrefix + "16";
                public const string SPC015204 = SPCAFPrefix + "17";
            }

            public static class ListTemplate
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.ListTemplate;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.ListTemplate;

                public const string ConsiderHiddenListTemplates = Prefix + "01";
                public const string DeclareEmptyFieldsElement = Prefix + "02";
                public const string DoNotDeployTaxonomyFieldsInList = Prefix + "03";
                public const string EnsureFolderContentTypeInListDefinition = Prefix + "04";
                public const string DoNotAllowDeletionForList = Prefix + "05";
                public const string RepeatCalculatedFieldsInListSchema = Prefix + "06";
                public const string RepeatLookupFieldsInListSchema = Prefix + "07";
                public const string SPC017510 = SPCAFPrefix + "08";
                public const string SPC017511 = SPCAFPrefix + "09";
                public const string SPC015502 = SPCAFPrefix + "10";
                public const string SPC055501 = SPCAFPrefix + "11";
                public const string SPC015506 = SPCAFPrefix + "12";
                public const string SPC015505 = SPCAFPrefix + "13";
                public const string SPC015501 = SPCAFPrefix + "14";
                public const string SPC025501 = SPCAFPrefix + "15";
            }

            public static class FieldTemplate
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.Field;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.Field;

                public const string DeployTaxonomyFieldsCorrectly = Prefix + "01";
                public const string DoNotDefineMultipleFieldGroupInOneElementFile = Prefix + "02";
                public const string DeployFieldsCorrectly = Prefix + "03";
                public const string ConsiderOverwriteAttributeForField = Prefix + "04";
                public const string FieldIdShouldBeUppercase = Prefix + "05";
                public const string NameWithPictureForUserField = Prefix + "06";
                public const string DoNotAllowDeletionForField = Prefix + "07";
                public const string DoNotUseUnderscoreInFieldName = Prefix + "08";
                public const string UniqueFieldName = Prefix + "09";
                public const string UniqueFieldStaticName = Prefix + "10";
                public const string FieldNameLengthLimitExceed = Prefix + "11";
                public const string DoNotSpecifyIndexedAttributeForNoteField = Prefix + "12";
                public const string DifferentInternalAndStaticFieldNames = Prefix + "13";
                public const string DoNotProvisionLookupFieldBeforeRelatedList = Prefix + "14";
                public const string MixedIDInFieldName = Prefix + "15";
                public const string SPC015102 = SPCAFPrefix + "16";
                public const string SPC015108 = SPCAFPrefix + "17";
                public const string SPC015105 = SPCAFPrefix + "18";
                public const string SPC015106 = SPCAFPrefix + "19";
                public const string SPC015104 = SPCAFPrefix + "20";
                public const string SPC015103 = SPCAFPrefix + "21";
                public const string SPC015101 = SPCAFPrefix + "22";
                public const string SPC015107 = SPCAFPrefix + "23";
            }

            public static class WebPart
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.WebPart;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.WebPart;

                public const string WebPartDefinitionMightBeImproved = Prefix + "01";
                public const string WebPartModuleDefinitionMightbeImproved = Prefix + "02";
                // not used: public const string AvoidDollarGlobalVariableInWebPart = Prefix + "03";
                // not used: public const string AvoidJQueryDocumentReadyInWebPart = Prefix + "04";
                public const string SPC046401 = SPCAFPrefix + "05";
                public const string SPC016401 = SPCAFPrefix + "06";
            }

            public static class Assembly
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.Assembly;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.Assembly;

                public const string ThreadSleepShouldNotBeUsed = Prefix + "01";
                public const string ConfigurationManagerShouldNotBeUsed = Prefix + "02";
                public const string UseBuiltInUserProfilePropertyContantInsteadOfStrings = Prefix + "03";
                public const string AvoidEnumeratingAllUserProfiles = Prefix + "04";
                public const string SPViewScopeDoesNotDefined = Prefix + "05";
                public const string UseSecureStoreService = Prefix + "06";
                // not used: public const string UsingSPWebGetFilePreferable = Prefix + "07";
                public const string SPMonitoredScopeShouldBeUsed = Prefix + "08";
                public const string InappropriateUsageOfSPListCollection = Prefix + "09";
                public const string SPQueryScopeDoesNotDefined = Prefix + "10";
                // not used: public const string AvoidUsingAjaxControlToolkit = Prefix + "11";
                public const string DoNotUseDirectorySearcher = Prefix + "12";
                public const string DoNotUseUnsafeTypeConversionOnSPListItem = Prefix + "13";
                // not used: public const string SPWebEnsureUserMethodUsage = Prefix + "14";
                // not used: public const string SPWebRequestAccessEmailPropertyUsage = Prefix + "15";
                // not used: public const string AvoidHeavyOperationsInsideRepeaterItemEventHandlers = Prefix + "16";
                // not used: public const string AvoidInappropriateDataAccess = Prefix + "17";
                public const string SPDataSourceScopeDoesNotDefined = Prefix + "18";
                // not used: public const string DoNotDeleteUserProfile = Prefix + "19";
                // not used: public const string AvoidObsoleteWebServicesInCode = Prefix + "20";
                // not used: public const string AvoidObsoleteWebServicesInJavaScriptFiles = Prefix + "21";
                public const string AvoidUsingSPContextOutsideOfWebContext = Prefix + "22";
                public const string AvoidJQueryDocumentReadyInCode = Prefix + "23";
                // not used: public const string CamlexQueryDoubleWhere = Prefix + "24";
                public const string MagicStringShouldNotBeUsed = Prefix + "25";
                public const string AvoidUsingSPListItemFile = Prefix + "26";
                public const string InappropriateUsageOfTaxonomyGroupCollection = Prefix + "27";
                public const string AvoidUnsafeUrlConcatenations = Prefix + "28";
                // not used: public const string LoadJavaScriptWithinSandbox = Prefix + "29";
                // not used: public const string RepeatableMagicStringShouldNotBeUsed = Prefix + "30";
                // not used: public const string ListModificationFromJob = Prefix + "31";
                public const string GetPublishingPages = Prefix + "32";
                // not used: public const string AvoidSPObjectsInFields = Prefix + "33";
                public const string AvoidStaticSPObjectsInFields = Prefix + "34";
                public const string AvoidSPObjectNameStringComparison = Prefix + "35";
                public const string DoNotSuppressExceptions = Prefix + "36";
                // not used: public const string ULSLoggingShouldBeUsed = Prefix + "37";
                public const string SpecifySPZoneInSPSite = Prefix + "38";
                public const string NoCustomLogging = Prefix + "39";
                public const string DoNotUseSPWebProperties = Prefix + "40";
                public const string ULSLoggingInCatchBlock = Prefix + "41";
                public const string AvoidDollarGlobalVariableInCode = Prefix + "42";
                public const string DoNotChangeSPPersistedObject = Prefix + "43";
                public const string OutOfContextRWEP = Prefix + "44";
                public const string DoNotUsePortalLog = Prefix + "45";
                // not used: public const string DoNotDisposePersonalSiteWeb = Prefix + "46";
                public const string WrongSPViewUsage = Prefix + "47";
                // not used: public const string AvoidUsingUpdatePanelInCode = Prefix + "48";
                public const string OutOfContextSPWebPartManager = Prefix + "49";
                public const string PutSPFileExistsIntoTryCatchBlock = Prefix + "50";
                public const string DoNotUseEntityEditorEntities = Prefix + "51";
                public const string SendMailFromWcfService = Prefix + "52";
                public const string DoNotGetUtcTimeFromDateTime = Prefix + "53";
                public const string UseBuiltInFeatureInsteadOfStrings = Prefix + "54";
                public const string UseBuiltInPublishingFieldInsteadOfStrings = Prefix + "55";
                public const string UseBuiltInFieldInsteadOfStrings = Prefix + "56";
                public const string ProcessingIdParameterForAddWorkItemShouldBeEmpty = Prefix + "57";
                public const string DoNotUseSPContentTypeFieldsToAddOrDelete = Prefix + "58";
                public const string UnsafeCastingInItemReceiver = Prefix + "59";
                public const string ContentType3Tooltip = Prefix + "60";
                public const string DoNotUseSPContextObjectInDisposedBlock = Prefix + "61";
                public const string SPC050250 = SPCAFPrefix + "62";
                public const string SPC050250_2 = SPCAFPrefix + "62-2";
                public const string SPC020203 = SPCAFPrefix + "63";
                public const string SPC020202 = SPCAFPrefix + "64";
                public const string SPC020206 = SPCAFPrefix + "65";
                public const string SPC055201 = SPCAFPrefix + "66";
                public const string SPC020220 = SPCAFPrefix + "67";
                public const string SPC050236 = SPCAFPrefix + "68";
                public const string SPC010212 = SPCAFPrefix + "69";
                public const string SPC010213 = SPCAFPrefix + "70";
                public const string SPC020204 = SPCAFPrefix + "71";
                public const string SPC040212 = SPCAFPrefix + "72";
                public const string SPC050228 = SPCAFPrefix + "73";
                public const string SPC040213 = SPCAFPrefix + "74";
                public const string SPC056003 = SPCAFPrefix + "75";
                public const string SPC056004 = SPCAFPrefix + "76";
                public const string SPC030203 = SPCAFPrefix + "77";
                public const string SPC020205 = SPCAFPrefix + "78";
                public const string SPC050224 = SPCAFPrefix + "79";
                public const string SPC050225 = SPCAFPrefix + "80";
                public const string SPC050223 = SPCAFPrefix + "81";
                public const string SPC050227 = SPCAFPrefix + "82";
                public const string SPC050230 = SPCAFPrefix + "83";
                public const string SPC050222 = SPCAFPrefix + "84";
                public const string SPC050226 = SPCAFPrefix + "85";
                public const string SPC050231 = SPCAFPrefix + "86";
                public const string SPC050232 = SPCAFPrefix + "87";
                public const string SPC050233 = SPCAFPrefix + "88";
                                                        
            }

            public static class Feature
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.Feature;

                public const string SPSiteFeatureShouldNotBeActivatedFromCode = Prefix + "01";
                // not used: public const string FeatureShouldHaveNotEmptyImageUrl = Prefix + "02";
                // not used: public const string FeatureAlwaysForceInstall = Prefix + "03";
                /* not used: 04 - 06 */
            }

            public static class JavaScriptFile
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.JavaScriptFile;

                public const string AvoidDollarGlobalVariableInJSFile = Prefix + "01";
                public const string AvoidJQueryDocumentReadyInJSFile = Prefix + "02";
            }

            public static class Module
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.Module;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.Module;

                // not used: public const string AvoidAllUsersWebPartInModules = Prefix + "01";
                // not used: public const string AvoidXsltForSharePoint2013 = Prefix + "02";
                // not used: public const string AvoidInfoPathForms = Prefix + "03";
                public const string SPC015303 = SPCAFPrefix + "04";
                public const string SPC015301 = SPCAFPrefix + "05";
            }

            public static class CustomAction
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.CustomAction;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.CustomAction;

                public const string CustomActionURLTypos = Prefix + "01";
                public const string SPC015701 = SPCAFPrefix + "02";
                public const string SPC015702 = SPCAFPrefix + "03";
            }

            public static class FieldType
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.FieldType;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.FieldType;

                public const string SPC052201 = SPCAFPrefix + "01";
                public const string SPC012206 = SPCAFPrefix + "02";
                public const string SPC012201 = SPCAFPrefix + "03";
                public const string SPC012202 = SPCAFPrefix + "04";
                public const string SPC012203 = SPCAFPrefix + "05";
                public const string SPC012212 = SPCAFPrefix + "06";
                public const string SPC012211 = SPCAFPrefix + "07";
            }

            public static class DelegateControl
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.DelegateControl;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.DelegateControl;

                public const string SPC046102 = SPCAFPrefix + "01";
                public const string SPC046101 = SPCAFPrefix + "02";
            }

            public static class CustomActionGroup
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.CustomActionGroup;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.CustomActionGroup;

                public const string SPC045801 = SPCAFPrefix + "01";
                public const string SPC015801 = SPCAFPrefix + "02";
            }

            public static class WebTemplateDefinition
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.WebTemplateDefinition;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.WebTemplateDefinition;

                public const string SPC017711 = SPCAFPrefix + "01";
                public const string SPC017701 = SPCAFPrefix + "02";
                public const string SPC017702 = SPCAFPrefix + "03";
            }

            public static class FieldRef
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.FieldRef;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.FieldRef;

                public const string SPC016501 = SPCAFPrefix + "01";
                public const string SPC016503 = SPCAFPrefix + "02";
                public const string SPC016502 = SPCAFPrefix + "03";
            }

            public static class RemoveFieldRef
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.RemoveFieldRef;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.RemoveFieldRef;

                public const string SPC016601 = SPCAFPrefix + "01";
                public const string SPC016603 = SPCAFPrefix + "02";
                public const string SPC016602 = SPCAFPrefix + "03";
            }

            public static class SiteDefinition
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.SiteDefinition;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.SiteDefinition;

                public const string SPC010701 = SPCAFPrefix + "01";
                public const string SPC010703 = SPCAFPrefix + "02";
                public const string SPC040701 = SPCAFPrefix + "03";
            }

            public static class SiteTemplateAssocation
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.FeatureSiteTemplateAssociation;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.FeatureSiteTemplateAssociation;

                public const string SPC017002 = SPCAFPrefix + "01";
                public const string SPC017001 = SPCAFPrefix + "02";
            }

            public static class Workflow
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.Workflow;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.Workflow;

                public const string SPC016202 = SPCAFPrefix + "01";
                public const string SPC016203 = SPCAFPrefix + "02";
                public const string SPC016201 = SPCAFPrefix + "03";
            }

            public static class Receiver
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.Receiver;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.Receiver;

                public const string SPC016002 = SPCAFPrefix + "01";
                public const string SPC016003 = SPCAFPrefix + "02";
                public const string SPC016001 = SPCAFPrefix + "03";
            }

            public static class ContentTypeBinding
            {
                private const string Prefix = ReSPRulePrefix + ReSPCategory + InspectedSPElement.ContentTypeBinding;
                private const string SPCAFPrefix = SPCAFRulePrefix + SPCAFCategory + InspectedSPElement.ContentTypeBinding;

                public const string SPC015601 = SPCAFPrefix + "01";
            }
        }
    }
}
