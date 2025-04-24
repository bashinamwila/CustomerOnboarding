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
    public class DeductionDal : IDeductionDal
    {
        public void Delete(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        public DeductionDto Fetch(string tenantId, string id)
        {
            var result = MockDb.Deductions.Where(r => r.TenantId == tenantId && r.Id == id).
                            Select(r => new DeductionDto
                            {
                                Id = r.Id,
                                Name = r.Name,
                                IsSystemDefined = r.IsSystemDefined,
                                Limit = r.Limit,
                                Type = r.Type,
                            }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("Deduction");
            return result;

        }

        public List<DeductionDto> Fetch(string tenantId)
        {

            var result = MockDb.Deductions.Where(r => r.TenantId == tenantId).
                            Select(r => new DeductionDto
                            {
                                Id = r.Id,
                                Name = r.Name,
                                IsSystemDefined = r.IsSystemDefined,
                                Limit = r.Limit,
                                Type = r.Type,
                            }).ToList();
            return result;

        }

        public void Insert(DeductionDto item)
        {
            item.LastChanged=MockDb.GetTimeStamp();
            var newItem = new DeductionEntity
            {
                Id = item.Id,
                TenantId = item.TenantId,
                Name = item.Name,
                IsSystemDefined = item.IsSystemDefined,
                Limit = item.Limit,
                Type = item.Type,
                LastChanged = item.LastChanged,
            };
            MockDb.Deductions.Add(newItem);

        }

        public void Update(DeductionDto item)
        {
            var result = MockDb.Deductions.
                Where(r => r.TenantId == item.TenantId && r.Id == item.Id)
                .Select(r => r).FirstOrDefault();
            if(result==null)
                throw new DataNotFoundException("Deduction");
            if (!result.LastChanged.Matches(item.LastChanged))
                throw new ConcurrencyException("Deduction");
            item.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = item.LastChanged;
            result.Name = item.Name;
            result.Type= item.Type;
            result.Limit = item.Limit;
        }
    }
}
