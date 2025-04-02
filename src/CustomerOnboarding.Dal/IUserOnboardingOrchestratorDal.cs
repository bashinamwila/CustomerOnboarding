using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IUserOnboardingOrchestratorDal
    {
        public void Insert(UserOnboardingOrchestratorDto data);
        public void Update(UserOnboardingOrchestratorDto data);
        public UserOnboardingOrchestratorDto Fetch(string tenantId);
        public void UpdateCurrentStepIndex(string tenantId, int currentStepIndex, byte[] timeStamp);
    }
}
