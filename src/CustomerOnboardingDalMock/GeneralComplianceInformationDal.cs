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
    public class GeneralComplianceInformationDal : IGeneralComplianceInformationDal
    {
        public GeneralComplianceInformationDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.GeneralComplianceInformation
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new GeneralComplianceInformationDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              StepIndex = r.StepIndex,
                              Name = s.Name,
                              Type = s.Type,
                              IsCompleted = r.IsCompleted,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("GeneralComplianceInformationStep");
            return result;
        }

        public void Insert(GeneralComplianceInformationDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new GeneralComplianceInformationEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.GeneralComplianceInformation.Add(newItem);
        }

        public void Update(GeneralComplianceInformationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
