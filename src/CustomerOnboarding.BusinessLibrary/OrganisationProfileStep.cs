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
    public class OrganisationProfileStep :
        StepBase<OrganisationProfileStep>
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

        public static readonly PropertyInfo<OrganisationProfile> OrganisationProfileProperty =
           RegisterProperty<OrganisationProfile>(nameof(OrganisationProfile));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public OrganisationProfile OrganisationProfile
        {
            get => GetProperty(OrganisationProfileProperty);
            private set => LoadProperty(OrganisationProfileProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(OrganisationProfileProperty, IsCompletedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is OrganisationProfile)
                BusinessRules.CheckRules(OrganisationProfileProperty);
            base.OnChildChanged(e);
        }

        [CreateChild]
        private async Task CreateAsync(
            string tenantId, int id,
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
                StepIndex = 0;
                if (currentStepIndex == StepIndex)
                {
                    RuleSet = data.RuleSet;
                }
                else
                {
                    RuleSet = "";
                }
                IsCompleted = false;
                OrganisationProfile = await portal.GetPortal<OrganisationProfile>().CreateChildAsync(tenantId,RuleSet);
            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IChildDataPortal<OrganisationProfile> portal,
            [Inject] IOrganisationProfileStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new OrganisationProfileStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted=this.IsCompleted
                };

                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
                if((parent.CurrentStepIndex-1)==StepIndex)
                    await portal.UpdateChildAsync(OrganisationProfile, parent);
            }
        }

        [FetchChild]
        private async Task FetchAsync(string tenantId, int id,
            int currentStepIndex,
           [Inject] IOrganisationProfileStepDal dal,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                StepIndex = data.StepIndex;
                if (currentStepIndex == StepIndex)
                   RuleSet = data.RuleSet;
                else
                    RuleSet = "";
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                IsCompleted=data.IsCompleted;
                TimeStamp = data.LastChanged;

                OrganisationProfile = await portal.GetPortal<OrganisationProfile>().FetchChildAsync(tenantId, RuleSet);
            }
        }

    }
}
