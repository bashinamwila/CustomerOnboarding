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
    public class AddDeductionStep :
        StepBase<AddDeductionStep>, IItemAdditionStep
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

        public static readonly PropertyInfo<Deduction> DeductionProperty =
           RegisterProperty<Deduction>(nameof(Deduction));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public Deduction Deduction
        {
            get => GetProperty(DeductionProperty);
            private set => LoadProperty(DeductionProperty, value);
        }

        public void MarkAsComplete() => IsCompleted = true;

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(DeductionProperty, IsCompletedProperty));
            // BusinessRules.AddRule(new CaptureTheWageIdCurrentlyBeingEdited(WageProperty, CurrentWageIdBeingEditedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is Deduction || e.ChildObject is IDeductionType
                || e.ChildObject is ILimit)
                BusinessRules.CheckRules(DeductionProperty);
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
                Deduction = await portal.GetPortal<Deduction>().CreateChildAsync(RuleSet);

            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddDeductionsStepDal dal,
            [Inject] IChildDataPortal<Deduction> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddDeductionsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    // CurrentWageIdBeingEdited = this.CurrentWageIdBeingEdited
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                if (parent.CurrentStepIndex == ((DeductionsStep)Parent.Parent).StepIndex
                   && (((DeductionsStep)Parent.Parent).CurrentStepIndex - 1) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(Deduction, parent);
            }
        }


        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] IAddDeductionsStepDal dal,
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
                Deduction = await portal.GetPortal<Deduction>().CreateChildAsync(RuleSet);



            }


            await BusinessRules.CheckRulesAsync();
        }


        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddDeductionsStepDal dal,
            [Inject] IChildDataPortal<Deduction> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddDeductionsStepDto
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

                if (parent.CurrentStepIndex == ((DeductionsStep)Parent.Parent).StepIndex
                   && (((DeductionsStep)Parent.Parent).CurrentStepIndex - 1) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(Deduction, parent);
            }
        }

    }
}
