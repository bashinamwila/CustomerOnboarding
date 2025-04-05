using Csla;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    public class TenantOnboardingOrchestratorUpdater :
        CommandBase<TenantOnboardingOrchestratorUpdater>
    {
        public static readonly PropertyInfo<TenantOnboardingOrchestrator> TenantOnboardingOrchestratorProperty =
           RegisterProperty<TenantOnboardingOrchestrator>(nameof(TenantOnboardingOrchestrator));
        public TenantOnboardingOrchestrator TenantOnboardingOrchestrator
        {
            get => ReadProperty(TenantOnboardingOrchestratorProperty);
            private set => LoadProperty(TenantOnboardingOrchestratorProperty, value);
        }

        [Create]
        private void Create(TenantOnboardingOrchestrator tenantOnboardingOrchestrator)
        {
            TenantOnboardingOrchestrator = tenantOnboardingOrchestrator;
            
        }
        [Execute]
        private async Task ExecuteAsync()
        {
            TenantOnboardingOrchestrator=await TenantOnboardingOrchestrator.SaveAsync();
        }
    }
}
