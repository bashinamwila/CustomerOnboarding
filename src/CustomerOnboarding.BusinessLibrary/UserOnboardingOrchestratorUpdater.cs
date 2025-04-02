using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class UserOnboardingOrchestratorUpdater :
        CommandBase<UserOnboardingOrchestratorUpdater>
    {
        public static readonly PropertyInfo<UserOnboardingOrchestrator> CustomerOnboardingOrchestratorProperty =
            RegisterProperty<UserOnboardingOrchestrator>(nameof(CustomerOnboardingOrchestrator));
        public UserOnboardingOrchestrator CustomerOnboardingOrchestrator
        {
            get=>ReadProperty(CustomerOnboardingOrchestratorProperty);
            private set=>LoadProperty(CustomerOnboardingOrchestratorProperty, value);
        }

        [Create]
        private async Task CreateAsync(UserOnboardingOrchestrator customerOnboardingOrchestrator)
        {
            CustomerOnboardingOrchestrator=customerOnboardingOrchestrator;
            await Task.CompletedTask;
        }
        [Execute]
        private async Task ExecuteAsync()
        {
            CustomerOnboardingOrchestrator = await CustomerOnboardingOrchestrator.SaveAsync();
        }
    }
}
