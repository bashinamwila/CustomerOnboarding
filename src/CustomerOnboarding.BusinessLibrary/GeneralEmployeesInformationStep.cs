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
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class GeneralEmployeesInformationStep :
        StepBase<GeneralEmployeesInformationStep>,IInformationOnlyStep
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

         public static readonly PropertyInfo<int> InputMethodProperty =
            RegisterProperty<int>(nameof(InputMethod));
        public int InputMethod
        {
            get => GetProperty(InputMethodProperty);
            set => SetProperty(InputMethodProperty, value);
        }

       
        public void SetInputMethod(int id)
        {
            InputMethod = id;
            BusinessRules.CheckRules(InputMethodProperty);
        }

        public void MarkAsCompleted() => IsCompleted = true;
        

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            // BusinessRules.AddRule(new MarkItemAdditionStepsComplete(StepsProperty) { Priority = 0 });
            BusinessRules.AddRule(new SetEmployeeDataInputMethod(InputMethodProperty));

        }

        [CreateChild]
        private void Create(
            int id,int currentStepIndex,
           [Inject] IStepTypeDal dal,
             [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = 0;
                IsCompleted = false;

            }

        }


        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
           [Inject] IGeneralEmployeesInformationDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new GeneralEmployeesInformationDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

            }
        }

        [FetchChild]
        private async Task FetchAsync(
           string tenantId,int id,int currentStepIndex,
         [Inject] IGeneralEmployeesInformationDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = data.StepIndex;
                TimeStamp = data.LastChanged;
                IsCompleted = data.IsCompleted;

            }

            await BusinessRules.CheckRulesAsync();
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent,
            [Inject] IGeneralEmployeesInformationDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new GeneralEmployeesInformationDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    LastChanged = this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

            }
        }
    }
}
