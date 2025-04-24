using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployeeWagesStepDal
    {
        public EmployeeWagesStepDto Fetch(string tenantId, int id,string employeeId);
        public void Insert(EmployeeWagesStepDto dto);
        public void Update(EmployeeWagesStepDto dto);
    }
}
