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
    public class AddCompensationComponentConfirmationStepDal :
        IAddCompensationComponentConfirmationStepDal
    {
        public AddCompensationComponentConfirmationStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.AddCompensationComponentConfirmationSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new AddCompensationComponentConfirmationStepDto
                          {
                              StepId=r.Id,
                              Name=s.Name,
                              StepIndex=r.StepIndex,
                              Type=s.Type,
                              IsCompleted=r.IsCompleted,
                              ActionTaken=r.ActionTaken,
                              LastChanged=r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddCompensationComponentConfirmationStep");
            return result;
        }

        public void Insert(AddCompensationComponentConfirmationStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new AddCompensationComponentConfirmationStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                IsCompleted = dto.IsCompleted,
                StepIndex = dto.StepIndex,
                ActionTaken = dto.ActionTaken,
                LastChanged = dto.LastChanged


            };
            MockDb.AddCompensationComponentConfirmationSteps.Add(newItem);
        }

        public void Update(AddCompensationComponentConfirmationStepDto dto)
        {
            var result = (from r in MockDb.AddCompensationComponentConfirmationSteps
                          where r.TenantId == dto.TenantId && r.Id == dto.StepId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddCompensationComponentConfirmationStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("AddCompensationComponentConfirmationStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = dto.LastChanged;
            result.IsCompleted = dto.IsCompleted;
            result.ActionTaken = dto.ActionTaken;

        }
    }
}
