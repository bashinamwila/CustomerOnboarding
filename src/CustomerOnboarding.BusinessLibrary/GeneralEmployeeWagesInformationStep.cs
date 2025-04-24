using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
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
    public class GeneralEmployeeWagesInformationStep :
        StepBase<GeneralEmployeeWagesInformationStep>,IInformationOnlyStep
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

        public void MarkAsCompleted() => IsCompleted = true;

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
        }

        [CreateChild]
        private void Create(
            int id, int currentStepIndex,
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
           [Inject] IGeneralEmployeeWagesInformationStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new GeneralEmployeeWagesInformationStepDto
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
          string tenantId, int id, int currentStepIndex,
        [Inject] IGeneralEmployeeWagesInformationStepDal dal)
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
           [Inject] IGeneralEmployeeWagesInformationStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new GeneralEmployeeWagesInformationStepDto
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
