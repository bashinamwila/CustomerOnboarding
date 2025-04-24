using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IGeneralComplianceInformationDal 
    {
        public void Insert(GeneralComplianceInformationDto dto);
        public void Update(GeneralComplianceInformationDto dto);

        public GeneralComplianceInformationDto Fetch(string tenantId, int id);
    }
}
