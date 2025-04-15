using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class CompensationComponentsStep :
        StepBase<CompensationComponentsStep>,IIskippable,IOnboardingOrchestrator
    {
        public static readonly PropertyInfo<byte[]> TimeStampProperty =
           RegisterProperty<byte[]>(nameof(TimeStamp));

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
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


        public void Skip() => throw new NotImplementedException();


        public static readonly PropertyInfo<bool> SkippedProperty =
          RegisterProperty<bool>(nameof(Skipped));

        /// <summary>
        /// Indicates whether all steps in the workflow have been completed.
        /// This is managed by the CheckIfWorkflowIsComplete business rule.
        /// </summary>
        public bool Skipped
        {
            get => GetProperty(SkippedProperty);
            set => SetProperty(SkippedProperty, value); // Rule sets this
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
                else if (step.Type == StepTypes.ConfirmationAction)
                {
                    if (step.IsCompleted && currentStepIndexBeforeUpdate == step.StepIndex)
                    {
                        if(step is IConfirmationAction confirmation)
                        {
                            if (confirmation.ActionTaken == ConfirmationActions.IHaveAddedAllTheCurrentItems)
                            {
                               
                                if ((CurrentStepIndex + 1) <= Steps.Count - 1)
                                    CurrentStepIndex++;
                                else
                                    break;
                            }
                            else
                            {
                               
                            }
                        }
                      
                        else
                        {
                            break;
                        }
                    }
                    else if (!step.IsCompleted && currentStepIndexBeforeUpdate == step.StepIndex)
                    {
                        if (step is IConfirmationAction confirmationAction)
                        {
                            if (confirmationAction.ActionTaken == ConfirmationActions.AddAnotherItem)
                            {

                                //await confirmation.AddAnotherItemAsync();
                                if ((CurrentStepIndex - 1) >= 0)
                                {
                                    CurrentStepIndex--;
                                    confirmationAction.ActionTaken = ConfirmationActions.NoActionTakenYet;
                                }

                                else
                                    break;
                            }
                            else
                            {
                                break;
                            }
                        }
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

            // Step is complete only if both child objects are valid
           // BusinessRules.AddRule(new MarkItemAdditionStepsComplete(StepsProperty) { Priority = 0 });
            BusinessRules.AddRule(new CheckIfMultiStepIsComplete(StepsProperty, IsCompletedProperty) { Priority=2});

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
          

            

            if (e.ChildObject is IStep || e.ChildObject is Steps
              || e.ChildObject is Wage)
                BusinessRules.CheckRules(StepsProperty);

            base.OnChildChanged(e);
        }


        [CreateChild]
        private async Task CreateAsync(
            int id,
            int currentStepIndex,
          [Inject] IStepTypeDal dal,
          [Inject] IDataPortalFactory factory,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = 3;
                IsCompleted = false;
                CurrentStepIndex = 0;
                Skipped = false;
                var _factory = await factory.GetPortal<CompensationComponentsStepStepsFactory>().FetchAsync(new[] { 10,11,12 }, CurrentStepIndex);
                Steps = _factory.Steps;
            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task Insert(TenantOnboardingOrchestrator parent,
          [Inject] ICompensationComponentsStepDal dal,
          [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new CompensationComponentsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                    CurrentStepIndex = this.CurrentStepIndex,
                    Skip=this.Skipped
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                await portal.UpdateChildAsync(Steps, parent);

            }
        }


        [UpdateChild]
        private async Task Update(TenantOnboardingOrchestrator parent,
         [Inject] ICompensationComponentsStepDal dal,
         [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new CompensationComponentsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                    CurrentStepIndex = this.CurrentStepIndex,
                    Skip = this.Skipped,
                    LastChanged=this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

                await portal.UpdateChildAsync(Steps, parent);

            }
        }


        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] ICompensationComponentsStepDal dal,
          [Inject] IDataPortalFactory factory,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = data.StepIndex;
                IsCompleted = data.IsCompleted;
                CurrentStepIndex = data.CurrentStepIndex;
                Skipped = data.Skip;
                TimeStamp = data.LastChanged;
                var _factory = await factory.GetPortal<CompensationComponentsStepStepsFactory>().FetchAsync(tenantId, CurrentStepIndex);
                Steps = _factory.Steps;

            }



            await BusinessRules.CheckRulesAsync();
        }


    }
}
