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
    public class PercentGrossEmployerExpenseDal :
        IPercentGrossEmployerExpenseDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public TypeOfEmployerExpenseDto Fetch(string tenantId, string id)
        {

            var result = MockDb.PercentGrossEmployerExpenses.Where(r => r.TenantId == tenantId && r.EmployerExpenseId == id).
                        Select(r => new TypeOfEmployerExpenseDto
                        {
                            Id = r.Id,
                            Value = r.Percent,
                            LastChanged = r.LastChanged
                        }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("PercentGrossEmployerExpense");
            return result;
        }

        public void Insert(TypeOfEmployerExpenseDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new PercentGrossEmployerExpenseEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployerExpenseId = data.EmployerExpenseId,
                Percent = data.Value!.Value,
                LastChanged = data.LastChanged
            };
            MockDb.PercentGrossEmployerExpenses.Add(newItem);
        }

        public void Update(TypeOfEmployerExpenseDto data)
        {
            var result = MockDb.PercentGrossEmployerExpenses.
                Where(r => r.TenantId == data.TenantId && r.EmployerExpenseId == data.EmployerExpenseId)
                       .Select(r => r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("PercentGrossEmployerExpense");
            if (!result.LastChanged.Matches(data.LastChanged))
                throw new ConcurrencyException("PercentGrossEmployerExpense");
            data.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = data.LastChanged;
            result.Percent = data.Value!.Value;
        }
    }
}
