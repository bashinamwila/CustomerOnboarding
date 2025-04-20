using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployeesStepDal
    {
        public void Insert(EmployeesStepDto dto);
        public void Update(EmployeesStepDto dto);
        public EmployeesStepDto Fetch(string tenantId, int id);
    }
}
