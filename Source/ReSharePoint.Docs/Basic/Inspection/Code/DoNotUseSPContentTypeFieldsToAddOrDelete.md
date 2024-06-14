﻿<properties 
	pageTitle="RESP510258: Inappropriate usage of SPContentType.Fields" 
    pageName="resp510258"
    parentPageId="csharp"
/>

###Description
SPContentType contains two collections, Fields (of type SPFieldCollection) and FieldLinks (of type SPFieldLinkCollection). Even if the object model appears to support addition or deletion of fields using the Fields collection, an exception is thrown if you try to do so.
Lists and Webs contain the actual fields with field data. A content type, on the other hand, only holds Field Reference, which simply points at the corresponding field in the list or web. This gets a bit confusing, because content types have both an SPFieldLinkCollection and an SPFieldCollection.

Inappropriate usage:
[TEST.InappropriateSPContentTypeFieldsUsage]

Exception: This functionality is unavailable for field collections not associated with a list.

Just to remember: *Content Type is the definition for a list, but it does not contain any data*. 
Since Fields contain data, Content Types (disassociated from a list) do not have Fields, they have FieldRefs.

###Resolution
Add field reference to Content Type:
[TEST.AppropriateAddingFields]

Delete field reference from Content Type:
[TEST.AppropriateDeletingFields]

###Links
- [SPContentType.Fields property](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spcontenttype.fields.aspx)
- [SPContentType.FieldLinks property](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spcontenttype.fieldlinks.aspx)
- [Fields and Field References](https://msdn.microsoft.com/en-us/library/aa543680(v=office.14).aspx)