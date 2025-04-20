using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IGeneralEmployeesInformationDal
    {
        public void Insert(GeneralEmployeesInformationDto dto);
        public void Update(GeneralEmployeesInformationDto dto);
        public GeneralEmployeesInformationDto Fetch(string tenantId, int id);
    }
}
