using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.BaseTypes;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class PercentWageDeduction : BusinessBase<PercentWageDeduction>, IPercentWageDeduction
    {
        public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<decimal> PercentProperty = RegisterProperty<decimal>(nameof(Percent));
        [Display(Name = "Equals")]
        public decimal Percent
        {
            get { return GetProperty(PercentProperty); }
            set { SetProperty(PercentProperty, value); }
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }

        public static readonly PropertyInfo<string> WageIdProperty =
            RegisterProperty<string>(nameof(WageId));
        [Display(Name = "% of")]
        public string WageId
        {
            get { return GetProperty(WageIdProperty); }
            set { SetProperty(WageIdProperty, value); }
        }

        protected override void AddBusinessRules()
        {
            BusinessRules.RuleSet = "Add Deduction";
            base.AddBusinessRules();
            BusinessRules.AddRule(new ValidWage(WageIdProperty));
            BusinessRules.AddRule(new ShouldBeGreaterThan(PercentProperty, 0m));
            //  BusinessRules.AddRule(new FormatPercentage(PercentProperty));
        }




        [CreateChild]
        private void Create(string ruleSet)
        {
            using (BypassPropertyChecks)
            {
                Id = 2;

            }
            BusinessRules.RuleSet= ruleSet;
            BusinessRules.CheckRules();
        }
        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] IPercentWageDeductionDal dal)
        {
            using (BypassPropertyChecks)
            {
                var item = new TypeOfDeductionDto
                {
                    Id = this.Id,
                    TenantId= parent.TenantId,
                    DeductionId = ((Deduction)Parent).Id,
                    Value = this.Percent,
                    WageId = this.WageId
                };
                dal.Insert(item);
                TimeStamp = item.LastChanged!;
            }
        }

        [FetchChild]
        private void Fetch(string tenantId,string id,string ruleSet, [Inject] IPercentWageDeductionDal dal)
        {
            using (BypassPropertyChecks)
            {
                var item = dal.Fetch(tenantId,id);
                this.Id = item.Id;
                this.Percent = item.Value!.Value;
                this.WageId = item.WageId;
                this.TimeStamp = item.LastChanged!;
            }
            BusinessRules.RuleSet = ruleSet;
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] IPercentWageDeductionDal dal)
        {
            using (BypassPropertyChecks)
            {
                var item = new TypeOfDeductionDto
                {
                    Id = this.Id,
                    TenantId=parent.TenantId,
                    DeductionId = ((Deduction)Parent).Id,
                    Value = this.Percent,
                    WageId = this.WageId,
                    LastChanged = this.TimeStamp
                };
                dal.Update(item);
                TimeStamp = item.LastChanged;
            }
        }

        [DeleteSelfChild]
        private void DeleteAsync(TenantOnboardingOrchestrator parent, [Inject] IPercentWageDeductionDal dal)
        {
            dal.Delete(parent.TenantId,((Deduction)Parent).Id);
        }

        [DeleteSelfChild]
        private void Delete(string tenantId,string id, [Inject] IPercentWageDeductionDal dal)
        {
            dal.Delete(tenantId, id);
        }
    }
}
