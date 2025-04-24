using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class TenantOnboardingStatusFinder :
        ReadOnlyBase<TenantOnboardingStatusFinder>

    {
        public static readonly PropertyInfo<TenantOnboardingStatus> TenantOnboardingStatusProperty =
            RegisterProperty<TenantOnboardingStatus>(nameof(Result));   
        public TenantOnboardingStatus Result
        {
            get => GetProperty(TenantOnboardingStatusProperty);
            private set => LoadProperty(TenantOnboardingStatusProperty, value);
        }

        [Fetch]
        private async Task FetchAsync(string id,
            [Inject] IDataPortalFactory factory)
        {
            var cmd=factory.GetPortal<HasStartedOnboardingCommand>().Execute(id);
            if (cmd.HasStartedOnboarding)
            {
                var tenantOnboarding=await factory.GetPortal<TenantOnboardingOrchestrator>().FetchAsync(id);
                if(tenantOnboarding.IsComplete)
                    Result=TenantOnboardingStatus.Completed;
                else
                    Result = TenantOnboardingStatus.InProgress;
            }
            else
            {
                Result = TenantOnboardingStatus.NotStarted;
            }
        }
    }
}
