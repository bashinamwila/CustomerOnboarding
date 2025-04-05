using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class TenantOnboardingOrchestratorStepUpdater :
        CommandBase<TenantOnboardingOrchestratorStepUpdater>
    {
        public static readonly PropertyInfo<IStep> StepProperty =
            RegisterProperty<IStep>(nameof(Step));
        public IStep Step
        {
            get => ReadProperty(StepProperty);
            private set => LoadProperty(StepProperty, value);
        }

        public static readonly PropertyInfo<TenantOnboardingOrchestrator> TenantOnboardingOrchestratorProperty =
          RegisterProperty<TenantOnboardingOrchestrator>(nameof(TenantOnboardingOrchestrator));
        public TenantOnboardingOrchestrator TenantOnboardingOrchestrator
        {
            get => ReadProperty(TenantOnboardingOrchestratorProperty);
            private set => LoadProperty(TenantOnboardingOrchestratorProperty, value);
        }

        [Create]
        private void Create(TenantOnboardingOrchestrator tenantOnboardingOrchestrator ,IStep step)
        {
            TenantOnboardingOrchestrator = tenantOnboardingOrchestrator;
            Step = step;
        }

        [Execute]
        private async Task ExecuteAsync([Inject]ApplicationContext appCtx)
        {
            var type=Step.GetType();
            var dpType=typeof(IChildDataPortal<>).MakeGenericType(type);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            await dp.UpdateChildAsync(Step, TenantOnboardingOrchestrator);
        }
    }
}
