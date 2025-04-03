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
        public UserOnboardingOrchestratorDto Fetch(string tenantId)
        {
            var result = (from r in MockDb.Customers
                          where r.TenantId == tenantId
                          select new UserOnboardingOrchestratorDto
                          {
                              TenantId = tenantId,
                              CurrentStepIndex = r.CurrentStepIndex,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("CustomerOnboardingOrchestrator");
            return result;
        }

        public void Insert(UserOnboardingOrchestratorDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new UserOnboardingEntity
            {
                TenantId = data.TenantId,
                CurrentStepIndex = data.CurrentStepIndex,
                LastChanged = data.LastChanged
            };
            MockDb.Customers.Add(newItem);

        }

        public void Update(UserOnboardingOrchestratorDto data)
        {
           // throw new NotImplementedException();
        }

        public void UpdateCurrentStepIndex(string tenantId, int currentStepIndex, byte[] timeStamp)
        {
            var result = (from r in MockDb.Customers
                          where r.TenantId == tenantId
                          select r).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("CustomerOnboardingOrchestrator");
            if (!result.LastChanged.Matches(timeStamp))
                throw new ConcurrencyException("CustomerOnboardingOrchestrator");
            result.LastChanged = MockDb.GetTimeStamp();
            result.CurrentStepIndex = currentStepIndex;
        }
    }
}
