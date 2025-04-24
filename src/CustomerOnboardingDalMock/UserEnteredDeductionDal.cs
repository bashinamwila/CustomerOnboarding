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
    public class UserEnteredDeductionDal : IUserEnteredDeductionDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public TypeOfDeductionDto Fetch(string tenantId, string id)
        {
            var result = MockDb.UserEnteredDeductions.Where(r => r.TenantId == tenantId && r.DeductionId == id).
                       Select(r => new TypeOfDeductionDto
                       {
                           Id = r.Id,
                           LastChanged = r.LastChanged
                       }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("UserEnteredDeduction");
            return result;
        }

        public void Insert(TypeOfDeductionDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new UserEnteredDeductionEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                DeductionId = data.DeductionId,
                LastChanged = data.LastChanged
            };
            MockDb.UserEnteredDeductions.Add(newItem);
        }

        public void Update(TypeOfDeductionDto data)
        {
            throw new NotImplementedException();
        }
    }
}
