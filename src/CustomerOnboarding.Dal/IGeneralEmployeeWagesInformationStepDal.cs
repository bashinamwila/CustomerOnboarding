using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IGeneralEmployeeWagesInformationStepDal
    {
        public GeneralEmployeeWagesInformationStepDto Fetch(string tenantId, int id);
        public void Insert(GeneralEmployeeWagesInformationStepDto dto);
        public void Update(GeneralEmployeeWagesInformationStepDto dto);
    }
}
