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
    public class EmployeeWagesStepDal : IEmployeeWagesStepDal
    {
        public EmployeeWagesStepDto Fetch(string tenantId, int id,string employeeId)
        {
            var result = (from r in MockDb.EmployeeWagesSteps
                          join s in MockDb.Steps
                                on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id && r.EmployeeId == employeeId
                          select new EmployeeWagesStepDto
                          {
                              StepId=r.Id,
                              Name=s.Name,
                              Type=s.Type,
                              CurrentStepIndex=r.CurrentStepIndex,
                              StepIndex=r.StepIndex,
                              IsCompleted=r.IsCompleted,
                              LastChanged=r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("EmployeeWagesStep");
            return result;
        }

        public void Insert(EmployeeWagesStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();

            var newItem = new EmployeeWagesStepEntity
            {
                TenantId=dto.TenantId,
                Id=dto.StepId,
                EmployeeId=dto.EmployeeId,
                CurrentStepIndex=dto.CurrentStepIndex,
                StepIndex=dto.StepIndex,
                IsCompleted=dto.IsCompleted,
                LastChanged=dto.LastChanged
            };

            MockDb.EmployeeWagesSteps.Add(newItem);

        }

        public void Update(EmployeeWagesStepDto dto)
        {
            var result = MockDb.EmployeeWagesSteps.Where(r => r.TenantId == dto.TenantId &&
                                               r.Id == dto.StepId && r.EmployeeId == dto.EmployeeId
                                               )
                                                .Select(r => r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("EmployeeWagesStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new DataNotFoundException("EmployeeWagesStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.CurrentStepIndex = dto.CurrentStepIndex;
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
        }
    }
}
