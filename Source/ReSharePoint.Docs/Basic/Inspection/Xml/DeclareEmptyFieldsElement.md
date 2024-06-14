﻿<properties 
	pageTitle="Declare empty Fields element" 
    pageName="resp515502"
    parentPageId="xml"
/>

###Description
Declare empty Fields element when using only ContentTypeRefs. Fields automatically populated from content types.

[XML.DeclareEmptyFields]

This works well. For most field types, that is. It does not work for Calculated Fields. More about <a title="this" href="http://www.hekstra.org/how-to-deploy-a-calculated-field/" target="_blank">this</a>.</pre>

###Resolution
Add empty Fields node to List shema.

###Links
- [Fields Element](http://msdn.microsoft.com/en-us/library/office/ms451470.aspx)