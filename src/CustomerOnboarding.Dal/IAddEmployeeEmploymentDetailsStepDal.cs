using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddEmployeeEmploymentDetailsStepDal
    {
        public void Insert(AddEmployeeEmploymentDetailsStepDto dto);
        public void Update(AddEmployeeEmploymentDetailsStepDto dto);
        public AddEmployeeEmploymentDetailsStepDto Fetch(string tenantId, int id);
    }
}
