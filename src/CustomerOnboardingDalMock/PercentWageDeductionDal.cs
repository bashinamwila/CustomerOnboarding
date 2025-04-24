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
    public class PercentWageDeductionDal :
        IPercentWageDeductionDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public TypeOfDeductionDto Fetch(string tenantId, string id)
        {
            var result = MockDb.PercentWageDeductions.Where(r => r.TenantId == tenantId && r.DeductionId == id).
                       Select(r => new TypeOfDeductionDto
                       {
                           Id = r.Id,
                           Value = r.Percent,
                           WageId=r.WageId,
                           LastChanged = r.LastChanged
                       }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("PercentWageDeduction");
            return result;
        }

        public void Insert(TypeOfDeductionDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new PercentWageDeductionEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                DeductionId = data.DeductionId,
                Percent = data.Value!.Value,
                WageId=data.WageId,
                LastChanged = data.LastChanged
            };
            MockDb.PercentWageDeductions.Add(newItem);
        }

        public void Update(TypeOfDeductionDto data)
        {
            var result = MockDb.PercentWageDeductions.
               Where(r => r.TenantId == data.TenantId && r.DeductionId == data.DeductionId)
                      .Select(r => r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("PercentWageDeduction");
            if (!result.LastChanged.Matches(data.LastChanged))
                throw new ConcurrencyException("PercentWageDeduction");
            data.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = data.LastChanged;
            result.Percent = data.Value!.Value;
            result.WageId = data.WageId;
        }
    }
}
