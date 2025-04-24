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
    public class AddDeductionsStepDal :
        IAddDeductionsStepDal
    {
        public AddDeductionsStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.AddDeductionSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new AddDeductionsStepDto
                          {
                              TenantId = r.TenantId,
                              Name = s.Name,
                              Type = s.Type,
                              StepId = r.Id,
                              StepIndex = r.StepIndex,
                              RuleSet = s.RuleSet,
                              IsCompleted = r.IsCompleted,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("AddDeductionStep");
            return result;

        }

        public void Insert(AddDeductionsStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new AddDeductionStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.AddDeductionSteps.Add(newItem);

        }

        public void Update(AddDeductionsStepDto dto)
        {
            var result = (from r in MockDb.AddDeductionSteps
                          where r.TenantId == dto.TenantId && r.Id == dto.StepId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddDeductionStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("AddDeductionStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = dto.LastChanged;
            result.IsCompleted = dto.IsCompleted;


        }
    }
}

