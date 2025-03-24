using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ICustomerOnboardingOrchestratorDal
    {
        public void Insert(CustomerOnboardingOrchestratorDto data);
        public void Update(CustomerOnboardingOrchestratorDto data);
        public CustomerOnboardingOrchestratorDto Fetch(string tenantId);
        public void UpdateCurrentStepIndex(string tenantId, int currentStepIndex, byte[] timeStamp);
    }
}
