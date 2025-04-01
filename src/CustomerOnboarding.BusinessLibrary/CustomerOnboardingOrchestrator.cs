using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System.ComponentModel;

namespace CustomerOnboarding.BusinessLibrary
{
    /// <summary>
    /// Orchestrates the customer onboarding process by managing multiple onboarding steps.
    /// Supports progression logic and workflow state management.
    /// </summary>
    [Serializable]
    public class CustomerOnboardingOrchestrator : BusinessBase<CustomerOnboardingOrchestrator>
    {
        #region Properties

        public static readonly PropertyInfo<string> TenantIdProperty =
            RegisterProperty<string>(nameof(TenantId));

        /// <summary>
        /// Unique identifier for the onboarding session (typically a GUID).
        /// </summary>
        public string TenantId
        {
            get => GetProperty(TenantIdProperty);
            private set => LoadProperty(TenantIdProperty, value);
        }

        public static readonly PropertyInfo<bool> IsCompleteProperty =
            RegisterProperty<bool>(nameof(IsComplete));

        /// <summary>
        /// Indicates whether all steps in the workflow have been completed.
        /// </summary>
        public bool IsComplete
        {
            get => GetProperty(IsCompleteProperty);
            private set => SetProperty(IsCompleteProperty, value);
        }

        public static readonly PropertyInfo<Steps> StepsProperty =
            RegisterProperty<Steps>(nameof(Steps));

        /// <summary>
        /// A collection of steps representing the onboarding process.
        /// </summary>
        public Steps Steps
        {
            get => GetProperty(StepsProperty);
            private set => LoadProperty(StepsProperty, value);
        }

        public static readonly PropertyInfo<int> CurrentStepIndexProperty =
            RegisterProperty<int>(nameof(CurrentStepIndex));

        /// <summary>
        /// Index of the currently active step in the onboarding workflow.
        /// </summary>
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

       

        #endregion

        #region Workflow Methods

        /// <summary>
        /// Advances the workflow to the next step if it is automatic or already completed.
        /// Stops at the next incomplete manual step.
        /// </summary>
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

        /// <summary>
        /// Navigates directly to a specific step in the workflow.
        /// </summary>
        public IStep GoTo(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= Steps.Count)
                throw new ArgumentOutOfRangeException(nameof(stepIndex), "Invalid step index.");

            return Steps[stepIndex];
        }

        #endregion

        #region Business Rules

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Rule: Set IsComplete = true if all steps are complete
            BusinessRules.AddRule(new CheckIfWorkflowIsComplete(StepsProperty, IsCompleteProperty));

            // Dependency: Whenever Steps change, reevaluate IsComplete
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(StepsProperty, IsCompleteProperty));
        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            base.OnChildChanged(e);

            // If any step or the Steps collection changes, re-evaluate rules
            if (e.ChildObject is IStep || e.ChildObject is Steps)
            {
                BusinessRules.CheckRules(StepsProperty);
            }
        }

        #endregion

        #region Data Access (DataPortal Methods)

        [Create]
        private async Task CreateAsync([Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                TenantId = Guid.NewGuid().ToString();
                IsComplete = false;
                CurrentStepIndex = 0;

                Steps = await portal.GetPortal<Steps>().CreateChildAsync();

                // Add steps to the workflow
                var createAccountStep = await portal.GetPortal<CreateAccountStep>().CreateChildAsync(TenantId, 1,CurrentStepIndex);
                var sendEmailStep = await portal.GetPortal<SendEmailNotificationStep>().CreateChildAsync(2,CurrentStepIndex);
                var confirmEmailStep = await portal.GetPortal<ConfirmEmailStep>().CreateChildAsync(3, CurrentStepIndex);

                Steps.AddRange(new IStep[] { createAccountStep, sendEmailStep, confirmEmailStep });
            }

            await BusinessRules.CheckRulesAsync();
        }

        [Insert]
        private async Task InsertAsync(
            [Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new CustomerOnboardingOrchestratorDto
                {
                    TenantId = this.TenantId,
                    CurrentStepIndex = this.CurrentStepIndex
                };

                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                await portal.UpdateChildAsync(Steps, this);
            }
        }

        [Update]
        private async Task UpdateAsync(
            [Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new CustomerOnboardingOrchestratorDto
                {
                    TenantId = this.TenantId,
                    CurrentStepIndex = this.CurrentStepIndex,
                    LastChanged = this.TimeStamp
                };

                dal.Update(dto);
                TimeStamp = dto.LastChanged;

                await portal.UpdateChildAsync(Steps, this);
            }
        }

        [Fetch]
        private async Task FetchAsync(
            string tenantId,
            [Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = dal.Fetch(tenantId);

                TenantId = dto.TenantId;
                CurrentStepIndex = dto.CurrentStepIndex;
                TimeStamp = dto.LastChanged;
                Steps = await portal.FetchChildAsync(TenantId,CurrentStepIndex);
            }

            await BusinessRules.CheckRulesAsync();
        }

        #endregion
    }
}
