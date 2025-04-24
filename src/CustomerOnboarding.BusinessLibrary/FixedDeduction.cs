using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class FixedDeduction : BusinessBase<FixedDeduction>, IFixedDeduction
    {
        public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<decimal> AmountProperty = RegisterProperty<decimal>(nameof(Amount));
        [Display(Name = "Equals")]
        public decimal Amount
        {
            get { return GetProperty(AmountProperty); }
            set { SetProperty(AmountProperty, value); }
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
            BusinessRules.AddRule(new ShouldBeGreaterThan(AmountProperty, 0m));
        }



        [CreateChild]
        private void Create(string ruleSet)
        {
            using (BypassPropertyChecks)
            {
                Id = 1;
                LoadProperty(AmountProperty, 0m);
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }
        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] IFixedDeductionDal dal)
        {
            using (BypassPropertyChecks)
            {
                var amount = ReadProperty(AmountProperty);
                var item = new TypeOfDeductionDto
                {
                    Id = this.Id,
                    TenantId=parent.TenantId,
                    DeductionId = ((Deduction)Parent).Id,
                    Value = Amount
                };
                 dal.Insert(item);
                TimeStamp = item.LastChanged!;
            }
        }

        [FetchChild]
        private void Fetch(string tenantId,string id,string ruleSet, [Inject] IFixedDeductionDal dal)
        {
            using (BypassPropertyChecks)
            {
                var item = dal.Fetch(tenantId,id);
                this.Id = item.Id;
                LoadProperty(AmountProperty, item.Value!.Value);
                this.TimeStamp = item.LastChanged!;
            }
            BusinessRules.RuleSet=ruleSet;
            BusinessRules.CheckRules();
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] IFixedDeductionDal dal)
        {
            using (BypassPropertyChecks)
            {
                var amount = ReadProperty(AmountProperty);
                var item = new TypeOfDeductionDto
                {
                    Id = this.Id,
                    TenantId=parent.TenantId,
                    DeductionId = ((Deduction)Parent).Id,
                    Value = Amount,
                    LastChanged = this.TimeStamp
                };
                dal.Update(item);
                TimeStamp = item.LastChanged;
            }
        }

        [DeleteSelfChild]
        private void Delete(TenantOnboardingOrchestrator parent, [Inject] IFixedDeductionDal dal)
        {
            var _deduction = (Deduction)Parent;
            dal.Delete(parent.TenantId,_deduction.Id);
        }

        [DeleteSelfChild]
        private void Delete(string tenantId,string id, [Inject] IFixedDeductionDal dal)
        {
            dal.Delete(tenantId,id);
        }


    }
}
