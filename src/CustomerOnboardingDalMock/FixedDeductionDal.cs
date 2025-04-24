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
    public class FixedDeductionDal :
        IFixedDeductionDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public TypeOfDeductionDto Fetch(string tenantId, string id)
        {
            var result = MockDb.FixedDeductions.Where(r => r.TenantId == tenantId && r.DeductionId == id).
                        Select(r => new TypeOfDeductionDto
                        {
                            Id = r.Id,
                            Value = r.Amount,
                            LastChanged = r.LastChanged
                        }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("FixedDeduction");
            return result;

        }

        public void Insert(TypeOfDeductionDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new FixedDeductionEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                DeductionId = data.DeductionId,
                Amount = data.Value!.Value,
                LastChanged = data.LastChanged
            };
            MockDb.FixedDeductions.Add(newItem);
        }

        public void Update(TypeOfDeductionDto data)
        {
            var result = MockDb.FixedDeductions.Where(r => r.TenantId == data.TenantId && r.DeductionId == data.DeductionId)
                        .Select(r => r).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("FixedDeduction");
            if (!result.LastChanged.Matches(data.LastChanged))
                throw new ConcurrencyException("FixedDeduction");
            data.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = data.LastChanged;
            result.Amount= data.Value!.Value;

        }
    }
}
