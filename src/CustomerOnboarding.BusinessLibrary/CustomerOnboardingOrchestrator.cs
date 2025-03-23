using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;


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
        private int CurrentStepIndex
        {
            get => GetProperty(CurrentStepIndexProperty);
            set => SetProperty(CurrentStepIndexProperty, value);
        }

        public async Task MoveNextAsync()
        {
            if (CurrentStepIndex >= Steps.Count) return;

            var step = Steps[CurrentStepIndex];

            if (step.Type == StepType.Automatic && !step.IsCompleted)
            {
                await step.ExecuteAsync();
            }

            if (step.IsCompleted || step.Type == StepType.Manual)
            {
                CurrentStepIndex++;
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
                var createAccountStep=await portal.GetPortal<CreateAccountStep>().CreateChildAsync();
                var sendEmailNotificationStep = await portal.GetPortal<SendEmailNotificationStep>().CreateChildAsync();
                Steps.AddRange(new IStep[] { createAccountStep, sendEmailNotificationStep });

            }
            await BusinessRules.CheckRulesAsync();
        }
    }
}
