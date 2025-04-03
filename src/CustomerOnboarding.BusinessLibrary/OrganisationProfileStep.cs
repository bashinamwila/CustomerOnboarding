using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
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
                IsCompleted = false;
                OrganisationProfile = await portal.GetPortal<OrganisationProfile>().CreateChildAsync(tenantId);
            }

            await BusinessRules.CheckRulesAsync();
        }
    }
}
