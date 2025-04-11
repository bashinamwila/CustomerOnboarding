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
    public class StatutoryRegistrationsDal : IStatutoryRegistrationsDal
    {
        public bool Exists(string tenantId)
        {
            var result=MockDb.StatutoryRegistrations.Any(r => r.TenantId == tenantId);
            return result;
        }

        public StatutoryRegistrationsDto Fetch(string tenantId)
        {
            var result = (from r in MockDb.StatutoryRegistrations
                          where r.TenantId == tenantId
                          select new StatutoryRegistrationsDto
                          {
                              TPIN = r.TPIN,
                              NAPSAAccountNumber = r.NAPSAAccountNumber,
                              NHIMAAccountNumber = r.NHIMAAccountNumber,
                              LastChanged = r.LastChanged,
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("StatutoryRegistrations");
            return result;
        }

        public void Insert(StatutoryRegistrationsDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem=new StatutoryRegistrationsEntity
            {
                TenantId = dto.TenantId,
                TPIN = dto.TPIN,
                NAPSAAccountNumber = dto.NAPSAAccountNumber,
                NHIMAAccountNumber = dto.NHIMAAccountNumber,
                LastChanged = dto.LastChanged
            };

            MockDb.StatutoryRegistrations.Add(newItem);
        }

        public void Update(StatutoryRegistrationsDto dto)
        {
            var result = (from r in MockDb.StatutoryRegistrations
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("StatutoryRegistrations");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("StatutoryRegistrations");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.TPIN = dto.TPIN;
            result.NAPSAAccountNumber = dto.NAPSAAccountNumber;
            result.NHIMAAccountNumber = dto.NHIMAAccountNumber;
            result.LastChanged = dto.LastChanged;
        }
    }
}
