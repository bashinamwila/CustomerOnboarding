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
    public class AddCompensationComponentStep :
        StepBase<AddCompensationComponentStep>,IItemAdditionStep
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

        public static readonly PropertyInfo<Wage> WageProperty =
           RegisterProperty<Wage>(nameof(Wage));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public Wage Wage
        {
            get => GetProperty(WageProperty);
            private set => LoadProperty(WageProperty, value);
        }

        public void MarkAsComplete() => IsCompleted = true;    

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(WageProperty, IsCompletedProperty));
           // BusinessRules.AddRule(new CaptureTheWageIdCurrentlyBeingEdited(WageProperty, CurrentWageIdBeingEditedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is Wage)
                BusinessRules.CheckRules(WageProperty);
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
                Wage = await portal.GetPortal<Wage>().CreateChildAsync(RuleSet);
               
            }
          
                await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddCompensationComponentStepDal dal,
            [Inject] IChildDataPortal<Wage> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddCompensationComponentStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                   // CurrentWageIdBeingEdited = this.CurrentWageIdBeingEdited
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                if (parent.CurrentStepIndex == ((CompensationComponentsStep)Parent.Parent).StepIndex
                   && (((CompensationComponentsStep)Parent.Parent).CurrentStepIndex-1) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(Wage, parent);
            }
        }


        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] IAddCompensationComponentStepDal dal,
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
                Wage = await portal.GetPortal<Wage>().CreateChildAsync(RuleSet);
               

                
            }

            
                await BusinessRules.CheckRulesAsync();
        }


        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddCompensationComponentStepDal dal,
            [Inject] IChildDataPortal<Wage> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddCompensationComponentStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                 //   CurrentWageIdBeingEdited = this.CurrentWageIdBeingEdited,
                    LastChanged=this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

                if (parent.CurrentStepIndex == ((CompensationComponentsStep)Parent.Parent).StepIndex
                   && (((CompensationComponentsStep)Parent.Parent).CurrentStepIndex-1) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(Wage, parent);
            }
        }

    }
}
