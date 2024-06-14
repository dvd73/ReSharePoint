<properties 
	pageTitle="RESP510201: Thread.Sleep() usage" 
    pageName="resp510201"
    parentPageId="csharp"
/>

###Description
Avoid using Thread.Sleep() within SharePoint solutions. 
It leads to potential performance issues, and potentially indicates misunderstanding of SharePoint API or lack of overall solution design/architecture.

###Resolution
Consider reviewing and refactor the code which uses Thread.Sleep() method.

###Links
No links are provided.