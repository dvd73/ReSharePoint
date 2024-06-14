using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public class SPPublishingField
        {
            public string Title { get; set; }
            public Guid Id { get; set; }
        }

        public static List<SPPublishingField> SPPublishingFields = new List<SPPublishingField>()
        {
            new SPPublishingField {Title = "Description", Id = new Guid("9da97a8a-1da5-4a77-98d3-4bc10456e700")},
            new SPPublishingField {Title = "StartDate", Id = new Guid("51d39414-03dc-4bd0-b777-d3e20cb350f7")},
            new SPPublishingField {Title = "ExpiryDate", Id = new Guid("a990e64f-faa3-49c1-aafa-885fda79de62")},
            new SPPublishingField {Title = "LocalContactEmail", Id = new Guid("c79dba91-e60b-400e-973d-c6d06f192720")},
            new SPPublishingField {Title = "LocalContactName", Id = new Guid("7546ad0d-6c33-4501-b470-fb3003ca14ba")},
            new SPPublishingField {Title = "LocalContactImage", Id = new Guid("dc47d55f-9bf9-494a-8d5b-e619214dd19a")},
            new SPPublishingField {Title = "PageLayout", Id = new Guid("0f800910-b30d-4c8f-b011-8189b2297094")},
            new SPPublishingField
            {
                Title = "PublishingPageContent",
                Id = new Guid("f55c4d88-1f2e-4ad9-aaa8-819af4ee7ee8")
            },
            new SPPublishingField {Title = "PublishingPageImage", Id = new Guid("3de94b06-4120-41a5-b907-88773e493458")},
            new SPPublishingField
            {
                Title = "HeaderStylesDefinitions",
                Id = new Guid("a932ec3f-94c1-48b1-b6dc-41aaa6eb7e54")
            },
            new SPPublishingField {Title = "Contact", Id = new Guid("aea1a4dd-0f19-417d-8721-95a1d28762ab")},
            new SPPublishingField {Title = "PreviewImage", Id = new Guid("188ce56c-61e0-4d2a-9d3e-7561390668f7")},
            new SPPublishingField {Title = "Hidden", Id = new Guid("7581e709-5d87-42e7-9fe6-698ef5e86dd3")},
            new SPPublishingField
            {
                Title = "AuthenticatedCacheProfile",
                Id = new Guid("9a36d6c6-f7d4-4cce-8923-ad99a44e2f5b")
            },
            new SPPublishingField
            {
                Title = "AnonymousCacheProfile",
                Id = new Guid("bd51bbe5-9a06-4195-b385-e04fe47a33c8")
            },
            new SPPublishingField {Title = "MigratedGuid", Id = new Guid("75bed596-0661-4edd-9724-1d607ab8d3b5")},
            new SPPublishingField
            {
                Title = "AssociatedContentType",
                Id = new Guid("b510aac1-bba3-4652-ab70-2d756c29540f")
            },
            new SPPublishingField
            {
                Title = "AssociatedVariations",
                Id = new Guid("d211d750-4fe6-4d92-90e8-eb16dff196c8")
            },
            new SPPublishingField {Title = "CacheDisplayName", Id = new Guid("983f490b-fc53-4820-9354-e8de646b4b82")},
            new SPPublishingField {Title = "CacheCacheability", Id = new Guid("18f165be-6285-4a57-b3ab-4e9f913d299f")},
            new SPPublishingField
            {
                Title = "CachePerformAclCheck",
                Id = new Guid("db03cb99-cf1e-40b8-adc7-913f7181dac3")
            },
            new SPPublishingField
            {
                Title = "CacheCheckForChanges",
                Id = new Guid("5b4d927c-d383-496b-bc79-1e61bd383019")
            },
            new SPPublishingField {Title = "CacheVaryByCustom", Id = new Guid("4689a812-320e-4623-aab9-10ad68941126")},
            new SPPublishingField {Title = "CacheEnabled", Id = new Guid("d8f18167-7cff-4c4e-bdbe-e7b0f01678f3")},
            new SPPublishingField {Title = "CacheDuration", Id = new Guid("bdd1b3c3-18db-4acf-a963-e70ef4227fbc")},
            new SPPublishingField {Title = "CacheVaryByHeader", Id = new Guid("89587dfd-b9ca-4fae-8eb9-ba779e917d48")},
            new SPPublishingField {Title = "CacheVaryByParam", Id = new Guid("b8abfc64-c2bd-4c88-8cef-b040c1b9d8c0")},
            new SPPublishingField {Title = "CacheVaryByRights", Id = new Guid("d4a6af1d-c6d7-4045-8def-cefa25b9ec30")},
            new SPPublishingField {Title = "CacheAllowWriters", Id = new Guid("773ed051-58db-4ff2-879b-08b21ab001e0")},
            new SPPublishingField
            {
                Title = "CacheDisplayDescription",
                Id = new Guid("9550e77a-4d10-464f-bc0c-102d5b1aec42")
            },
            new SPPublishingField
            {
                Title = "CacheAuthenticatedUse",
                Id = new Guid("0a90b5e8-185a-4dec-bf3c-e60aae08373f")
            },
            new SPPublishingField {Title = "ContentType", Id = new Guid("c042a256-787d-4a6f-8a8a-cf6ab767f12d")},
            new SPPublishingField {Title = "ContentTypeId", Id = new Guid("03e45e84-1992-4d42-9116-26f756012634")},
            new SPPublishingField {Title = "CreatedBy", Id = new Guid("1df5e554-ec7e-46a6-901d-d85a3881cb18")},
            new SPPublishingField {Title = "CreatedDate", Id = new Guid("8c06beca-0777-48f7-91c7-6da68bc07b69")},
            new SPPublishingField {Title = "LastModifiedBy", Id = new Guid("d31655d1-1d5b-4511-95a1-7a09e9b75bf2")},
            new SPPublishingField {Title = "LastModifiedDate", Id = new Guid("28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f")},
            new SPPublishingField {Title = "Title", Id = new Guid("fa564e0f-0c70-4ab9-b863-0177e6ddd247")},
            new SPPublishingField {Title = "VariationGroupId", Id = new Guid("914fdb80-7d4f-4500-bf4c-ce46ad7484a4")},
            new SPPublishingField
            {
                Title = "VariationRelationshipLink",
                Id = new Guid("766da693-38e5-4b1b-997f-e830b6dfcc7b")
            },
            new SPPublishingField {Title = "ReusableTextType", Id = new Guid("3a4b7f98-8d14-4800-8bf5-9ad1dd6a82ee")},
            new SPPublishingField {Title = "ReusableText", Id = new Guid("890e9d41-5a0e-4988-87bf-0fb9d80f60df")},
            new SPPublishingField {Title = "ReusableHtml", Id = new Guid("82dd22bf-433e-4260-b26e-5b8360dd9105")},
            new SPPublishingField {Title = "AutomaticUpdate", Id = new Guid("e977ed93-da24-4fcc-b77d-ac34eea7288f")},
            new SPPublishingField {Title = "RollupImage", Id = new Guid("543bc2cf-1f30-488e-8f25-6fe3b689d9ac")},
            new SPPublishingField {Title = "NotificationListUrl", Id = new Guid("789a7435-9c89-4d65-a472-d386aba0edf4")},
            new SPPublishingField
            {
                Title = "NotificationListDeliveryDate",
                Id = new Guid("e5bd81f1-c408-4abe-84e6-2119d568361e")
            },
            new SPPublishingField {Title = "SummaryIcon", Id = new Guid("897e4d29-edcf-42a4-951c-b6dbd6d1701f")},
            new SPPublishingField {Title = "SummaryImage", Id = new Guid("366ec12a-7e06-48fb-97d3-6d3094b3dcc2")},
            new SPPublishingField {Title = "TargetItemId", Id = new Guid("c546204c-9a88-41d9-913c-ad32cff7dc73")},
            new SPPublishingField {Title = "SummaryAudience", Id = new Guid("5f667e57-afc0-44e9-a33f-292b69060e8a")},
            new SPPublishingField {Title = "SummaryGroup", Id = new Guid("d42659b5-ded6-41da-a941-c66e06f0e574")},
            new SPPublishingField {Title = "SpsDescription", Id = new Guid("631ab9cc-6687-488d-ab1d-a65b364a0d44")},
            new SPPublishingField {Title = "AudienceTargeting", Id = new Guid("61cbb965-1e04-4273-b658-eedaa662f48d")},
            new SPPublishingField {Title = "RedirectURL", Id = new Guid("ac57186e-e90b-4711-a038-b6c6a62a57dc")},
            new SPPublishingField {Title = "PublishingPageIcon", Id = new Guid("3894ec3f-4674-4924-a440-8872bec40cf9")},
            new SPPublishingField {Title = "SummaryLinks", Id = new Guid("b3525efe-59b5-4f0f-b1e4-6e26cb6ef6aa")},
            new SPPublishingField {Title = "SummaryLinks2", Id = new Guid("27761311-936a-40ba-80cd-ca5e7a540a36")},
            new SPPublishingField {Title = "ByLine", Id = new Guid("d3429cc9-adc4-439b-84a8-5679070f84cb")},
            new SPPublishingField {Title = "ArticleDate", Id = new Guid("71316cea-40a0-49f3-8659-f0cefdbdbd4f")},
            new SPPublishingField
            {
                Title = "PublishingImageCaption",
                Id = new Guid("66f500e9-7955-49ab-abb1-663621727d10")
            },
            new SPPublishingField {Title = "ShowInRibbon", Id = new Guid("32e03f99-6949-466a-a4a6-057c21d4b516")},
            new SPPublishingField {Title = "DocumentIdPersistId", Id = new Guid("c010d384-479c-494f-968c-c413dbe3de29")},
            new SPPublishingField {Title = "DocumentId", Id = new Guid("ae3e2a36-125d-45d3-9051-744b513536a6")},
            new SPPublishingField {Title = "DocumentIdUrl", Id = new Guid("3b63724f-3418-461f-868b-7706f69b029c")},
            new SPPublishingField
            {
                Title = "PublishedLinksDisplayName",
                Id = new Guid("c80f535b-a430-4273-8f4f-f3e95507b62a")
            },
            new SPPublishingField {Title = "PublishedLinksUrl", Id = new Guid("70b38565-a310-4546-84a7-709cfdc140cf")},
            new SPPublishingField
            {
                Title = "PublishedLinksDescription",
                Id = new Guid("92bba27e-eef6-41aa-b728-6dd9caf2bde2")
            },
            new SPPublishingField {Title = "HoldRecordStatus", Id = new Guid("3afcc5c7-c6ef-44f8-9479-3561d72f9e8e")},
            new SPPublishingField {Title = "DeclaredRecord", Id = new Guid("f9a44731-84eb-43a4-9973-cd2953ad8646")},
            new SPPublishingField {Title = "IsLocked", Id = new Guid("740931e6-d79e-44a6-a752-a06eb23c11b0")},
            new SPPublishingField {Title = "ExemptFromPolicy", Id = new Guid("b0227f1a-b179-4d45-855b-a18f03706bcb")},
            new SPPublishingField {Title = "ExpirationDate", Id = new Guid("acd16fdf-052f-40f7-bb7e-564c269c9fbc")},
            new SPPublishingField {Title = "ExpirationDateSaved", Id = new Guid("74e6ae8a-0e3e-4dcb-bbff-b5a016d74d64")},
            new SPPublishingField {Title = "IconOverlay", Id = new Guid("b77cdbcf-5dce-4937-85a7-9fc202705c91")},
            new SPPublishingField {Title = "AverageRatings", Id = new Guid("5a14d1ab-1513-48c7-97b3-657a5ba6c742")},
            new SPPublishingField {Title = "RatingsCount", Id = new Guid("b1996002-9167-45e5-a4df-b2c41c6723c7")}
        };

        public static string GetBuiltInPublishingFieldId(Guid value)
        {
            string result = String.Empty;

            var feature = SPPublishingFields.FirstOrDefault(key => key.Id == value);

            if (feature != null)
                result = feature.Title;

            return result;
        }
    }
}
