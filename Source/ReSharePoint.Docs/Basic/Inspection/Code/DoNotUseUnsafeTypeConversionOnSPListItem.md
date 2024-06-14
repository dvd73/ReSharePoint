<properties 
	pageTitle="RESP510213: Unsafe cast on SPListItem" 
    pageName="resp510213"
    parentPageId="csharp"
/>

###Description
SPListItem is untyped entity, so null reference exceptions on nullable types or wrong type conversion exception might arise.

Incorrect usage:
[TEST.IncorrectSPListItemCast]


###Resolution
Consider Convert.ToXXX method or manual conversion to handle wrong/nullable types.

[TEST.CorrectSPListItemCast]

###Links
- [Convert Methods](https://msdn.microsoft.com/en-us/library/system.convert_methods(v=vs.110).aspx)
