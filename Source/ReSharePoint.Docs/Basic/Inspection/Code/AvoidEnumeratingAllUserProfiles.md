<properties 
	pageTitle="RESP510204: UserProfileManager.GetEnumerator() is used" 
    pageName="resp510204"
    parentPageId="csharp"
/>

###Description
UserProfileManager.GetEnumerator() should not be used.
It would enumerate all user profiles, so that it could have a performance impact.

###Resolution
Consider utilizing search to get user profiles you are interested in. 
Then retrieve needed user profiles as you need.

###Links
- [UserProfileManager Class](http://msdn.microsoft.com/en-us/library/microsoft.office.server.userprofiles.userprofilemanager(v=office.14).aspx)