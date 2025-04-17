using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IPercentWageDeduction : IDeductionType
    {
        decimal Percent { get; set; }
        string WageId { get; set; }
    }
}
