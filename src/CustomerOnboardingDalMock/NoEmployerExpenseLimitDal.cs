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
    public class NoEmployerExpenseLimitDal :
        INoEmployerExpenseLimitDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public LimitDto Fetch(string tenantId, string id)
        {
            var result = MockDb.NoEmployerExpenseLimits.Where(r => r.TenantId == tenantId && r.EmployerExpenseId == id).
                      Select(r => new LimitDto
                      {
                          Id = r.Id,
                          LastChanged = r.LastChanged
                      }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("NoEmployerExpenseLimit");
            return result;
        }

        public void Insert(LimitDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new NoEmployerExpenseLimitEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployerExpenseId = data.ItemId,
                LastChanged = data.LastChanged
            };
            MockDb.NoEmployerExpenseLimits.Add(newItem);
        }

        public void Update(LimitDto data)
        {
            throw new NotImplementedException();
        }
    }
}
