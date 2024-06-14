<properties 
	pageTitle="RESP515204: Consider Overwrite='TRUE' for content types" 
    pageName="resp515204"
    parentPageId="xml"
/>

###Description
ContentTypes with Overwrite="TRUE" are deployed directly to Content Database and not subject to ghosting issues.

###Note
- The rule does not check content types in List Schema.

###Resolution
Add Overwrite="TRUE" attribute.

###Links
- [ContentType Element](http://msdn.microsoft.com/en-us/library/office/aa544268(v=office.14).aspx)