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
    public class AddEmployeeEmploymentDetailsStepDal : IAddEmployeeEmploymentDetailsStepDal
    {
        public AddEmployeeEmploymentDetailsStepDto Fetch(string tenantId, int id, int counter)
        {
            var result = (from r in MockDb.AddEmployeeEmploymentDetailsSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id
                          && r.Counter==counter
                          select new AddEmployeeEmploymentDetailsStepDto
                          {
                              TenantId = r.TenantId,
                              Name = s.Name,
                              Type = s.Type,
                              StepId = r.Id,
                              StepIndex = r.StepIndex,
                              RuleSet = s.RuleSet,
                              IsCompleted = r.IsCompleted,
                              Counter=r.Counter,
                              EmployeeId=r.EmployeeId,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("AddEmployeeEmploymentDetailsStep");
            return result;
        }

        public void Insert(AddEmployeeEmploymentDetailsStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            dto.Counter = MockDb.AddEmployeeEmploymentDetailsSteps.Count + 1;
            var newItem = new AddEmployeeEmploymentDetailsStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                Counter=dto.Counter,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                EmployeeId=dto.EmployeeId,
                LastChanged = dto.LastChanged
            };
            MockDb.AddEmployeeEmploymentDetailsSteps.Add(newItem);
        }

        public void Update(AddEmployeeEmploymentDetailsStepDto dto)
        {
            var result = (from r in MockDb.AddEmployeeEmploymentDetailsSteps
                          where r.TenantId == dto.TenantId && r.Id == dto.StepId
                          && r.Counter==dto.Counter
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddEmployeeEmploymentDetailsStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("AddEmployeeEmploymentDetailsStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.EmployeeId = dto.EmployeeId;
            result.LastChanged = dto.LastChanged;
            result.IsCompleted = dto.IsCompleted;
        }
    }
}
