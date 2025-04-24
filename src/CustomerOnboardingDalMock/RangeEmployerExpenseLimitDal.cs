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
    public class RangeEmployerExpenseLimitDal :
        IRangeEmployerExpenseLimitDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public LimitDto Fetch(string tenantId, string id)
        {
            var result = MockDb.RangeEmployerExpenseLimits.Where(r => r.TenantId == tenantId && r.EmployerExpenseId == id).
                     Select(r => new LimitDto
                     {
                         Id = r.Id,
                         Minimum = r.Minimum,
                         Maximum = r.Maximum,
                         LastChanged = r.LastChanged
                     }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("RangeEmployerExpenseLimit");
            return result;
        }

        public void Insert(LimitDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new RangeEmployerExpenseLimitEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployerExpenseId = data.ItemId,
                Maximum = data.Maximum!.Value,
                Minimum = data.Minimum!.Value,
                LastChanged = data.LastChanged
            };
            MockDb.RangeEmployerExpenseLimits.Add(newItem);
        }

        public void Update(LimitDto data)
        {
            var result = MockDb.RangeEmployerExpenseLimits.Where(r => r.TenantId == data.TenantId && r.EmployerExpenseId == data.ItemId)
                       .Select(r => r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("RangeEmployerExpenseLimit");
            if (!result.LastChanged.Matches(data.LastChanged))
                throw new ConcurrencyException("RangeEmployerExpenseLimit");

            data.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = data.LastChanged;
            result.Minimum = data.Minimum!.Value;
            result.Maximum = data.Maximum!.Value;

        }
    }
}

