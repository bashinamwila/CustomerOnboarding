using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class TenantOnboardingStepsGetter :
        OnboardingStepsGetterBase<TenantOnboardingStepsGetter>
    {
        [Fetch]
        private async Task FetchAsync(
            string tenantId,int currentStepIndex,
            [Inject] ITenantOnboardingStepsDal dal,
            [Inject]IChildDataPortalFactory portal,
            [Inject]IDataPortal<StepFactory>factory)
        {
            Steps = await portal.GetPortal<Steps>().FetchChildAsync();
            var stepMetadataList = dal.Fetch(tenantId);
            foreach (var stepMeta in stepMetadataList)
            {
                var getter = await factory.FetchAsync(stepMeta.TenantId, stepMeta.Id, currentStepIndex);

                Steps.Add(getter.Result);
            }
        }
    }
}
