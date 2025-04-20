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
    public class AddEmployerExpenseConfirmationStepDal :
        IAddEmployerExpenseConfirmationStepDal
    {
        public AddEmployerExpenseConfirmationStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.AddEmployerExpenseConfirmationSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new AddEmployerExpenseConfirmationStepDto
                          {
                              StepId = r.Id,
                              Name = s.Name,
                              StepIndex = r.StepIndex,
                              Type = s.Type,
                              IsCompleted = r.IsCompleted,
                              ActionTaken = r.ActionTaken,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddEmployerExpenseConfirmationStep");
            return result;
        }

        public void Insert(AddEmployerExpenseConfirmationStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new AddEmployerExpenseConfirmationStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                IsCompleted = dto.IsCompleted,
                StepIndex = dto.StepIndex,
                ActionTaken = dto.ActionTaken,
                LastChanged = dto.LastChanged


            };
            MockDb.AddEmployerExpenseConfirmationSteps.Add(newItem);
        }

        public void Update(AddEmployerExpenseConfirmationStepDto dto)
        {
            var result = (from r in MockDb.AddEmployerExpenseConfirmationSteps
                          where r.TenantId == dto.TenantId && r.Id == dto.StepId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddEmployerExpenseConfirmationStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("AddEmployerExpenseConfirmationStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.LastChanged = dto.LastChanged;
            result.IsCompleted = dto.IsCompleted;
            result.ActionTaken = dto.ActionTaken;

        }
    }
}

