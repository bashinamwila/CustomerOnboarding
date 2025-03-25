using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System.ComponentModel;


namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class CustomerOnboardingOrchestrator :
        BusinessBase<CustomerOnboardingOrchestrator>
    {
        public static readonly PropertyInfo<string>TenantIdProperty=
            RegisterProperty<string>(nameof(TenantId));
        public string TenantId
        {
            get=> GetProperty(TenantIdProperty);
            private set => LoadProperty(TenantIdProperty, value);
        }

        public static readonly PropertyInfo<bool>IsCompleteProperty=
                        RegisterProperty<bool>(nameof(IsComplete));
        public bool IsComplete
        {
            get => GetProperty(IsCompleteProperty);
            private set => SetProperty(IsCompleteProperty, value);
        }

        public static readonly PropertyInfo<Steps> StepsProperty =
            RegisterProperty<Steps>(nameof(Steps));

        public Steps Steps
        {
            get => GetProperty(StepsProperty);
            private set => LoadProperty(StepsProperty, value);
        }

        public static readonly PropertyInfo<int>CurrentStepIndexProperty=
            RegisterProperty<int>(nameof(CurrentStepIndex));
        public int CurrentStepIndex
        {
            get => GetProperty(CurrentStepIndexProperty);
            private set => SetProperty(CurrentStepIndexProperty, value);
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty =
            RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }

        

        public async Task MoveNextAsync()
        {
            while (CurrentStepIndex < Steps.Count)
            {
                var step = Steps[CurrentStepIndex];

                if (!step.IsCompleted && step.Type == StepType.Automatic)
                {
                    await step.ExecuteAsync();
                    CurrentStepIndex++;

                    var portal = ApplicationContext.GetRequiredService<IDataPortal<CustomerOnboardingOrchestratorCurrentStepUpdater>>();
                    var cmd = await portal.CreateAsync(TenantId, CurrentStepIndex, TimeStamp);
                    cmd = await portal.ExecuteAsync(cmd);
                }
                else if (step.Type == StepType.Manual)
                {
                    if (step.IsCompleted)
                    {
                        CurrentStepIndex++;
                        var portal = ApplicationContext.GetRequiredService<IDataPortal<CustomerOnboardingOrchestratorUpdater>>();
                        var updater = await portal.CreateAsync(this);
                        updater = await portal.ExecuteAsync(updater);
                        TimeStamp = updater.CustomerOnboardingOrchestrator.TimeStamp;
                    }
                    else
                    {
                        break; // wait for user to manually complete this step
                    }
                       
                }
                else
                {
                    break; // unexpected state
                }
            }
        }


       

        public IStep GoTo(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= Steps.Count)
                throw new ArgumentOutOfRangeException(nameof(stepIndex), "Invalid step index.");

            return Steps[stepIndex];
        }

       


        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new CheckIfWorkflowIsComplete(StepsProperty,IsCompleteProperty));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(StepsProperty, IsCompleteProperty));
        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            base.OnChildChanged(e);
            if (e.ChildObject is IStep || e.ChildObject is Steps)
            {
                BusinessRules.CheckRules(StepsProperty);
            }
        }

        [Create]
        private async Task CreateAsync([Inject]IChildDataPortalFactory portal)
        {
            using(BypassPropertyChecks)
            {
                TenantId = Guid.NewGuid().ToString();
                IsComplete = false;
                Steps = await portal.GetPortal<Steps>().CreateChildAsync();
                var createAccountStep=await portal.GetPortal<CreateAccountStep>().CreateChildAsync(TenantId,1);
                var sendEmailNotificationStep = await portal.GetPortal<SendEmailNotificationStep>().CreateChildAsync(2);
                var confirmEmailStep=await portal.GetPortal<ConfirmEmailStep>().CreateChildAsync(3);
                Steps.AddRange(new IStep[] { createAccountStep, sendEmailNotificationStep,confirmEmailStep });

            }
            await BusinessRules.CheckRulesAsync();
        }

        [Insert]
        private async Task InsertAsync([Inject]ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CustomerOnboardingOrchestratorDto
                {
                    TenantId=this.TenantId,
                    CurrentStepIndex=this.CurrentStepIndex,

                };
                 dal.Insert(data);
                TimeStamp = data.LastChanged;
                await portal.UpdateChildAsync(Steps,this);
            }
        }

        [Update]
        private async Task UpdateAsync([Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CustomerOnboardingOrchestratorDto
                {
                    TenantId = this.TenantId,
                    CurrentStepIndex = this.CurrentStepIndex,
                    LastChanged=this.TimeStamp

                };
                dal.Update(data);
                TimeStamp = data.LastChanged;
                await portal.UpdateChildAsync(Steps, this);
            }
        }

        [Fetch]
        private async Task FetchAsync(string tenantId, [Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId);
                
                TenantId = data.TenantId;
                CurrentStepIndex = data.CurrentStepIndex;
                TimeStamp = data.LastChanged;
                Steps=  await portal.FetchChildAsync(TenantId);
                
            }
            await BusinessRules.CheckRulesAsync();
        }

    }
}
