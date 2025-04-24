using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal.Dtos;
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
    public class UserEnteredEmployerExpense : BusinessBase<UserEnteredEmployerExpense>, IEmployerExpenseType
    {
        public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }



        [CreateChild]
        private void Create(string ruleSet)
        {
            using (BypassPropertyChecks)
            {
                Id = 4;

            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] IUserEnteredEmployerExpenseDal dal)
        {
            using (BypassPropertyChecks)
            {
                var item = new TypeOfEmployerExpenseDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    EmployerExpenseId = ((EmployerExpense)Parent).Id

                };
                dal.Insert(item);
                TimeStamp = item.LastChanged!;
            }
        }

        [FetchChild]
        private void Fetch(string tenantId, string id, string ruleSet, [Inject] IUserEnteredEmployerExpenseDal dal)
        {
            using (BypassPropertyChecks)
            {
                var item = dal.Fetch(tenantId, id);
                this.Id = item.Id;
                this.TimeStamp = item.LastChanged!;
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] IUserEnteredEmployerExpenseDal dal)
        {
            //Nothing to update
        }

        [DeleteSelfChild]
        private void Delete(TenantOnboardingOrchestrator parent, [Inject] IUserEnteredEmployerExpenseDal dal)
        {
            dal.Delete(parent.TenantId, ((EmployerExpense)Parent).Id);
        }

        [DeleteSelfChild]
        private void Delete(string tenantId, string id, [Inject] IUserEnteredEmployerExpenseDal dal)
        {
            dal.Delete(tenantId, id);
        }

    }
}
