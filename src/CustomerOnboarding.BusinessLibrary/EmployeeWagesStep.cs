using Csla.Core;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeeWagesStep :
        StepBase<EmployeeWagesStep>,IOnboardingOrchestrator
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


        private void GoTo(int numberOfStepsForwardOrBackward)
        {
            if (numberOfStepsForwardOrBackward > 0)
                if ((CurrentStepIndex + numberOfStepsForwardOrBackward) <= Steps.Count - 1)
                    CurrentStepIndex = CurrentStepIndex + numberOfStepsForwardOrBackward;

            if (numberOfStepsForwardOrBackward < 0)
                if ((CurrentStepIndex - numberOfStepsForwardOrBackward) >= 0)
                    CurrentStepIndex = CurrentStepIndex + numberOfStepsForwardOrBackward;

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
                            GoTo(1);
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
                            GoTo(1);
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
                        if (step is IConfirmationAction confirmation)
                        {
                            if (confirmation.ActionTaken == ConfirmationActions.IHaveAddedAllTheCurrentItems)
                            {

                                if ((CurrentStepIndex + 1) <= Steps.Count - 1)
                                    GoTo(1);
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
                                    GoTo(-1);
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
            BusinessRules.AddRule(new CheckIfMultiStepIsComplete(StepsProperty, IsCompletedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {




            if (e.ChildObject is IStep || e.ChildObject is Steps)
                BusinessRules.CheckRules(StepsProperty);

            base.OnChildChanged(e);
        }


        [CreateChild]
        private async Task CreateAsync(
           int id, int currentStepIndex,
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
                var _factory = await factory.GetPortal<EmployeeWagesStepStepsFactory>().FetchAsync(new[] { 28}, CurrentStepIndex);
                Steps = _factory.Steps;
            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task Insert(TenantOnboardingOrchestrator parent,
         [Inject] IEmployeeWagesStepDal dal,
         [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new EmployeeWagesStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    EmployeeId=((Steps)Parent).Where(r=>r is AddEmployeeEmploymentDetailsStep)
                                .Select(r=>(AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                .EmployeeEmploymentDetails.EmployeeId!,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                    CurrentStepIndex = this.CurrentStepIndex,

                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                await portal.UpdateChildAsync(Steps, parent);

            }
        }


        [UpdateChild]
        private async Task Update(TenantOnboardingOrchestrator parent,
         [Inject] IEmployeeWagesStepDal dal,
         [Inject] IChildDataPortal<Steps> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new EmployeeWagesStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    EmployeeId = ((Steps)Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                .EmployeeEmploymentDetails.EmployeeId!,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                    CurrentStepIndex = this.CurrentStepIndex,
                    LastChanged = this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

                await portal.UpdateChildAsync(Steps, parent);

            }
        }


        [FetchChild]
        private async Task FetchAsync(
          string tenantId, int id,string employeeId, int currentStepIndex,
          [Inject] IEmployeeWagesStepDal dal,
          [Inject] IDataPortalFactory factory,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id,employeeId);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = data.StepIndex;
                IsCompleted = data.IsCompleted;
                CurrentStepIndex = data.CurrentStepIndex;
                TimeStamp = data.LastChanged;
                var _factory = await factory.GetPortal<EmployeeWagesStepStepsFactory>().FetchAsync(tenantId,employeeId, CurrentStepIndex);
                Steps = _factory.Steps;

            }



            await BusinessRules.CheckRulesAsync();
        }
    }
}
