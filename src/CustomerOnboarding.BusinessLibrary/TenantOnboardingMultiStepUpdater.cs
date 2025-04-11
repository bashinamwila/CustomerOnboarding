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
    public class TenantOnboardingMultiStepUpdater :
        CommandBase<TenantOnboardingMultiStepUpdater>
    {

        [Execute]
        private async Task ExecuteAsync(TenantOnboardingOrchestrator parent,
            int nextStep,IOnboardingOrchestrator orchestrator, [Inject]ApplicationContext appCtx)
        {
            var type = orchestrator.GetType();
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            await dp.UpdateChildAsync(orchestrator, parent, nextStep);
            
        }

    }
}
