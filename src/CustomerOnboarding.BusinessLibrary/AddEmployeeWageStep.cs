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
    public class AddEmployeeWageStep :
        StepBase<AddEmployeeWageStep>
    {
        public static readonly PropertyInfo<string> RuleSetProperty =
          RegisterProperty<string>(nameof(RuleSet));

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

        public static readonly PropertyInfo<EmployeeWage> EmployeeWageProperty =
           RegisterProperty<EmployeeWage>(nameof(EmployeeWage));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public EmployeeWage EmployeeWage
        {
            get => GetProperty(EmployeeWageProperty);
            private set => LoadProperty(EmployeeWageProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(EmployeeWageProperty, IsCompletedProperty));
            // BusinessRules.AddRule(new CaptureTheWageIdCurrentlyBeingEdited(WageProperty, CurrentWageIdBeingEditedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is EmployeeWage|| e.ChildObject is IEmployeeEarningOrAccrualType
                || e.ChildObject is DateSplitter)
                BusinessRules.CheckRules(EmployeeWageProperty);
            base.OnChildChanged(e);
        }


        [CreateChild]
        private async Task CreateAsync(WageInfo info,
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
                StepIndex = 2;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "Default";
                IsCompleted = false;
                EmployeeWage = await portal.GetPortal<EmployeeWage>().CreateChildAsync(info,RuleSet);

            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployeeWageStepDal dal,
            [Inject] IChildDataPortal<EmployeeWage> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeeWageStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    WageId=EmployeeWage.Id,
                    EmployeeId = ((Steps)Parent.Parent.Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                                        .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                                        .EmployeeEmploymentDetails.EmployeeId!,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    // CurrentWageIdBeingEdited = this.CurrentWageIdBeingEdited
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                if (parent.CurrentStepIndex == ((EmployeesStep)Parent.Parent.Parent.Parent.Parent.Parent).StepIndex
                                 && (((EmployeesStep)Parent.Parent.Parent.Parent.Parent).CurrentStepIndex)
                                 == ((ManualEmployeeDataInputMethodStep)Parent.Parent.Parent.Parent).StepIndex &&
                                 (((EmployeeWagesStep)Parent.Parent).CurrentStepIndex - 1) == StepIndex &&
                                 IsCompleted)
                    await portal.UpdateChildAsync(EmployeeWage, parent);
            }
        }

        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id, int currentStepIndex,
          [Inject] IAddEmployeeWageStepDal dal,
           [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = data.StepIndex;
                TimeStamp = data.LastChanged;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "Default";

                IsCompleted = data.IsCompleted;
                EmployeeWage =await portal.GetPortal<EmployeeWage>().FetchChildAsync(tenantId,data.EmployeeId,data.WageId,RuleSet);
                



            }


            await BusinessRules.CheckRulesAsync();
        }

        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,
           [Inject] IAddEmployeeWageStepDal dal,
           [Inject] IChildDataPortal<EmployeeWage> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeeWageStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    WageId = EmployeeWage.Id,
                    EmployeeId = ((Steps)Parent.Parent.Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                                        .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                                        .EmployeeEmploymentDetails.EmployeeId!,
                    LastChanged = this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

                if (parent.CurrentStepIndex == ((EmployeesStep)Parent.Parent.Parent.Parent.Parent.Parent).StepIndex
                   && (((EmployeesStep)Parent.Parent.Parent.Parent.Parent).CurrentStepIndex)
                   == ((ManualEmployeeDataInputMethodStep)Parent.Parent.Parent.Parent).StepIndex &&
                   (((EmployeeWagesStep)Parent.Parent).CurrentStepIndex-1)==StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(EmployeeWage, parent);
            }
        }
    }
}
