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
    public class TenantOnboardingOrchestratorDal : ITenantOnboardingOrchestratorDal
    {
        public OnboardingOrchestratorDto Fetch(string tenantId)
        {
            var result = (from r in MockDb.TenantOnboardingWorkflows
                          where r.TenantId == tenantId
                          select new OnboardingOrchestratorDto
                          {
                              TenantId = tenantId,
                              CurrentStepIndex = r.CurrentStepIndex,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("TenantOnboardingOrchestrator");
            return result;
        }

        public void Insert(OnboardingOrchestratorDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new TenantOnboardingEntity
            {
                TenantId = data.TenantId,
                CurrentStepIndex = data.CurrentStepIndex,
                LastChanged = data.LastChanged
            };
            MockDb.TenantOnboardingWorkflows.Add(newItem);
        }

        public void Update(OnboardingOrchestratorDto data)
        {
            UpdateCurrentStepIndex(data.TenantId, data.CurrentStepIndex, data.LastChanged);
        }

        public void UpdateCurrentStepIndex(string tenantId, int currentStepIndex, byte[] timeStamp)
        {
            var result = (from r in MockDb.TenantOnboardingWorkflows
                          where r.TenantId == tenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("TenantOnboardingOrchestrator");
            if (!result.LastChanged.Matches(timeStamp))
                throw new ConcurrencyException("TenantOnboardingOrchestrator");
            result.LastChanged = MockDb.GetTimeStamp();
            result.CurrentStepIndex = currentStepIndex;
        }
    }
}
