
<properties 
	pageTitle="RESP515205: Avoid comments for content types" 
    pageName="resp515205"
    parentPageId="xml"
/>

###Description
Xml comment in elements file can break your content type.
The following content type is incorrect and all fields after the comment won't be created by SharePoint.

[XML.IncorrectContentTypeWithComments]

###Resolution
Remove all comments from content type schema.
The following content type is correct. Just remove comments from XML.

[XML.CorrectContentTypeWithComments]

###Links
- [Content Type Element](http://msdn.microsoft.com/en-us/library/office/aa544268(v=office.14).aspx)
- [Xml Comment in Elements File can Break your Content Type](https://www.1stquad.com/sharepoint-kompetenz-erfahrung-know-how/blog/default/oktober-2012/xml-comment-in-elements-breaks-your-contenttype)