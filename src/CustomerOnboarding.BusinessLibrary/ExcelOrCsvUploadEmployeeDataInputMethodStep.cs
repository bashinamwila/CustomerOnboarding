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

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class ExcelOrCsvUploadEmployeeDataInputMethodStep :
        StepBase<ExcelOrCsvUploadEmployeeDataInputMethodStep>,IEmployeeDataInputMethodStep
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


        public static readonly PropertyInfo<bool> IsSkippedProperty =
          RegisterProperty<bool>(nameof(IsSkipped));

        /// <summary>
        /// Indicates whether all steps in the workflow have been completed.
        /// This is managed by the CheckIfWorkflowIsComplete business rule.
        /// </summary>
        public bool IsSkipped
        {
            get => GetProperty(IsSkippedProperty);
            set => SetProperty(IsSkippedProperty, value); // Rule sets this
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
                        if (step is IConfirmationAction confirmation)
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
            BusinessRules.AddRule(new CheckIfMultiStepIsComplete(StepsProperty, IsCompletedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {




            if (e.ChildObject is IStep || e.ChildObject is Steps
              || e.ChildObject is Deduction)
                BusinessRules.CheckRules(StepsProperty);

            base.OnChildChanged(e);
        }

    }
}
