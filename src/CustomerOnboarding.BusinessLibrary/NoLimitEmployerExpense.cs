using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using CustomerOnboarding.BusinessLibrary.Attributes;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class NoLimitEmployerExpense : BusinessBase<NoLimitEmployerExpense>, ILimit
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
        [DefaultValueAllowed("Zero is allowed for minimum")]
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
            BusinessRules.AddRule(
                new NotEditable(AuthorizationActions.WriteProperty, MinimumProperty));
            BusinessRules.AddRule(
               new NotEditable(AuthorizationActions.WriteProperty, MaximumProperty));

        }

        private class NotEditable : Csla.Rules.AuthorizationRule
        {
            public NotEditable(Csla.Rules.AuthorizationActions actions, Csla.Core.IMemberInfo element)
                : base(actions, element)
            {

            }
            protected override void Execute(IAuthorizationContext context)
            {
                var target = context.Target as ILimit;
                if (target != null)
                {
                    if (target is NoLimitEmployerExpense)
                        context.HasPermission = false;
                    else
                        context.HasPermission = true;
                }
            }
        }



        [CreateChild]
        private void Create(string ruleSet)
        {
            using (BypassPropertyChecks)
            {
                this.Id = 1;
                this.Minimum = 0m;
                this.Maximum = decimal.MaxValue;
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] INoEmployerExpenseLimitDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new LimitDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    ItemId = ((EmployerExpense)Parent).Id
                };
                dal.Insert(data);
                this.TimeStamp = data.LastChanged!;

            }
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] INoEmployerExpenseLimitDal dal)
        {
            //Do Notihng
        }
        [FetchChild]
        private void Fetch(string tenantId, string id, string ruleSet, [Inject] INoEmployerExpenseLimitDal dal)
        {
            var data = dal.Fetch(tenantId, id);
            using (BypassPropertyChecks)
            {
                this.Id = data.Id;
                this.TimeStamp = data.LastChanged!;
                this.Minimum = 0m;
                this.Maximum = decimal.MaxValue;
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }


        [DeleteSelfChild]
        private void DeleteSelf(string tenantId, string id, [Inject] INoEmployerExpenseLimitDal dal)
        {
            dal.Delete(tenantId, id);
        }
        [DeleteSelfChild]
        private void Delete(TenantOnboardingOrchestrator parent, [Inject] INoEmployerExpenseLimitDal dal)
        {
            dal.Delete(parent.TenantId, ((EmployerExpense)Parent).Id);
        }
    }
}
