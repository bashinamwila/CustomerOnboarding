using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Multitenancy;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class TenantOnboardingOrchestrator :
        BusinessBase<TenantOnboardingOrchestrator>,IOnboardingOrchestrator
    {
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

        public void Skip()
        {
            var step = Steps[CurrentStepIndex];
            if (step is IIskippable skippable)
            {
                if (skippable.Skipped)
                {

                    if ((CurrentStepIndex + 1) <= Steps.Count - 1)
                        CurrentStepIndex++;
                }
            }   
        }

        public async Task MoveNextAsync()
        {
            var currentStepIndexBeforeUpdate = CurrentStepIndex;
            
            
            
                while (CurrentStepIndex < Steps.Count)
                {
                    var step = Steps[CurrentStepIndex];
                    if (step is IInformationOnlyStep informationOnlyStep)
                        informationOnlyStep.MarkAsCompleted();

               

                if (!step.IsCompleted && step.Type == StepTypes.Automatic)
                    {
                     
                    }
                    else if (step.Type == StepTypes.Manual)
                    {
                        if (step.IsCompleted && currentStepIndexBeforeUpdate == step.StepIndex)
                        {
                        

                                if ((CurrentStepIndex + 1) <= Steps.Count - 1)
                                    CurrentStepIndex++;
                                else
                                    break;

                          
                        }
                        else
                        {
                                break;
                       
                        }
                    }
                    else if (step.Type == StepTypes.MultiStep)
                    {
                        if (!step.IsCompleted && currentStepIndexBeforeUpdate == step.StepIndex)
                        {

                           
                                var multiStep = (IOnboardingOrchestrator)step;
                                 await multiStep.MoveNextAsync();
                                break;
                            

                                                   

                        }
                        else if (step.IsCompleted && currentStepIndexBeforeUpdate == step.StepIndex)
                         {

                                if ((CurrentStepIndex + 1) <= Steps.Count - 1)
                                    CurrentStepIndex++;
                                else
                                    break;
                        }
                        else
                        {
                                break;
                        }
                           
                        
                    }
                   
                    else
                {

                    break;
                }
                }
            }
           
        

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Rule: The workflow is complete (IsComplete = true) only if all steps in the Steps collection are complete.
            BusinessRules.AddRule(new CheckIfWorkflowIsComplete(StepsProperty, IsCompleteProperty));

            // Dependency: Ensure the IsComplete property is re-evaluated whenever the Steps collection or its items change.
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(StepsProperty, IsCompleteProperty));
        }

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


        [Create]
        private async Task CreateAsync([Inject]TenantInfo tenant,
            [Inject]IChildDataPortalFactory portal,
            [Inject]IDataPortal<TenantOnboardingStepsFactory>factory,
            [Inject]ILogger<TenantOnboardingOrchestrator>logger)
        {
            using (BypassPropertyChecks)
            {
                IsComplete = false;
                TenantId = tenant.Id;
                CurrentStepIndex = 0;
                var creator = await factory.FetchAsync(new int[] { 4, 5, 6, 9 }, CurrentStepIndex);
                Steps = creator.Steps;
               
                
            }
            await BusinessRules.CheckRulesAsync();
        }

        [Create]
        private async Task CreateAsync(string tenantId,
           [Inject] IChildDataPortalFactory portal,
           [Inject] IDataPortal<TenantOnboardingStepsFactory> factory,
           [Inject] ILogger<TenantOnboardingOrchestrator> logger)
        {
            using (BypassPropertyChecks)
            {
                IsComplete = false;
                TenantId = tenantId;
                CurrentStepIndex = 0;
                var creator = await factory.FetchAsync(new int[] { 4, 5, 6,9 }, CurrentStepIndex);
                Steps = creator.Steps;


            }
            await BusinessRules.CheckRulesAsync();
        }

        [Insert]
        private async Task InsertAsync([Inject]ITenantOnboardingOrchestratorDal dal,
            [Inject]IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                // Create the DTO for the orchestrator itself
                var dto = new OnboardingOrchestratorDto
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

        [Fetch]
        private async Task FetchAsync(
            string tenantId,
            [Inject] ITenantOnboardingOrchestratorDal dal,
            [Inject] IDataPortal<TenantOnboardingStepsFactory> portal)
        {
            using (BypassPropertyChecks)
            {
                // Fetch the orchestrator's core data
                var dto = dal.Fetch(tenantId); // DAL should throw DataNotFoundException if not found
                // Load properties from the DTO
                TenantId = dto.TenantId;
                CurrentStepIndex = dto.CurrentStepIndex;
                TimeStamp = dto.LastChanged;
                var factory=await portal.FetchAsync(tenantId,CurrentStepIndex);
                Steps=factory.Steps;
            }
            // Check rules after fetching (e.g., IsComplete)
            await BusinessRules.CheckRulesAsync();
        }

        [Update]
        private async Task UpdateAsync([Inject] ITenantOnboardingOrchestratorDal dal,
            [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                // Create the DTO for the orchestrator itself
                var dto = new OnboardingOrchestratorDto
                {
                    TenantId = this.TenantId,
                    CurrentStepIndex = this.CurrentStepIndex,
                    LastChanged=this.TimeStamp
                    // TimeStamp will be set by the DAL upon insertion
                };
                // Persist the orchestrator data
                dal.Update(dto);
                // Update the object's timestamp with the value returned from the DAL
                TimeStamp = dto.LastChanged;

                // Cascade the insert/update operation to the child Steps collection
                // This will trigger the appropriate InsertChild/UpdateChild methods on each IStep

                await portal.UpdateChildAsync(Steps, this); // Pass 'this' as the parent context
            }
        }
    }


}
