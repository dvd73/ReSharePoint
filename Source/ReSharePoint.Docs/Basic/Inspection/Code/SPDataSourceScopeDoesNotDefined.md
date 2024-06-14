﻿<properties 
	pageTitle="RESP510218: SPDataSource.Scope is missed" 
    pageName="resp510218"
    parentPageId="csharp"
/>

###Description
How do we know what people want – folder, inside folder, or just items?
SPQuery has an “affected scope”. It can be one of the follow: 

[table]

Member name, Description
Default, Show only the files and subfolders of a specific folder.
Recursive, Show all files of all folders.
RecursiveAll, Show all files and all subfolders of all folders.
FilesOnly, Show only the files of a specific folder.

[/table]

All enumeration values are covered all possible developer’s intentions. Other words, without specified, SharePoint will use Default value. It is not always correspond develepers needs now or later espetially in case of new folder added. Notify about missing Scope we give developer change to specify it’s architecture approach.

[TEST.SPDataSourceScopeUsageSamples]

###Resolution
Specify Scope property.

###Links
- [SPViewScope enumeration](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spviewscope.aspx)
- [SPDataSource.Scope property](https://msdn.microsoft.com/EN-US/library/microsoft.sharepoint.webcontrols.spdatasource.scope.aspx)