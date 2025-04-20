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
    public class AddEmployerExpenseStep :
        StepBase<AddEmployerExpenseStep>, IItemAdditionStep
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

        public static readonly PropertyInfo<EmployerExpense> EmployerExpenseProperty =
           RegisterProperty<EmployerExpense>(nameof(EmployerExpense));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public EmployerExpense EmployerExpense
        {
            get => GetProperty(EmployerExpenseProperty);
            private set => LoadProperty(EmployerExpenseProperty, value);
        }

        public void MarkAsComplete() => IsCompleted = true;

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(EmployerExpenseProperty, IsCompletedProperty));
            // BusinessRules.AddRule(new CaptureTheWageIdCurrentlyBeingEdited(WageProperty, CurrentWageIdBeingEditedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is EmployerExpense || e.ChildObject is IEmployerExpenseType
                || e.ChildObject is ILimit)
                BusinessRules.CheckRules(EmployerExpenseProperty);
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
                EmployerExpense = await portal.GetPortal<EmployerExpense>().CreateChildAsync(RuleSet);

            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployerExpensesStepDal dal,
            [Inject] IChildDataPortal<EmployerExpense> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployerExpensesStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    // CurrentWageIdBeingEdited = this.CurrentWageIdBeingEdited
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                if (parent.CurrentStepIndex == ((EmployerExpensesStep)Parent.Parent).StepIndex
                   && (((EmployerExpensesStep)Parent.Parent).CurrentStepIndex - 1) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(EmployerExpense, parent);
            }
        }


        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] IAddEmployerExpensesStepDal dal,
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
                EmployerExpense = await portal.GetPortal<EmployerExpense>().CreateChildAsync(RuleSet);



            }


            await BusinessRules.CheckRulesAsync();
        }


        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,
            [Inject] IAddEmployerExpensesStepDal dal,
            [Inject] IChildDataPortal<EmployerExpense> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new AddEmployerExpensesStepDto
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

                if (parent.CurrentStepIndex == ((EmployerExpensesStep)Parent.Parent).StepIndex
                   && (((EmployerExpensesStep)Parent.Parent).CurrentStepIndex - 1) == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(EmployerExpense, parent);
            }
        }

    }
}

