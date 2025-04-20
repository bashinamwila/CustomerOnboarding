using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployeePersonalDetailsDal
    {
        public void Insert(EmployeePersonalDetailsDto dto);
        public void Update(EmployeePersonalDetailsDto dto);
        public EmployeePersonalDetailsDto Fetch(string tenantId, string employeeId);
    }
}
