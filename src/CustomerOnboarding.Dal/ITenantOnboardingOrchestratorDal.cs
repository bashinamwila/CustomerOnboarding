using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ITenantOnboardingOrchestratorDal
    {
        public void Insert(OnboardingOrchestratorDto data);
        public void Update(OnboardingOrchestratorDto data);
        public OnboardingOrchestratorDto Fetch(string tenantId);
        public void UpdateCurrentStepIndex(string tenantId, int currentStepIndex, byte[] timeStamp);
    }
}
