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
        /// This is managed by the CheckIfWorkflowIsComplete business rule.
        /// </summary>
        public bool IsComplete
        {
            get => GetProperty(IsCompleteProperty);
            private set => SetProperty(IsCompleteProperty, value); // Rule sets this
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

        /// <summary>
        /// Timestamp for concurrency control during data persistence.
        /// </summary>
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
        /// Asynchronously advances the workflow to the next step.
        /// It executes automatic steps and proceeds until an incomplete manual step is encountered
        /// or the end of the workflow is reached. Persists state changes for step index updates.
        /// </summary>
        /// <remarks>
        /// This method uses helper command/updater objects (`CustomerOnboardingOrchestratorCurrentStepUpdater`,
        /// `CustomerOnboardingOrchestratorUpdater`) to persist changes to the orchestrator's state
        /// during the progression, ensuring data integrity.
        /// </remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task MoveNextAsync()
        {
            // Ensure we haven't gone past the last step
            while (CurrentStepIndex < Steps.Count)
            {
                var step = Steps[CurrentStepIndex];

                // Handle Automatic Steps
                if (!step.IsCompleted && step.Type == StepType.Automatic)
                {
                    await step.ExecuteAsync(); // Execute the automatic step logic
                    CurrentStepIndex++;        // Move to the next index

                    // Persist the CurrentStepIndex change using a dedicated command
                    var portal = ApplicationContext.GetRequiredService<IDataPortal<CustomerOnboardingOrchestratorCurrentStepUpdater>>();
                    // Create the command with necessary data (tenant ID, new index, timestamp for concurrency)
                    var cmd = await portal.CreateAsync(TenantId, CurrentStepIndex, TimeStamp);
                    // Execute the command to update the database
                    cmd = await portal.ExecuteAsync(cmd);
                    // Update the local timestamp from the command result
                    TimeStamp = cmd.TimeStamp;
                }
                // Handle Manual Steps
                else if (step.Type == StepType.Manual)
                {
                    // If the manual step is already complete, move to the next one
                    if (step.IsCompleted)
                    {
                        CurrentStepIndex++;
                        // Persist the CurrentStepIndex change and any potential changes in completed steps
                        // using a dedicated updater object.
                        var portal = ApplicationContext.GetRequiredService<IDataPortal<CustomerOnboardingOrchestratorUpdater>>();
                        var updater = await portal.CreateAsync(this); // Pass the current orchestrator instance
                        updater = await portal.ExecuteAsync(updater); // Execute the save logic within the updater
                        // Update the local timestamp from the updater result
                        TimeStamp = updater.CustomerOnboardingOrchestrator.TimeStamp;
                    }
                    else
                    {
                        // If the manual step is not complete, stop progressing.
                        // The user needs to complete this step via the UI.
                        break;
                    }
                }
                else
                {
                    // Should not happen with current step types (Manual/Automatic)
                    // Log potential issue or throw? For now, just break.
                    // TODO: Add logging here for unexpected step types.
                    break; // Unexpected state
                }
            }
        }

        /// <summary>
        /// Navigates directly to a specific step in the workflow by its index.
        /// </summary>
        /// <param name="stepIndex">The zero-based index of the step to navigate to.</param>
        /// <returns>The <see cref="IStep"/> at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="stepIndex"/> is out of the valid range for the <see cref="Steps"/> collection.</exception>
        public IStep GoTo(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= Steps.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(stepIndex), "Invalid step index.");
            }
            // Note: This method doesn't change the CurrentStepIndex automatically.
            // It simply returns the step object at the given index. UI logic would
            // typically use this to display the correct step, but progression state
            // is managed by MoveNextAsync or explicit setting of CurrentStepIndex (if allowed).
            return Steps[stepIndex];
        }

        #endregion

        #region Business Rules

        /// <summary>
        /// Adds business rules specific to the orchestrator.
        /// </summary>
        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Rule: The workflow is complete (IsComplete = true) only if all steps in the Steps collection are complete.
            BusinessRules.AddRule(new CheckIfWorkflowIsComplete(StepsProperty, IsCompleteProperty));

            // Dependency: Ensure the IsComplete property is re-evaluated whenever the Steps collection or its items change.
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(StepsProperty, IsCompleteProperty));
        }

        /// <summary>
        /// Handles changes in child objects (specifically the Steps collection or individual Step items).
        /// Ensures that business rules dependent on the Steps collection are re-checked.
        /// </summary>
        /// <param name="e">Event arguments containing information about the child change.</param>
        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            base.OnChildChanged(e);

            // If any step within the Steps collection changes, or if the Steps collection itself changes
            // (e.g., items added/removed, though not typical after creation here),
            // re-evaluate rules associated with the StepsProperty (like CheckIfWorkflowIsComplete).
            if (e.ChildObject is IStep || e.ChildObject is Steps)
            {
                BusinessRules.CheckRules(StepsProperty); // Check rules associated with StepsProperty
            }
        }

        #endregion



        #region Data Access (DataPortal Methods)


        /// <summary>
        /// DataPortal_Create method. Initializes a new CustomerOnboardingOrchestrator instance.
        /// This creates the initial structure of the onboarding workflow, including its steps.
        /// Uses the <see cref="StepFactory"/> to dynamically create each step instance.
        /// </summary>
        /// <param name="factoryPortal">Data portal instance for the <see cref="StepFactory"/>.</param>

        [Create]
        private async Task CreateAsync([Inject] IChildDataPortalFactory factoryPortal)
        {
            using (BypassPropertyChecks)
            {
                TenantId = Guid.NewGuid().ToString(); // Assign a unique ID for this onboarding session
                IsComplete = false;                   // Workflow starts as incomplete
                CurrentStepIndex = 0;                 // Start at the first step


                Steps = await factoryPortal.GetPortal<Steps>().CreateChildAsync();

                // Add steps to the workflow
                var createAccountStep = await factoryPortal.GetPortal<CreateAccountStep>().CreateChildAsync(TenantId, 1,CurrentStepIndex);
                var sendEmailStep = await factoryPortal.GetPortal<SendEmailNotificationStep>().CreateChildAsync(2,CurrentStepIndex);
                var confirmEmailStep = await factoryPortal.GetPortal<ConfirmEmailStep>().CreateChildAsync(3, CurrentStepIndex);

                Steps.AddRange(new IStep[] { createAccountStep, sendEmailStep, confirmEmailStep });
            }

            await BusinessRules.CheckRulesAsync();



        }


        /// <summary>
        /// DataPortal_Insert method. Persists a new CustomerOnboardingOrchestrator instance and its child Steps to the data store.
        /// </summary>
        /// <param name="dal">The data access layer implementation for the orchestrator.</param>
        /// <param name="portal">The child data portal for managing the Steps collection.</param>

        [Insert]
        private async Task InsertAsync(
            [Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                // Create the DTO for the orchestrator itself
                var dto = new CustomerOnboardingOrchestratorDto
                {
                    TenantId = this.TenantId,
                    CurrentStepIndex = this.CurrentStepIndex
                    // TimeStamp will be set by the DAL upon insertion
                };
                // Persist the orchestrator data
                dal.Insert(dto);
                // Update the object's timestamp with the value returned from the DAL
                TimeStamp = dto.LastChanged;

                // Cascade the insert/update operation to the child Steps collection
                // This will trigger the appropriate InsertChild/UpdateChild methods on each IStep

                await portal.UpdateChildAsync(Steps, this); // Pass 'this' as the parent context
            }
        }


        /// <summary>
        /// DataPortal_Update method. Persists changes for an existing CustomerOnboardingOrchestrator instance and its child Steps.
        /// </summary>
        /// <param name="dal">The data access layer implementation for the orchestrator.</param>
        /// <param name="portal">The child data portal for managing the Steps collection.</param>

        [Update]
        private async Task UpdateAsync(
            [Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                // Create the DTO, including the current TimeStamp for concurrency checking
                var dto = new CustomerOnboardingOrchestratorDto
                {
                    TenantId = this.TenantId,
                    CurrentStepIndex = this.CurrentStepIndex,
                    LastChanged = this.TimeStamp // Pass the current timestamp for concurrency check in DAL
                };

                // Update the orchestrator data (DAL should handle concurrency check)
                dal.Update(dto);
                // Update the object's timestamp with the new value from the DAL
                TimeStamp = dto.LastChanged;



                // Cascade the update operation to the child Steps collection
                await portal.UpdateChildAsync(Steps, this); // Pass 'this' as the parent context
            }
        }

        /// <summary>
        /// DataPortal_Fetch method. Retrieves an existing CustomerOnboardingOrchestrator instance and its child Steps based on the Tenant ID.
        /// </summary>
        /// <param name="tenantId">The unique identifier for the onboarding session to fetch.</param>
        /// <param name="dal">The data access layer implementation for the orchestrator.</param>
        /// <param name="portal">The child data portal for managing the Steps collection.</param>

        [Fetch]
        private async Task FetchAsync(
            string tenantId,
            [Inject] ICustomerOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                // Fetch the orchestrator's core data
                var dto = dal.Fetch(tenantId); // DAL should throw DataNotFoundException if not found
                // Load properties from the DTO
                TenantId = dto.TenantId;
                CurrentStepIndex = dto.CurrentStepIndex;
                TimeStamp = dto.LastChanged;
                // Fetch the child Steps collection. The Steps.FetchChildAsync method
                // should handle fetching and creating individual IStep objects using the StepFactory.
                Steps = await portal.FetchChildAsync(TenantId,CurrentStepIndex);
            }
            // Check rules after fetching (e.g., IsComplete)
            await BusinessRules.CheckRulesAsync();
        }

        #endregion
    }
}
