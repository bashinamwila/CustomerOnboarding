using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface ITerm : IBusinessBase
    {
        DateSplitter ContractStartDate { get; set; }
        int? Interval { get; set; }
        DateTime ContractExpiryDate { get; }
        int? ContractDuration { get; set; }
    }
}
