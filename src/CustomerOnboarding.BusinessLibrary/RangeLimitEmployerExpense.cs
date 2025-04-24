using Csla.Core;
using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal;
using CustomerOnboarding.BusinessLibrary.Rules;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class RangeLimitEmployerExpense : BusinessBase<RangeLimitEmployerExpense>, ILimit
    {
        public static readonly PropertyInfo<int> IdProperty =
            RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<decimal?> MinimumProperty =
            RegisterProperty<decimal?>(nameof(Minimum));
        public decimal? Minimum
        {
            get { return GetProperty(MinimumProperty); }
            set { SetProperty(MinimumProperty, value); }
        }

        public static readonly PropertyInfo<decimal?> MaximumProperty =
            RegisterProperty<decimal?>(nameof(Maximum));
        public decimal? Maximum
        {
            get { return GetProperty(MaximumProperty); }
            set { SetProperty(MaximumProperty, value); }
        }

        public static readonly PropertyInfo<byte[]> TimeStamProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStamProperty); }
            set { SetProperty(TimeStamProperty, value); }
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new
                MinimumLimitGTMaximumLimit(MinimumProperty, MaximumProperty));
            BusinessRules.AddRule(new
                MinimumLimitGTMaximumLimit(MaximumProperty, MinimumProperty));

        }



        [CreateChild]
        private void Create(string ruleSet)
        {
            using (BypassPropertyChecks)
            {
                this.Id = 2;

            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] IRangeEmployerExpenseLimitDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new LimitDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    ItemId = ((EmployerExpense)Parent).Id,
                    Minimum = this.Minimum,
                    Maximum = this.Maximum
                };
                dal.Insert(data);
                this.TimeStamp = data.LastChanged!;

            }
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] IRangeEmployerExpenseLimitDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new LimitDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    ItemId = ((EmployerExpense)Parent).Id,
                    Minimum = this.Minimum,
                    Maximum = this.Maximum,
                    LastChanged = this.TimeStamp
                };
                dal.Update(data);
                this.TimeStamp = data.LastChanged!;

            }
        }

        [FetchChild]
        private void Fetch(string tenantId, string id, string ruleSet, [Inject] IRangeEmployerExpenseLimitDal dal)
        {
            var data = dal.Fetch(tenantId, id);
            using (BypassPropertyChecks)
            {
                this.Id = data.Id;
                this.TimeStamp = data.LastChanged!;
                this.Minimum = data.Minimum;
                this.Maximum = data.Maximum;
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [DeleteSelfChild]
        private void Delete(string tenantId, string id, [Inject] IRangeEmployerExpenseLimitDal dal)
        {
            dal.Delete(tenantId, id);
        }
        [DeleteSelfChild]
        private void Delete(TenantOnboardingOrchestrator parent, [Inject] IRangeEmployerExpenseLimitDal dal)
        {
            dal.Delete(parent.TenantId, ((EmployerExpense)Parent).Id);
        }
    }
}
