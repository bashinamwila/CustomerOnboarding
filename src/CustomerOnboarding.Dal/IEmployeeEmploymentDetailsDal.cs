using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployeeEmploymentDetailsDal
    {
        public void Insert(EmployeeEmploymentDetailsDto dto);
        public void Update(EmployeeEmploymentDetailsDto dto);
        public EmployeeEmploymentDetailsDto Fetch(string tenantId, string employeeId);
    }
}
