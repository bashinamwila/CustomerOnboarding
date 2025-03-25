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
    public class CustomerOnboardingOrchestratorDal :
        ICustomerOnboardingOrchestratorDal
    {
        public CustomerOnboardingOrchestratorDto Fetch(string tenantId)
        {
            var result = (from r in MockDb.Customers
                          where r.TenantId == tenantId
                          select new CustomerOnboardingOrchestratorDto
                          {
                              TenantId = tenantId,
                              CurrentStepIndex = r.CurrentStepIndex,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("CustomerOnboardingOrchestrator");
            return result;
        }

        public void Insert(CustomerOnboardingOrchestratorDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new CustomerOnboardingEntity
            {
                TenantId = data.TenantId,
                CurrentStepIndex = data.CurrentStepIndex,
                LastChanged = data.LastChanged
            };
            MockDb.Customers.Add(newItem);

        }

        public void Update(CustomerOnboardingOrchestratorDto data)
        {
            throw new NotImplementedException();
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
