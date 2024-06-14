<properties 
	pageTitle="RESP510226: SPListItem.File is used" 
    pageName="resp510226"
    parentPageId="csharp"
/>

###Description
For non document library it returns null.

###Resolution
Check that you work with document library. Use SPWeb.GetFile(SPListItem.UniqueId) instead.
[TEST.AppropriateSPListItemSPFileUsage]

###Links
- [SPListItem.File property](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.splistitem.file.aspx)