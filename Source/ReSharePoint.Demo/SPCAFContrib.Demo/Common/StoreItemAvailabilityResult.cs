using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCAFContrib.Demo.Common
{
    public class StoreItemAvailabilityResult
    {
        public string Title;
        public int OrderedNumber;
        public int StoreNumber;
        public bool Result;

        public StoreItemAvailabilityResult()
        {
            Result = true;
        }
    }
}
