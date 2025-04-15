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
    public class GeneralCompensationComponentsInformationDal
        :IGeneralCompensationComponentsInformationDal
    {
        public GeneralCompensationComponentsInformationDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.GeneralCompensationComponentsInformationSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new GeneralCompensationComponentsInformationDto
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
                throw new DataNotFoundException("GeneralCompensationComponentsInformationStep");
            return result;
        }

        public void Insert(GeneralCompensationComponentsInformationDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new GeneralCompensationComponentsInformationEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.GeneralCompensationComponentsInformationSteps.Add(newItem);
        }

        public void Update(GeneralCompensationComponentsInformationDto dto)
        {
            var result = (from r in MockDb.GeneralCompensationComponentsInformationSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("GeneralCompensationComponentsInformationStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("GeneralCompensationComponentsInformationStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
        }
    }
}
