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
    public class FixedEmployerExpenseDal :
        IFixedEmployerExpenseDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public TypeOfEmployerExpenseDto Fetch(string tenantId, string id)
        {
            var result = MockDb.FixedEmployerExpenses.Where(r => r.TenantId == tenantId && r.EmployerExpenseId == id).
                        Select(r => new TypeOfEmployerExpenseDto
                        {
                            Id = r.Id,
                            Value = r.Amount,
                            LastChanged = r.LastChanged
                        }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("FixedEmployerExpense");
            return result;

        }

        public void Insert(TypeOfEmployerExpenseDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new FixedEmployerExpenseEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployerExpenseId = data.EmployerExpenseId,
                Amount = data.Value!.Value,
                LastChanged = data.LastChanged
            };
            MockDb.FixedEmployerExpenses.Add(newItem);
        }

        public void Update(TypeOfEmployerExpenseDto data)
        {
            var result = MockDb.FixedEmployerExpenses.Where(r => r.TenantId == data.TenantId && r.EmployerExpenseId == data.EmployerExpenseId)
                        .Select(r => r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("FixedEmployerExpense");
            if (!result.LastChanged.Matches(data.LastChanged))
                throw new ConcurrencyException("FixedEmployerExpense");
            data.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = data.LastChanged;
            result.Amount = data.Value!.Value;

        }
    }
}

