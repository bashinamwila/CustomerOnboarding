using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddEmployerExpenseConfirmationStepDal
    {
        public void Insert(AddEmployerExpenseConfirmationStepDto dto);
        public void Update(AddEmployerExpenseConfirmationStepDto dto);

        public AddEmployerExpenseConfirmationStepDto Fetch(string tenantId, int id);
    }
}


