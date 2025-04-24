using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployerExpenseDal
    {
        void Insert(EmployerExpenseDto item);
        EmployerExpenseDto Fetch(string tenantId, string id);
        void Update(EmployerExpenseDto item);
        void Delete(string tenantId, string id);
        List<EmployerExpenseDto> Fetch(string tenantId);
        bool Exists(string tenantId, string id);


    }
}
