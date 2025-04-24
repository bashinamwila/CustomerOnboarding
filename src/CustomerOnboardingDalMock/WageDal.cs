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
    public class WageDal :
        IWageDal
    {
        public Task Delete(string tenantId,string id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string tenantId,string id)
        {
            var result = MockDb.Wages.Any(r => r.Id == id
            && r.TenantId==tenantId);
            return result;
        }

        public List<WageDto> Fetch(string tenantId)
        {
            var result = (from r in MockDb.Wages
                          where r.TenantId == tenantId
                          select new WageDto
                          {
                              Id = r.Id,
                              Name = r.Name,
                              Type = r.Type,
                              Formular = r.Formular,
                              IsSystemDefined = r.IsSystemDefined,
                              LastChanged = r.LastChanged
                          }).ToList();
            return result;
        }

        public WageDto Fetch(string tenantId, string id)
        {
            var result = MockDb.Wages.Where(r => r.TenantId == tenantId &&
                r.Id == id)
                .Select(r => new WageDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSystemDefined = r.IsSystemDefined,
                    Type = r.Type,
                    Formular = r.Formular,
                    LastChanged = r.LastChanged
                }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("Wage");
            return result;
        }

        public void Insert(WageDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new WageEntity
            {
                Id = data.Id,
                Name = data.Name,
                Type = data.Type,
                IsSystemDefined = data.IsSystemDefined,
                Formular = data.Formular,
                TenantId = data.TenantId,
                LastChanged = data.LastChanged
            };
            MockDb.Wages.Add(newItem);
        }

        public void Update(WageDto data)
        {
            var result = (from r in MockDb.Wages
                          where r.TenantId == data.TenantId && r.Id == data.Id
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("Wage");
            if (!result.LastChanged.Matches(data.LastChanged!))
                throw new ConcurrencyException("Wage");
            data.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = data.LastChanged;
            result.Name = data.Name;
            result.Formular = data.Formular;
            result.Type = data.Type;

        }
    }
}
