<properties 
	pageTitle="RESP510222: SPContext.Current is used outside web context" 
    pageName="resp510222"
    parentPageId="csharp"
/>

###Description
Avoid using SPContext.Current outside of web request context.

###Resolution
Pass SPWeb/SPSite object as parameter of method.

###Links
- [SPContext.Current Property](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.spcontext.current(v=office.14).aspx)