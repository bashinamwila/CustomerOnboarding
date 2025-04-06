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
                LastChanged = dto.LastChanged
            };
            MockDb.ComplianceInfoSteps.Add(newItem);
        }

        public void Update(ComplianceInfoStepDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
