using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class AddEmployeeEmploymentDetailsStep :
        StepBase<AddEmployeeEmploymentDetailsStep>
    {
        public static readonly PropertyInfo<string> RuleSetProperty =
            RegisterProperty<string>(nameof(RuleSet));

        /// <summary>
        /// Name of the rule set used to validate this step's children.
        /// </summary>
        public string RuleSet
        {
            get => GetProperty(RuleSetProperty);
            private set => LoadProperty(RuleSetProperty, value);
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

        public static readonly PropertyInfo<EmployeeEmploymentDetails> EmployeeEmploymentDetailsProperty =
           RegisterProperty<EmployeeEmploymentDetails>(nameof(EmployeeEmploymentDetails));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public EmployeeEmploymentDetails EmployeeEmploymentDetails
        {
            get => GetProperty(EmployeeEmploymentDetailsProperty);
            private set => LoadProperty(EmployeeEmploymentDetailsProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(EmployeeEmploymentDetailsProperty, IsCompletedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is EmployeeEmploymentDetails || e.ChildObject is IEmployeeStatus
                || e.ChildObject is IEmployeeType || e.ChildObject is DateSplitter)
                BusinessRules.CheckRules(EmployeeEmploymentDetailsProperty);
            base.OnChildChanged(e);
        }


        [CreateChild]
        private async Task CreateAsync(
           int id,
           int currentStepIndex,
         [Inject] IStepTypeDal dal,
           [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = 1;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "";
                IsCompleted = false;
                EmployeeEmploymentDetails = await portal.GetPortal<EmployeeEmploymentDetails>().CreateChildAsync(RuleSet);

            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployeeEmploymentDetailsStepDal dal,
            [Inject] IChildDataPortal<EmployeeEmploymentDetails> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeeEmploymentDetailsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    // CurrentWageIdBeingEdited = this.CurrentWageIdBeingEdited
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                if (((EmployeesStep)Parent.Parent.Parent.Parent).CurrentStepIndex == ((ManualEmployeeDataInputMethodStep)Parent.Parent).StepIndex
                   && (((ManualEmployeeDataInputMethodStep)Parent.Parent).CurrentStepIndex) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(EmployeeEmploymentDetails, parent);
            }
        }


        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] IAddEmployeeEmploymentDetailsStepDal dal,
           [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = data.StepIndex;
                // CurrentWageIdBeingEdited = data.CurrentWageIdBeingEdited;
                TimeStamp = data.LastChanged;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "";

                IsCompleted = data.IsCompleted;
                EmployeeEmploymentDetails = await portal.GetPortal<EmployeeEmploymentDetails>().CreateChildAsync(RuleSet);



            }


            await BusinessRules.CheckRulesAsync();
        }


        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployeeEmploymentDetailsStepDal dal,
            [Inject] IChildDataPortal<EmployeeEmploymentDetails> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeeEmploymentDetailsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    //   CurrentWageIdBeingEdited = this.CurrentWageIdBeingEdited,
                    LastChanged = this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

                if (((EmployeesStep)Parent.Parent.Parent.Parent).CurrentStepIndex == ((ManualEmployeeDataInputMethodStep)Parent.Parent).StepIndex
                   && (((ManualEmployeeDataInputMethodStep)Parent.Parent).CurrentStepIndex-1) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(EmployeeEmploymentDetails, parent);
            }
        }
    }
}
