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
    public class AddCompensationComponentStepDal :
        IAddCompensationComponentStepDal
    {
        public AddCompensationComponentStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.AddCompensationComponentSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new AddCompensationComponentStepDto
                          {
                              TenantId=r.TenantId,
                              Name=s.Name,
                              Type=s.Type,
                              StepId=r.Id,
                              StepIndex=r.StepIndex,
                              RuleSet=s.RuleSet,
                              IsCompleted=r.IsCompleted,
                              LastChanged=r.LastChanged
                          }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("AddCompensationComponentStep");
            return result;

        }

        public void Insert(AddCompensationComponentStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new AddCompensationComponentStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged=dto.LastChanged
            };
            MockDb.AddCompensationComponentSteps.Add(newItem);

        }

        public void Update(AddCompensationComponentStepDto dto)
        {
            var result = (from r in MockDb.AddCompensationComponentSteps
                          where r.TenantId == dto.TenantId && r.Id == dto.StepId
                          select r).FirstOrDefault();
            if(result is null)
                throw new DataNotFoundException("AddCompensationComponentStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("AddCompensationComponentStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = dto.LastChanged;
            result.IsCompleted = dto.IsCompleted;


        }
    }
}
