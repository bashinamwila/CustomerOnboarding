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
    public class UserOnboardingOrchestratorDal :
        IUserOnboardingOrchestratorDal
    {
        public OnboardingOrchestratorDto Fetch(string tenantId)
        {
            var result = (from r in MockDb.UserOnboardingWorkflows
                          where r.TenantId == tenantId
                          select new OnboardingOrchestratorDto
                          {
                              TenantId = tenantId,
                              CurrentStepIndex = r.CurrentStepIndex,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("UserOnboardingOrchestrator");
            return result;
        }

        public void Insert(OnboardingOrchestratorDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new UserOnboardingEntity
            {
                TenantId = data.TenantId,
                CurrentStepIndex = data.CurrentStepIndex,
                LastChanged = data.LastChanged
            };
            MockDb.UserOnboardingWorkflows.Add(newItem);

        }

        public void Update(OnboardingOrchestratorDto data)
        {
            UpdateCurrentStepIndex(data.TenantId,data.CurrentStepIndex,data.LastChanged);
        }

        public void UpdateCurrentStepIndex(string tenantId, int currentStepIndex, byte[] timeStamp)
        {
            var result = (from r in MockDb.UserOnboardingWorkflows
                          where r.TenantId == tenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("UserOnboardingOrchestrator");
            if (!result.LastChanged.Matches(timeStamp))
                throw new ConcurrencyException("UserOnboardingOrchestrator");
            result.LastChanged = MockDb.GetTimeStamp();
            result.CurrentStepIndex = currentStepIndex;
        }
    }
}
