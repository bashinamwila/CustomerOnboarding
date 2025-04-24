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
    public class StatutoryRegistrationsStepDal :
        IStatutoryRegistrationsStepDal
    {
        public StatutoryRegistrationsStepDto Fetch(string tenantId, int id)
        {
            var result = (from r in MockDb.StatutoryRegistrationsSteps
                          join s in MockDb.Steps on r.Id equals s.Id
                          where r.TenantId == tenantId
                          && r.Id == id
                          select new StatutoryRegistrationsStepDto
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
                throw new DataNotFoundException("BankingDetailsStep");
            return result;
        }

        public void Insert(StatutoryRegistrationsStepDto dto)
        {
            dto.LastChanged = MockDb.GetTimeStamp();
            var newItem = new StatutoryRegistrationsStepEntity
            {
                TenantId = dto.TenantId,
                Id = dto.StepId,
                StepIndex = dto.StepIndex,
                IsCompleted = dto.IsCompleted,
                LastChanged = dto.LastChanged
            };
            MockDb.StatutoryRegistrationsSteps.Add(newItem);
        }

        public void Update(StatutoryRegistrationsStepDto dto)
        {
            var result = (from r in MockDb.StatutoryRegistrationsSteps
                          where r.TenantId == dto.TenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("StatutoryRegistrationsSteps");
            if (!result.LastChanged.Matches(dto.LastChanged))
                throw new ConcurrencyException("StatutoryRegistrationsSteps");
            dto.LastChanged = MockDb.GetTimeStamp();
            result.IsCompleted = dto.IsCompleted;
            result.LastChanged = dto.LastChanged;
        }
    }
}
