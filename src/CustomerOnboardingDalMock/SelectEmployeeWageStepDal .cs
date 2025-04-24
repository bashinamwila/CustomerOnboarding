using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class SelectEmployeeWageStepDal : ISelectEmployeeWageStepDal
    {
        public SelectEmployeeWageStepDto Fetch(string tenantId, int id, string employeeId)
        {
            var result = (from r in MockDb.SelectEmployeeWageSteps
                          join s in MockDb.Steps
                        on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id && r.EmployeeId == employeeId
                          select new SelectEmployeeWageStepDto
                          {
                              StepId=r.Id,
                              Name=s.Name,
                              Type=s.Type,
                              Counter=r.Counter,
                              IsCompleted=r.IsCompleted,
                              StepIndex=r.StepIndex,
                              WageId=r.WageId,
                              LastChanged=r.LastChanged
                          }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("SelectEmployeeWageStep");
            return result;
        }

        public void Insert(SelectEmployeeWageStepDto dto)
        {
            dto.Counter = MockDb.SelectEmployeeWageSteps.Count + 1;
            dto.LastChanged = MockDb.GetTimeStamp();

            var newItem = new SelectEmployeeWageStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                EmployeeId = dto.EmployeeId,
                WageId = dto.WageId,
                Counter = dto.Counter,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.SelectEmployeeWageSteps.Add(newItem);
        }

        public void Update(SelectEmployeeWageStepDto dto)
        {
            var result = MockDb.SelectEmployeeWageSteps.Where(r => r.TenantId == dto.TenantId
                                                        && r.Id == dto.StepId && r.EmployeeId == dto.EmployeeId)
                                                    .Select(r => r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("SelectEmployeeWageStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new DataNotFoundException("SelectEmployeeWageStep");

            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
        }
    }
}
