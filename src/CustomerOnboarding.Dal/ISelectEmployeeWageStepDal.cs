using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ISelectEmployeeWageStepDal
    {
        public SelectEmployeeWageStepDto Fetch(string tenantId, int id, string employeeId);
        public void Insert(SelectEmployeeWageStepDto dto);
        public void Update(SelectEmployeeWageStepDto dto);
    }
}
