<properties 
	pageTitle="RESP510212: DirectorySearcher is used" 
    pageName="resp510212"
    parentPageId="csharp"
/>

###Description
Do not use DirectorySearcher class to query ActiveDirectory.

###Resolution
Consider SPUtility.GetPrincipalsInGroup method to perform necessary queries.

###Links
- [SPUtility.GetPrincipalsInGroup Method](http://msdn.microsoft.com/en-us/library/microsoft.sharepoint.utilities.sputility.getprincipalsingroup(v=office.14).aspx)