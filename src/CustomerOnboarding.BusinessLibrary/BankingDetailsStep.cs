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
    public class BankingDetailsStep :
        StepBase<BankingDetailsStep>
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

        public static readonly PropertyInfo<BankingDetails> BankingDetailsProperty =
           RegisterProperty<BankingDetails>(nameof(BankingDetails));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public BankingDetails BankingDetails
        {
            get => GetProperty(BankingDetailsProperty);
            private set => LoadProperty(BankingDetailsProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(BankingDetailsProperty, IsCompletedProperty));
            
        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is BankingDetails)
                BusinessRules.CheckRules(BankingDetailsProperty);
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
                StepIndex = 1;
                if (currentStepIndex == StepIndex)
                    RuleSet = data.RuleSet;
                else
                    RuleSet = "";
                IsCompleted = false;
                BankingDetails = await portal.GetPortal<BankingDetails>().CreateChildAsync(RuleSet);
            }
            if(currentStepIndex== StepIndex)
                await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IBankingDetailsStepDal dal,
            [Inject] IChildDataPortal<BankingDetails> portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new BankingDetailsStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted=(parent.CurrentStepIndex-1)==this.StepIndex?this.IsCompleted : false, // Only mark as completed if it's the current step
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
                if((parent.CurrentStepIndex-1)==StepIndex)
                        await portal.UpdateChildAsync(BankingDetails, parent);
            }
        }

        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] IBankingDetailsStepDal dal,
          [Inject] IBankingDetailsDal dalBankingDetails,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId,id);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                StepIndex = data.StepIndex;
                if(currentStepIndex== StepIndex)
                    RuleSet = data.RuleSet;
                else
                   RuleSet = "";
                
                IsCompleted = data.IsCompleted;
                if(dalBankingDetails.Exists(tenantId))
                    BankingDetails = await portal.GetPortal<BankingDetails>().FetchChildAsync(tenantId,RuleSet);
                
                else
                    BankingDetails = await portal.GetPortal<BankingDetails>().CreateChildAsync(RuleSet);
            }

            if (currentStepIndex == StepIndex)
                await BusinessRules.CheckRulesAsync();
        }
    }
}
