using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IPercentGrossDeduction : IDeductionType
    {
        decimal Percent { get; set; }
    }
}
