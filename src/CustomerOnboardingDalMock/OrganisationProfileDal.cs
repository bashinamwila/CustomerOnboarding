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
    public class OrganisationProfileDal :
        IOrganisationProfileDal
    {
        public OrganisationProfileDto Fetch(string id)
        {
            var result = (from r in MockDb.OrganisationProfiles
                          join o in MockDb.Organisations
                          on r.TenantId equals o.Id
                          where r.TenantId == id
                          select new OrganisationProfileDto
                          {
                              AddressLine1 = r.AddressLine1,
                              AddressLine2 = r.AddressLine2,
                              Email = r.Email,
                              PhoneNumber = r.PhoneNumber,
                              Name = o.Name,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("OrganisationProfile");
            return result;
        }

        public void Insert(OrganisationProfileDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new OrganisationProfileEntity
            {
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                LastChanged = dto.LastChanged,
                TenantId = dto.TenantId,

            };
            MockDb.OrganisationProfiles.Add(newItem);
        }

        public void Update(OrganisationProfileDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
