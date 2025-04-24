using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IGeneralEmployeeDetailsInformationStepDal
    {
        public GeneralEmployeeDetailsInformationStepDto Fetch(string tenantId, int id);
        public void Insert(GeneralEmployeeDetailsInformationStepDto dto);
        public void Update(GeneralEmployeeDetailsInformationStepDto dto);
    }
}
