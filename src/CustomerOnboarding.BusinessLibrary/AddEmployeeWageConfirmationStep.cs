using Csla;
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
    public class AddEmployeeWageConfirmationStep :
        StepBase<AddEmployeeWageConfirmationStep>,IConfirmationAction
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

        public static readonly PropertyInfo<ConfirmationActions> ActionTakenProperty =
           RegisterProperty<ConfirmationActions>(nameof(ActionTaken));

        public ConfirmationActions ActionTaken
        {
            get => GetProperty(ActionTakenProperty);
            set => SetProperty(ActionTakenProperty, value);
        }



        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new CheckIfActionOrientedStepIsComplete(ActionTakenProperty, IsCompletedProperty));
        }


        [CreateChild]
        private void Create(
          int id, int currentStepIndex,
         [Inject] IStepTypeDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = 2;
                ActionTaken = ConfirmationActions.NoActionTakenYet;
                IsCompleted = false;

            }

        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployeeWageConfirmationStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeeWageConfirmationStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    WageId = ((Steps)Parent).Where(r=>r is SelectEmployeeWageStep)
                                           .Select(r=>(SelectEmployeeWageStep)r)
                                           .FirstOrDefault()?
                                           .WageId!
                                           ,
                    EmployeeId = ((Steps)Parent.Parent.Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                                        .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                                        .EmployeeEmploymentDetails.EmployeeId!,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                    ActionTaken = (int)this.ActionTaken
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

            }
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployeeWageConfirmationStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeeWageConfirmationStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                    WageId = ((Steps)Parent).Where(r => r is SelectEmployeeWageStep)
                                           .Select(r => (SelectEmployeeWageStep)r)
                                           .FirstOrDefault()?
                                           .WageId!
                                           ,
                    EmployeeId = ((Steps)Parent.Parent.Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                                        .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                                        .EmployeeEmploymentDetails.EmployeeId!,
                    ActionTaken = (int)this.ActionTaken,
                    LastChanged = this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

            }
        }

        [FetchChild]
        private async Task FetchAsync(
          string tenantId, int id, int currentStepIndex,string employeeId,
          string wageId,
         [Inject] IAddEmployeeWageConfirmationStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id,employeeId,wageId);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                ActionTaken = (ConfirmationActions)Enum.Parse(typeof(ConfirmationActions), data.ActionTaken.ToString());
                StepIndex = data.StepIndex;
                TimeStamp = data.LastChanged;
                IsCompleted = data.IsCompleted;

            }

            await BusinessRules.CheckRulesAsync();
        }

    }
}
