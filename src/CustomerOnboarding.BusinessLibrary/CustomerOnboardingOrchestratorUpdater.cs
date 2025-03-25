using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class CustomerOnboardingOrchestratorUpdater :
        CommandBase<CustomerOnboardingOrchestratorUpdater>
    {
        public static readonly PropertyInfo<CustomerOnboardingOrchestrator> CustomerOnboardingOrchestratorProperty =
            RegisterProperty<CustomerOnboardingOrchestrator>(nameof(CustomerOnboardingOrchestrator));
        public CustomerOnboardingOrchestrator CustomerOnboardingOrchestrator
        {
            get=>ReadProperty(CustomerOnboardingOrchestratorProperty);
            private set=>LoadProperty(CustomerOnboardingOrchestratorProperty, value);
        }

        [Create]
        private async Task CreateAsync(CustomerOnboardingOrchestrator customerOnboardingOrchestrator)
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
