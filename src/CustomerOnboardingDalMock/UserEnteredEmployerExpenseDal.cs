using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class UserEnteredEmployerExpenseDal : IUserEnteredEmployerExpenseDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public TypeOfEmployerExpenseDto Fetch(string tenantId, string id)
        {
            var result = MockDb.UserEnteredEmployerExpenses.Where(r => r.TenantId == tenantId && r.EmployerExpenseId == id).
                       Select(r => new TypeOfEmployerExpenseDto
                       {
                           Id = r.Id,
                           LastChanged = r.LastChanged
                       }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("UserEnteredEmployerExpense");
            return result;
        }

        public void Insert(TypeOfEmployerExpenseDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new UserEnteredEmployerExpenseEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployerExpenseId = data.EmployerExpenseId,
                LastChanged = data.LastChanged
            };
            MockDb.UserEnteredEmployerExpenses.Add(newItem);
        }

        public void Update(TypeOfEmployerExpenseDto data)
        {
            throw new NotImplementedException();
        }
    }
}

