using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IGeneralDeductionsInformationDal
    {
        public void Insert(GeneralDeductionsInformationDto dto);
        public void Update(GeneralDeductionsInformationDto dto);

        public GeneralDeductionsInformationDto Fetch(string tenantId, int id);
    }
}
