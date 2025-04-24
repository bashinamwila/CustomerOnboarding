using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddEmployeeWageConfirmationStepDal
    {
        public AddEmployeeWageConfirmationStepDto Fetch(string tenantId, int id,string employeeId,string wageId);
        public void Insert(AddEmployeeWageConfirmationStepDto dto);
        public void Update(AddEmployeeWageConfirmationStepDto dto);
    }
}
