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
    public class EmployerExpensesStepDal :
        IEmployerExpensesStepDal
    {
        public EmployerExpensesStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.EmployerExpensesSteps
                          join t in MockDb.Steps on r.Id equals t.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new EmployerExpensesStepDto
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
                throw new DataNotFoundException("EmployerExpensesStep");
            return result;
        }

        public void Insert(EmployerExpensesStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new EmployerExpensesStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                CurrentStepIndex = dto.CurrentStepIndex,
                IsCompleted = dto.IsCompleted,
                Skip = dto.IsSkipped,
                LastChanged = dto.LastChanged
            };
            MockDb.EmployerExpensesSteps.Add(newItem);
        }

        public void Update(EmployerExpensesStepDto dto)
        {
            var result = (from r in MockDb.EmployerExpensesSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("EmployerExpensesStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("EmployerExpensesStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
            result.CurrentStepIndex = dto.CurrentStepIndex;
            result.Skip = dto.IsSkipped;
        }
    }
}

