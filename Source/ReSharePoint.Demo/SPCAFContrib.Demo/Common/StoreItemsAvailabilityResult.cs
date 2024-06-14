using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCAFContrib.Demo.Common
{
    public class StoreItemsAvailabilityResult
    {
        public List<StoreItemAvailabilityResult> ItemResults;
        public bool Result
        {
            get
            {
                bool result = true;
                foreach (StoreItemAvailabilityResult itemResult in ItemResults)
                {
                    result &= itemResult.OrderedNumber <= itemResult.StoreNumber;
                }
                return result;
            }
        }

        public StoreItemsAvailabilityResult()
        {
            ItemResults = new List<StoreItemAvailabilityResult>();
        }
    }
}
