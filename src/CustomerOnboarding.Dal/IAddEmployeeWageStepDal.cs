using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddEmployeeWageStepDal
    {
        AddEmployeeWageStepDto Fetch(string tenantId, int id,string employeeId,string wageId);
        void Insert(AddEmployeeWageStepDto dto);
        void Update(AddEmployeeWageStepDto dto);
    }
}
