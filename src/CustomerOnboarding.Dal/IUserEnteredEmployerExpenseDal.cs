using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IUserEnteredEmployerExpenseDal
    {
        public TypeOfEmployerExpenseDto Fetch(string tenantId, string id);
        public void Insert(TypeOfEmployerExpenseDto data);
        public void Update(TypeOfEmployerExpenseDto data);
        public void Delete(string tenantId, string id);
    }
}
