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
using System.Diagnostics.Metrics;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class AddEmployeePersonalDetailsStep :
        StepBase<AddEmployeePersonalDetailsStep>
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

        public static readonly PropertyInfo<EmployeePersonalDetails> EmployeePersonalDetailsProperty =
           RegisterProperty<EmployeePersonalDetails>(nameof(EmployeePersonalDetails));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public EmployeePersonalDetails EmployeePersonalDetails
        {
            get => GetProperty(EmployeePersonalDetailsProperty);
            private set => LoadProperty(EmployeePersonalDetailsProperty, value);
        }

        public static readonly PropertyInfo<int> CounterProperty =
            RegisterProperty<int>(nameof(Counter));
        public int Counter
        {
            get => GetProperty(CounterProperty);
            private set => LoadProperty(CounterProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(EmployeePersonalDetailsProperty, IsCompletedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is EmployeePersonalDetails || e.ChildObject is DateSplitter)
                BusinessRules.CheckRules(EmployeePersonalDetailsProperty);
            base.OnChildChanged(e);
        }

        [CreateChild]
        private async Task CreateAsync(
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
                StepIndex = 2;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "";
                IsCompleted = false;
                EmployeePersonalDetails = await portal.GetPortal<EmployeePersonalDetails>().CreateChildAsync(RuleSet);

            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployeePersonalDetailsStepDal dal,
            [Inject] IChildDataPortal<EmployeePersonalDetails> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeePersonalDetailsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    EmployeeId=((Steps)Parent).Where(r=>r is AddEmployeeEmploymentDetailsStep)
                                        .Select(r=>(AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()!
                                        .EmployeeEmploymentDetails.EmployeeId
                   
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
                Counter = dto.Counter;

                if (((EmployeesStep)Parent.Parent.Parent.Parent).CurrentStepIndex == ((ManualEmployeeDataInputMethodStep)Parent.Parent).StepIndex
                    && (((ManualEmployeeDataInputMethodStep)Parent.Parent).CurrentStepIndex) == StepIndex &&
                    IsCompleted)
                    await portal.UpdateChildAsync(EmployeePersonalDetails, parent);
            }
        }


        [FetchChild]
        private async Task FetchAsync(
           string tenantId,int id,int currentStepIndex,int counter,
          [Inject] IAddEmployeePersonalDetailsStepDal dal,
           [Inject] IDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId,id,counter);
                Id = data.StepId;
                Counter = data.Counter;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = data.StepIndex;
                IsCompleted = data.IsCompleted;
                TimeStamp = data.LastChanged;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "";

                
                EmployeePersonalDetails = portal.GetPortal<EmployeePersonalDetailsFactory>().Fetch(tenantId,data.EmployeeId,RuleSet).Result;



            }


            await BusinessRules.CheckRulesAsync();
        }


        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployeePersonalDetailsStepDal dal,
            [Inject] IChildDataPortal<EmployeePersonalDetails> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployeePersonalDetailsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    Counter=this.Counter,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    EmployeeId = ((Steps)Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                        .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()!
                                        .EmployeeEmploymentDetails.EmployeeId,
                    LastChanged = this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

                if (((EmployeesStep)Parent.Parent.Parent.Parent).CurrentStepIndex == ((ManualEmployeeDataInputMethodStep)Parent.Parent).StepIndex
                   && (((ManualEmployeeDataInputMethodStep)Parent.Parent).CurrentStepIndex) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(EmployeePersonalDetails, parent);
            }
        }
    }
}
