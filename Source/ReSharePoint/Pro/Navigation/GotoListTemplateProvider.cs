using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Navigation.Goto.Misc;
using JetBrains.ReSharper.Feature.Services.Navigation.Goto.ProvidersAPI;
using JetBrains.ReSharper.Feature.Services.Occurrences;
using JetBrains.Text;
using JetBrains.UI.Utils;
using JetBrains.Application.UI.Utils;

namespace ReSharePoint.Pro.Navigation
{
    public class GotoListTemplateProvider : IOccurrenceNavigationProvider 
    {
        public bool IsApplicable(INavigationScope scope, GotoContext gotoContext, IIdentifierMatcher matcher)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MatchingInfo> FindMatchingInfos(IIdentifierMatcher matcher, INavigationScope scope, GotoContext gotoContext)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOccurrence> GetOccurrencesByMatchingInfo(MatchingInfo navigationInfo, INavigationScope scope, GotoContext gotoContext)
        {
            throw new NotImplementedException();
        }
    }
}
