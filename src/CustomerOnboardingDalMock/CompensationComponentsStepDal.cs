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
    public class CompensationComponentsStepDal :
        ICompensationComponentsStepDal
    {
        public CompensationComponentsStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.CompensationComponentsSteps
                          join t in MockDb.Steps on r.Id equals t.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new CompensationComponentsStepDto
                          {
                            TenantId=r.TenantId,
                             StepId=r.Id,
                             Name=t.Name,
                             Type=t.Type,
                             Skip=r.Skip,
                             IsCompleted=r.IsCompleted,
                             CurrentStepIndex=r.CurrentStepIndex,
                             StepIndex=r.StepIndex,
                             LastChanged=r.LastChanged

                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("CompensationComponentsStep");
            return result;
        }

        public void Insert(CompensationComponentsStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new CompensationComponentsStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                CurrentStepIndex = dto.CurrentStepIndex,
                IsCompleted = dto.IsCompleted,
                Skip = dto.Skip,
                LastChanged = dto.LastChanged
            };
            MockDb.CompensationComponentsSteps.Add(newItem);
        }

        public void Update(CompensationComponentsStepDto dto)
        {
            var result = (from r in MockDb.CompensationComponentsSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("CompensationComponentsStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("CompensationComponentsStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
            result.CurrentStepIndex = dto.CurrentStepIndex;
            result.Skip = dto.Skip;
        }
    }
}
