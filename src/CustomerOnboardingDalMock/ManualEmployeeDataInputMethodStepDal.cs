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
    public class ManualEmployeeDataInputMethodStepDal :
        IManualEmployeeDataInputMethodStepDal
    {
        public ManualEmployeeDataInputMethodStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.ManualEmployeeDataInputMethodSteps
                          join t in MockDb.Steps on r.Id equals t.Id
                          where r.TenantId == tenantId && r.Id == id
                          select new ManualEmployeeDataInputMethodStepDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              Name = t.Name,
                              Type = t.Type,
                              IsCompleted = r.IsCompleted,
                              CurrentStepIndex = r.CurrentStepIndex,
                              StepIndex = r.StepIndex,
                              LastChanged = r.LastChanged

                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("ManualEmployeeDataInputMethodStep");
            return result;
        }

        public void Insert(ManualEmployeeDataInputMethodStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new ManualEmployeeDataInputMethodStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                CurrentStepIndex = dto.CurrentStepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.ManualEmployeeDataInputMethodSteps.Add(newItem);
        }

        public void Update(ManualEmployeeDataInputMethodStepDto dto)
        {
            var result = (from r in MockDb.ManualEmployeeDataInputMethodSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("ManualEmployeeDataInputMethodStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("ManualEmployeeDataInputMethodStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
            result.CurrentStepIndex = dto.CurrentStepIndex;
            
        }
    }
}
