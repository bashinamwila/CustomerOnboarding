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
    public class ComplianceInfoStepDal : IComplianceInfoStepDal
    {
        public ComplianceInfoStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.ComplianceInfoSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new ComplianceInfoStepDto
                          {
                              TenantId = r.TenantId,
                              StepId = r.Id,
                              StepIndex = r.StepIndex,
                              Name = s.Name,
                              Type = s.Type,
                              IsCompleted = r.IsCompleted,
                              RuleSet = s.RuleSet,
                              CurrentStepIndex=r.CurrentStepIndex,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("ComplianceInfoStep");
            return result;
        }

        public void Insert(ComplianceInfoStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new ComplianceInfoStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                CurrentStepIndex=dto.CurrentStepIndex,
                LastChanged = dto.LastChanged
            };
            MockDb.ComplianceInfoSteps.Add(newItem);
        }

        public void Update(ComplianceInfoStepDto dto)
        {

            var result = (from r in MockDb.ComplianceInfoSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("ComplianceInfoStep");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("ComplianceInfoStep");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.CurrentStepIndex = dto.CurrentStepIndex;
            result.LastChanged = dto.LastChanged;
        }
    }
}
