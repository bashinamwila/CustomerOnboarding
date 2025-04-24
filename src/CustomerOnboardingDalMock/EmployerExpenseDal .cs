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
    public class EmployerExpenseDal : IEmployerExpenseDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public EmployerExpenseDto Fetch(string tenantId, string id)
        {
            var result = MockDb.EmployerExpenses.Where(r => r.TenantId == tenantId && r.Id == id).
                            Select(r => new EmployerExpenseDto
                            {
                                Id = r.Id,
                                Name = r.Name,
                                IsSystemDefined = r.IsSystemDefined,
                                Limit = r.Limit,
                                Type = r.Type,
                            }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("EmployerExpense");
            return result;

        }

        public List<EmployerExpenseDto> Fetch(string tenantId)
        {

            var result = MockDb.EmployerExpenses.Where(r => r.TenantId == tenantId).
                            Select(r => new EmployerExpenseDto
                            {
                                Id = r.Id,
                                Name = r.Name,
                                IsSystemDefined = r.IsSystemDefined,
                                Limit = r.Limit,
                                Type = r.Type,
                            }).ToList();
            return result;

        }

        public void Insert(EmployerExpenseDto item)
        {
            item.LastChanged = MockDb.GetTimeStamp();
            var newItem = new EmployerExpenseEntity
            {
                Id = item.Id,
                TenantId = item.TenantId,
                Name = item.Name,
                IsSystemDefined = item.IsSystemDefined,
                Limit = item.Limit,
                Type = item.Type,
                LastChanged = item.LastChanged,
            };
            MockDb.EmployerExpenses.Add(newItem);

        }

        public void Update(EmployerExpenseDto item)
        {
            var result = MockDb.EmployerExpenses.
                Where(r => r.TenantId == item.TenantId && r.Id == item.Id)
                .Select(r => r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("EmployerExpense");
            if (!result.LastChanged.Matches(item.LastChanged))
                throw new ConcurrencyException("EmployerExpense");
            item.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = item.LastChanged;
            result.Name = item.Name;
            result.Type = item.Type;
            result.Limit = item.Limit;
        }
    }
}
