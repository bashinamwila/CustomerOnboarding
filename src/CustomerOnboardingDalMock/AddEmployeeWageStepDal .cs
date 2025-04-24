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
    public class AddEmployeeWageStepDal : IAddEmployeeWageStepDal
    {
        public AddEmployeeWageStepDto Fetch(string tenantId, int id,
            string employeeId, string wageId)
        {
            var result = (from r in MockDb.AddEmployeeWageSteps
                          join s in
                        MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id && r.EmployeeId == employeeId
                          && r.WageId == wageId
                          select new AddEmployeeWageStepDto
                          {
                              StepId=r.Id,
                              Name=s.Name,
                              RuleSet=s.RuleSet,
                              Type=s.Type,
                              IsCompleted=r.IsCompleted,
                              WageId=r.WageId,
                              EmployeeId=r.EmployeeId,
                              StepIndex=r.StepIndex,
                              LastChanged=r.LastChanged
                          }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("AddEmployeeWageStep");
            return result;
        }

        public void Insert(AddEmployeeWageStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new AddEmployeeWageStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                WageId = dto.WageId,
                EmployeeId = dto.EmployeeId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.AddEmployeeWageSteps.Add(newItem);
        }

        public void Update(AddEmployeeWageStepDto dto)
        {
            var result = MockDb.AddEmployeeWageSteps.Where(r =>
                                r.TenantId == dto.TenantId &&
                                r.Id == dto.StepId &&
                                r.EmployeeId == dto.EmployeeId &&
                                r.WageId == dto.WageId)
                        .Select(r => r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("AddEmployeeWageStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("AddEmployeeWageStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
        }
    }
}
