using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ITemporalEmployeeDal
    {
        void Insert(TemporalEmployeeDto data);
        void Update(TemporalEmployeeDto data);
        TemporalEmployeeDto Fetch(string tenantId, string employeeId);
        void Delete(string tenantId,string employeeId);
    }
}
