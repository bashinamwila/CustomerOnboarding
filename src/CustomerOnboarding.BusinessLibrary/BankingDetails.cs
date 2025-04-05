using Csla;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class BankingDetails :
        BusinessBase<BankingDetails>
    {
        public static readonly PropertyInfo<string> BankIdProperty =
           RegisterProperty<string>(nameof(BankId));
        [Display(Name = "Bank Id")]
        public string BankId
        {
            get { return GetProperty(BankIdProperty); }
            set { SetProperty(BankIdProperty, value); }
        }
        public static readonly PropertyInfo<int> BranchIdProperty =
            RegisterProperty<int>(nameof(BranchId));
        [Display(Name = "Branch Id")]
        public int BranchId
        {
            get { return GetProperty(BranchIdProperty); }
            set { SetProperty(BranchIdProperty, value); }
        }


        public static readonly PropertyInfo<string> BranchCodeProperty =
            RegisterProperty<string>(nameof(BranchCode));
        public string BranchCode
        {
            get { return GetProperty(BranchCodeProperty); }
            private set { LoadProperty(BranchCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> BankAccountNumberProperty =
           RegisterProperty<string>(nameof(BankAccountNumber));
        [Display(Name = "Account #")]
        public string BankAccountNumber
        {
            get { return GetProperty(BankAccountNumberProperty); }
            set { SetProperty(BankAccountNumberProperty, value); }
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
           


            BusinessRules.RuleSet = "Banking Details";
            BusinessRules.AddRule(new BusinessLibrary.Rules.ValidBank(BankIdProperty) { Priority = 0 });
            BusinessRules.AddRule(new BusinessLibrary.Rules.ValidBankBranch(BranchIdProperty, BankIdProperty) { Priority = 0 });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(BankIdProperty) { MessageText = "Bank is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(BranchIdProperty) { MessageText = "Branch is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(BankAccountNumberProperty) { MessageText = "Account # is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.GetBranchCode(BankIdProperty, BranchIdProperty, BranchCodeProperty) { Priority = 1 });
            BusinessRules.AddRule(new BusinessLibrary.Rules.GetBranchCode(BranchIdProperty, BankIdProperty, BranchCodeProperty) { Priority = 1 });
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(BankIdProperty, BranchCodeProperty, BranchIdProperty));
        }

        [CreateChild]
        private async Task CreateAsync(string ruleSet)
        {
            BusinessRules.RuleSet = ruleSet;
            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
            [Inject]IBankingDetailsDal dal)
        {
            using (BypassPropertyChecks) 
            {
                var dto = new BankingDetailsDto
                {
                    TenantId = parent.TenantId,
                    BankId = this.BankId,
                    BranchId = this.BranchId,
                    AccountNumber = this.BankAccountNumber
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
            }
        }

        [FetchChild]
        private async Task FetchAsync(string tenantId,string ruleSet, [Inject] IBankingDetailsDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = dal.Fetch(tenantId);
                BankId = dto.BankId;
                BranchId = dto.BranchId;
                BankAccountNumber = dto.AccountNumber;
                TimeStamp = dto.LastChanged;
                BusinessRules.RuleSet = ruleSet;
            }
            await BusinessRules.CheckRulesAsync();
        }
    }
}
