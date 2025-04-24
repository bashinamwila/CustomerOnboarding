using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddCompensationComponentConfirmationStepDal
    {
        public void Insert(AddCompensationComponentConfirmationStepDto dto);
        public void Update(AddCompensationComponentConfirmationStepDto dto);

        public AddCompensationComponentConfirmationStepDto Fetch(string tenantId, int id);
    }
}
