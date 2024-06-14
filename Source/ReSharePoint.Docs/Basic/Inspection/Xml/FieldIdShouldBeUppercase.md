﻿<properties 
	pageTitle="RESP515105: Incorrect 'ID' attr name" 
    pageName="resp515105"
    parentPageId="xml"
/>

###Description
List scoped field MUST HAVE "ID" (not "Id") attribute. SharePoint generates random GUID for these fields.

###Resolution
Please update ID attribute. Make it upper-case.

###Links
- [Field Element](http://msdn.microsoft.com/en-us/library/office/aa979575.aspx)