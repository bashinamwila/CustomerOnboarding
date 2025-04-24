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
    public class DeductionsStepDal :
        IDeductionsStepDal
    {
        public DeductionsStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.DeductionsSteps
                          join t in MockDb.Steps on r.Id equals t.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new DeductionsStepDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              Name = t.Name,
                              Type = t.Type,
                              IsSkipped = r.Skip,
                              IsCompleted = r.IsCompleted,
                              CurrentStepIndex = r.CurrentStepIndex,
                              StepIndex = r.StepIndex,
                              LastChanged = r.LastChanged

                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("DeductionsStep");
            return result;
        }

        public void Insert(DeductionsStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new DeductionsStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                CurrentStepIndex = dto.CurrentStepIndex,
                IsCompleted = dto.IsCompleted,
                Skip = dto.IsSkipped,
                LastChanged = dto.LastChanged
            };
            MockDb.DeductionsSteps.Add(newItem);
        }

        public void Update(DeductionsStepDto dto)
        {
            var result = (from r in MockDb.DeductionsSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("DeductionsStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("DeductionsStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
            result.CurrentStepIndex = dto.CurrentStepIndex;
            result.Skip = dto.IsSkipped;
        }
    }
}

