using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddDeductionConfirmationStepDal
    {
        public void Insert(AddDeductionConfirmationStepDto dto);
        public void Update(AddDeductionConfirmationStepDto dto);

        public AddDeductionConfirmationStepDto Fetch(string tenantId, int id);
    }
}

