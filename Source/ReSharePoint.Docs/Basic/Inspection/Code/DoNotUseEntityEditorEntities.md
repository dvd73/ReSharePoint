<properties 
	pageTitle="RESP510251: EntityEditor.Entities is used" 
    pageName="resp510251"
    parentPageId="csharp"
/>

###Description
It is not recommended to use the Entities property to get the selected entities, because using this sometimes causes unexpected behavior.

###Resolution
It’s much more reliable to use ResolvedEntities instead.
[TEST.GetPeoplePickerUser]

###Links
- [EntityEditor.Entities property](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.webcontrols.entityeditor.entities.aspx)
- [EntityEditor.ResolvedEntities property](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.webcontrols.entityeditor.resolvedentities.aspx)
