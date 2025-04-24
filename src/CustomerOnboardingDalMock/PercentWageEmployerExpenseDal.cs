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
    public class PercentWageEmployerExpenseDal :
        IPercentWageEmployerExpenseDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public TypeOfEmployerExpenseDto Fetch(string tenantId, string id)
        {
            var result = MockDb.PercentWageEmployerExpenses.Where(r => r.TenantId == tenantId && r.EmployerExpenseId == id).
                       Select(r => new TypeOfEmployerExpenseDto
                       {
                           Id = r.Id,
                           Value = r.Percent,
                           WageId = r.WageId,
                           LastChanged = r.LastChanged
                       }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("PercentWageEmployerExpense");
            return result;
        }

        public void Insert(TypeOfEmployerExpenseDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new PercentWageEmployerExpenseEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployerExpenseId = data.EmployerExpenseId,
                Percent = data.Value!.Value,
                WageId = data.WageId,
                LastChanged = data.LastChanged
            };
            MockDb.PercentWageEmployerExpenses.Add(newItem);
        }

        public void Update(TypeOfEmployerExpenseDto data)
        {
            var result = MockDb.PercentWageEmployerExpenses.
               Where(r => r.TenantId == data.TenantId && r.EmployerExpenseId == data.EmployerExpenseId)
                      .Select(r => r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("PercentWageEmployerExpense");
            if (!result.LastChanged.Matches(data.LastChanged))
                throw new ConcurrencyException("PercentWageEmployerExpense");
            data.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = data.LastChanged;
            result.Percent = data.Value!.Value;
            result.WageId = data.WageId;
        }
    }
}
