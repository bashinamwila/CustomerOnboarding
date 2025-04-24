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
    public class GeneralEmployeeWagesInformationStepDal :
        IGeneralEmployeeWagesInformationStepDal
    {
        public GeneralEmployeeWagesInformationStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.GeneralEmployeeWagesInformationSteps
                          join s in MockDb.Steps
                          on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new GeneralEmployeeWagesInformationStepDto
                          {
                              StepId = r.Id,
                              Name = s.Name,
                              Type = s.Type,
                              IsCompleted = r.IsCompleted,
                              StepIndex = r.StepIndex,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("GeneralEmployeeWagesInformationStep");
            return result;
        }

        public void Insert(GeneralEmployeeWagesInformationStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new GeneralEmployeeWagesInformationStepEntity
            {
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged,
                TenantId = dto.TenantId
            };
            MockDb.GeneralEmployeeWagesInformationSteps.Add(newItem);
        }

        public void Update(GeneralEmployeeWagesInformationStepDto dto)
        {
            var result = MockDb.GeneralEmployeeWagesInformationSteps.Where(r => r.TenantId == dto.TenantId
                                    && r.Id == dto.StepId)
                                    .Select(r => r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("GeneralEmployeeWagesInformationStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("GeneralEmployeeWagesInformationStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
        }
    }
}
