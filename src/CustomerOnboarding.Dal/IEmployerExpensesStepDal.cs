using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployerExpensesStepDal
    {
        public void Insert(EmployerExpensesStepDto dto);
        public void Update(EmployerExpensesStepDto dto);
        public EmployerExpensesStepDto Fetch(string tenantId, int id);
    }
}
