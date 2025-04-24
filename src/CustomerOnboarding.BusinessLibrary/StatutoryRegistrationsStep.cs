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
    public class StatutoryRegistrationsStep :
        StepBase<StatutoryRegistrationsStep>

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

        public static readonly PropertyInfo<StatutoryRegistrations> StatutoryRegistrationsProperty =
          RegisterProperty<StatutoryRegistrations>(nameof(StatutoryRegistrations));


        public StatutoryRegistrations StatutoryRegistrations
        {
            get => GetProperty(StatutoryRegistrationsProperty);
            private set => LoadProperty(StatutoryRegistrationsProperty, value);
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

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(StatutoryRegistrationsProperty, IsCompletedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is StatutoryRegistrations)
                BusinessRules.CheckRules(StatutoryRegistrationsProperty);
            base.OnChildChanged(e);
        }



        [CreateChild]
        private async Task CreateAsync(int id,int currentStepIndex,
             [Inject] IStepTypeDal dal,
            [Inject] IChildDataPortal<StatutoryRegistrations> portal)
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
                StatutoryRegistrations = await portal.CreateChildAsync(RuleSet);
            }
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IStatutoryRegistrationsStepDal dal,
            [Inject] IChildDataPortal<StatutoryRegistrations> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new StatutoryRegistrationsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
                

                  if((parent.CurrentStepIndex-1)==((ComplianceInfoStep)Parent.Parent).StepIndex
                    && ((ComplianceInfoStep)Parent.Parent).CurrentStepIndex==StepIndex
                    && IsCompleted)
                        await portal.UpdateChildAsync(StatutoryRegistrations, parent);
            }
        }


        [FetchChild]
        private async Task FetchAsync(
           string tenantId,int id,int currentStepIndex,
          [Inject] IStatutoryRegistrationsStepDal dal,
          [Inject] IStatutoryRegistrationsDal dalStatutoryRegistrations,
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
                    RuleSet = "";

                IsCompleted = data.IsCompleted;
                if (dalStatutoryRegistrations.Exists(tenantId))
                    StatutoryRegistrations = await portal.GetPortal<StatutoryRegistrations>().FetchChildAsync(tenantId, RuleSet);

                else
                    StatutoryRegistrations = await portal.GetPortal<StatutoryRegistrations>().CreateChildAsync(RuleSet);
            }

            if (currentStepIndex == StepIndex)
                await BusinessRules.CheckRulesAsync();
        }

        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,
            [Inject] IStatutoryRegistrationsStepDal dal,
            [Inject] IChildDataPortal<StatutoryRegistrations> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new StatutoryRegistrationsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted, // Only mark as completed if it's the current step
                    LastChanged=this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;


                if ((parent.CurrentStepIndex-1) == ((ComplianceInfoStep)Parent.Parent).StepIndex
                   && ((ComplianceInfoStep)Parent.Parent).CurrentStepIndex == StepIndex &&
                   IsCompleted)
                    await portal.UpdateChildAsync(StatutoryRegistrations, parent);
            }
        }

    }
}
