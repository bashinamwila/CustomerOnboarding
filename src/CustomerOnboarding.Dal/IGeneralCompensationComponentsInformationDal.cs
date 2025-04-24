using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IGeneralCompensationComponentsInformationDal
    {
        public void Insert(GeneralCompensationComponentsInformationDto dto);
        public void Update(GeneralCompensationComponentsInformationDto dto);

        public GeneralCompensationComponentsInformationDto Fetch(string tenantId, int id);
    }
}
