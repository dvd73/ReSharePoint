using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Xml.Stages.Analysis;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.XmlAnalysis
{
    [XmlAnalysisProvider]
    public class SPXmlSyntaxAnalysisProvider : IXmlAnalysisProvider
    {
        public bool OnlyPrimary => true;

        /// <summary>
        /// Check to see if it’s a file you’re interested in and add just the analyses you need
        /// </summary>
        /// <param name="file"></param>
        /// <param name="process"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IEnumerable<JetBrains.ReSharper.Daemon.Xml.Stages.XmlAnalysis> GetAnalyses(IXmlFile file, IDaemonProcess process, IContextBoundSettingsStore settings)
        {
            if (file is IDTDFile || file.GetSourceFile().HasExcluded(settings))
                return EmptyList<JetBrains.ReSharper.Daemon.Xml.Stages.XmlAnalysis>.InstanceList;

            return new JetBrains.ReSharper.Daemon.Xml.Stages.XmlAnalysis[]
            {
                new SPElementsFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new DeployContentTypesCorrectly(),
                    new DeployFieldsCorrectly(),
                    new ConsiderHiddenListTemplates(),
                    new ConsiderOverwriteAttributeForContentType(),
                    new ConsiderOverwriteAttributeForField(),
                    new DeployTaxonomyFieldsCorrectly(),
                    new DoNotAllowDeletionForField(),
                    new DoNotAllowDeletionForList(),
                    new DoNotDefineMultipleContentTypeGroupInOneElementFile(),
                    new DoNotDefineMultipleFieldGroupInOneElementFile(),
                    new DoNotUseSystemListNames(),
                    new WebPartModuleDefinitionMightbeImproved(),
                    new NameWithPictureForUserField(),
                    new UniqueListInstanceUrl(),
                    new DeclareRequiredAttributesInListInstance(),
                    new DoNotDefineDuplicateContentTypeID(),
                    new DefineUniqueFieldId(),
                    new UniqueFieldName(),
                    new UniqueFieldStaticName(),
                    new AddCurlyBracketsToFieldId(),
                    new DefineFieldIdAsGUID(),
                    new DeclareAllRequiredAttributesInFields(),
                    new DefineDisplayNameInFieldWithMax255Characters(),
                    new DoNotUseDeprecatedAttributeFromBaseTypeInField(),
                    new DeclareAllRecommendedAttributesInFields(),
                    new DoNotDefineContentTypeIDWithMoreThan1024Characters(),
                    new DeclareRequiredAttributesInContentType(),
                    new DoNotDefineContentTypeNameWithMoreThan124Characters(),
                    new DoNotDeclareInternalAttributesInContentType(),
                    new DoNotDeclareObsoleteAttributesInContentType(),
                    new DefineCorrectParentFeatureId(),
                    new DefineInheritsInContentTypeWithRemoveFiedRef(),
                    new DefineInheritsToFalseInContentTypeWithForms(),
                    new DeclareFileElementInModule(),
                    new DeclareRequiredAttributesInListInstance(), 
                    new DeployMissingListTemplateForListInstance(),
                    new RemoveCurlyBracketsFromFeatureIdInListInstance(),
                    new DeclareRequiredAttributesInListTemplate(),
                    new DoNotDefineListNameWithSpaces(),
                    new DefineUniqueListTemplateType(),
                    new DeclareRequiredAttributesInCustomAction(),
                    new DefineUniqueIDInCustomAction(),
                    new DeclareRequiredAttributesInCustomActionGroup(),
                    new DeclareScopeForReceiversInSiteFeatures(),
                    new DoNotDefineEmailEventReceiverInSiteCollectionLevel(),
                    new DeclareRequiredAttributesInWorkflow(),
                    new DeclareRequiredAttributesInFieldRef(),
                    new DefineValidIDInFieldRef(),
                    new DefineFieldRefInCorrectCasing(),
                    new DeclareRequiredAttributesInRemoveFieldRef(),
                    new NotDeclareIgnoredAttributesInRemoveFieldRef(),
                    new DefineValidIDInRemoveFiedRef(),
                    new DoNotDefineTemplateAssocationInFeatureWithWrongScope(),
                    new DeclareRequiredAttributesInTemplateAssociation(),
                    new DeclareRequiredAttributesInWebTemplate(),
                    new DefineValidParentWebTemplate(),
                    new DeclareRecommendedAttributesInWebTemplate(),
                    new RemoveSpacesFromResourceKey(),
                    new DefineBooleanAttributesInUpperCase(),
                    new DoNotDefineReceiverInFeatureWithWrongScope(),
                    new DoNotDefineWorkflowInFeatureWithWrongScope(), 
                    new DoNotDefineListInstanceInFeatureWithWrongScope(),
                    new DoNotDefineListTemplateInFeatureWithWrongScope(),
                    new DoNotDefineFieldInFeatureWithWrongScope(),
                    new DoNotDefineContentTypeInFeatureWithWrongScope(),
                    new DoNotDefineContentTypeBindingInFeatureWithWrongScope(),
                    new DoNotDefineModuleInFeatureWithWrongScope(),
                    new DoNotSetAllowEveryoneViewItemsToTrue(),
                    new DeclareAttributeInheritsInContentType(),
                    new DeclareContentTypeIDUpperCase(),
                    new DeclareTemplateTypeForExternalList(),
                    new DeclareDescriptionInCustomActionGroup(),
                    new DeclareAttributeSequenceInControl(),
                    new DeclareAttributeIdInControl(),
                    new DefineListTemplateTypeGreaterThan10000(),
                    new DoNotUseUnderscoreInFieldName(),  
                    new FieldNameLengthLimitExceed(),
                    new AvoidCommentsForContentType(),
                    new DoNotIndexNoteField(),
                    new DifferentInternalAndStaticFieldNames(),
                    new WrongFeatureIdInListInstance(),
                    new UniqueListInstanceTitle(),
                    new DoNotProvisionLookupFieldBeforeRelatedList(),
                    new NotProvisionedEntity(),
                    new MixedIDInFieldName()
                }),
                new SPListSchemaFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new FieldIdShouldBeUppercase(),
                    new AvoidListContentTypes(), 
                    new DeclareEmptyFieldsElement(),
                    new DoNotDeployTaxonomyFieldsInListDefinition(),
                    new EnsureFolderContentTypeInListDefinition(),
                    new NameWithPictureForUserField(),
                    new AddCurlyBracketsToFieldId(),
                    new DefineFieldIdAsGUID(),
                    new DeclareAllRequiredAttributesInFields(),
                    new DefineDisplayNameInFieldWithMax255Characters(),
                    new DoNotUseDeprecatedAttributeFromBaseTypeInField(),
                    new DeclareAllRecommendedAttributesInFields(),
                    new DefineValidIDInFieldRef(),
                    new DefineFieldRefInCorrectCasing(),
                    new DeclareDefaultDescriptionInListDefinition(),
                    new DeclareRequiredAttributesInListDefinition(),
                    new RemoveSpacesFromResourceKey(),
                    new DefineBooleanAttributesInUpperCase(),
                    new FieldNameLengthLimitExceed(),
                    new DoNotIndexNoteField(),
                    new DifferentInternalAndStaticFieldNames(),
                    new RepeatCalculatedFieldsInListSchema(),
                    new RepeatLookupFieldsInListSchema() 
                }),
                new SPWebPartFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new WebPartDefinitionMightBeImproved(),
                    new DeclareTitleInWebPartDefinition(),
                    new RemoveSpacesFromResourceKey(),
                    new DeclareTitleAndDescriptionInWebPart() 
                }),
                new OnetFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new DeclareRequiredAttributesInSiteDefinition(),
                    new RemoveSpacesFromResourceKey(),
                    new DefineBooleanAttributesInUpperCase()
                }),
                new WebTempFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new DoNotAddDependencyToWebAppFeatureInSiteDefinition(),
                    new DefineBooleanAttributesInUpperCase(),
                    new TemplateConfigurationShouldNotBeVisibleInWholeFarm()
                }),
                new FldTypesFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new DeclareAllRecommendedAttributesInFields(),
                    new DoNotDeclareInternalTypeInFieldTypes(),
                    new DoNotDeclareSQLTypeInFieldTypes(),
                    new DeclareFieldTypeClassInFieldType(),
                    new DoNotUseRenderPatternInFieldTypes(),
                    new DoNotUsePropertySchemaInFieldTypes(),
                    new RemoveSpacesFromResourceKey(),
                    new CustomFieldTypesShouldNotBeUserCreatable()
                })
            };
        }
    }
}
