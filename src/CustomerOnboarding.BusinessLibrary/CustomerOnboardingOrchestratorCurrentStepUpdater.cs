using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class CustomerOnboardingOrchestratorCurrentStepUpdater :
        CommandBase<CustomerOnboardingOrchestratorCurrentStepUpdater>
    {
        public static readonly PropertyInfo<string>TenantIdProperty=
            RegisterProperty<string>(nameof(TenantId));
        public string TenantId
        {
            get => ReadProperty(TenantIdProperty);
            private set => LoadProperty(TenantIdProperty, value);
        }

        public static readonly PropertyInfo<int> CurrentStepIndexProperty =
            RegisterProperty<int>(nameof(CurrentStepIndex));    
        public int CurrentStepIndex
        {
            get=>ReadProperty(CurrentStepIndexProperty);
            private set => LoadProperty(CurrentStepIndexProperty, value);
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty =
           RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => ReadProperty(TimeStampProperty);
            private set => LoadProperty(TimeStampProperty, value);
        }

        [Create]
        private async Task CreateAsync(string tenantId, int currentStepIndex, byte[] timeStamp)
        {
            TenantId = tenantId;
            CurrentStepIndex = currentStepIndex;
            TimeStamp = timeStamp;
            await Task.CompletedTask;
        }
        [Execute]
        private void Execute([Inject]ICustomerOnboardingOrchestratorDal dal)
        {
             dal.UpdateCurrentStepIndex(TenantId, CurrentStepIndex, TimeStamp);
        }

    }
}
