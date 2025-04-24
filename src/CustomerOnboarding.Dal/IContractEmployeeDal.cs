using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IContractEmployeeDal
    {
        void Insert(ContractEmployeeDto data);
        void Update(ContractEmployeeDto data);
        ContractEmployeeDto Fetch(string tenantId, string employeeId);
        void Delete(string tenantId,string employeeId);
    }
}
