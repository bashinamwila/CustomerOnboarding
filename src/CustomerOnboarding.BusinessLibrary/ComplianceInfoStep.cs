using Csla.Core;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal.Dtos;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class ComplianceInfoStep :
        StepBase<ComplianceInfoStep>
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

        public static readonly PropertyInfo<StatutoryRegistrations> StatutoryRegistrationsProperty =
           RegisterProperty<StatutoryRegistrations>(nameof(StatutoryRegistrations));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public StatutoryRegistrations StatutoryRegistrations
        {
            get => GetProperty(StatutoryRegistrationsProperty);
            private set => LoadProperty(StatutoryRegistrationsProperty, value);
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
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                StepIndex = 1;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "";
                IsCompleted = false;
                StatutoryRegistrations = await portal.GetPortal<StatutoryRegistrations>().CreateChildAsync(RuleSet);
            }
            if (currentStepIndex == StepIndex)
                await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
          [Inject] IComplianceInfoStepDal dal,
          [Inject] IChildDataPortal<StatutoryRegistrations> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new ComplianceInfoStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = (parent.CurrentStepIndex - 1) == this.StepIndex ? this.IsCompleted : false, // Only mark as completed if it's the current step
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
                if ((parent.CurrentStepIndex - 1) == StepIndex)
                    await portal.UpdateChildAsync(StatutoryRegistrations, parent);
            }
        }

        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] IComplianceInfoStepDal dal,
          //[Inject] IStatutoryRegistrationsDal dalStatutoryRegistrations,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                StepIndex = data.StepIndex;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "";
                /*
                IsCompleted = data.IsCompleted;
                if (dalStatutoryRegistrations.Exists(tenantId))
                    StatutoryRegistrations = await portal.GetPortal<StatutoryRegistrations>().FetchChildAsync(tenantId, RuleSet);

                else
                    StatutoryRegistrations = await portal.GetPortal<StatutoryRegistrations>().CreateChildAsync(RuleSet);
                */
            }


            if (currentStepIndex == StepIndex)
                await BusinessRules.CheckRulesAsync();
        }


        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent,
            [Inject] IChildDataPortal<StatutoryRegistrations> portal)
        { }
    }
}
