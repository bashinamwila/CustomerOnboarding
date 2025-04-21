using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IEmployeeWageOrAccrual :IBusinessBase
    {
        string Id { get; }
        string Name { get; }
        IEmployeeEarningOrAccrualType Type { get; }
    }
}
