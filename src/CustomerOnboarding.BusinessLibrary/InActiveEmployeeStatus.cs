using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
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
    public class InActiveEmployeeStatus : BusinessBase<InActiveEmployeeStatus>, IEmployeeStatus
    {
        public static readonly PropertyInfo<int> IdProperty =
        RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get => GetProperty(IdProperty);
            private set => LoadProperty(IdProperty, value);
        }
        public static readonly PropertyInfo<string> NameProperty =
          RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get => GetProperty(NameProperty);
            private set => LoadProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }

        }

        public static readonly PropertyInfo<int> ReasonForInActivityProperty =
         RegisterProperty<int>(nameof(ReasonForInActivity));
        public int ReasonForInActivity
        {
            get => GetProperty(ReasonForInActivityProperty);
            set => SetProperty(ReasonForInActivityProperty, value);
        }


        public static readonly PropertyInfo<DateTime> DateOfInActitityProperty =
          RegisterProperty<DateTime>(nameof(DateOfInActivity));
        public DateTime DateOfInActivity
        {
            get => GetProperty(DateOfInActitityProperty);
            set => SetProperty(DateOfInActitityProperty, value);
        }

        public static readonly PropertyInfo<bool> IsOnPayrollProperty =
         RegisterProperty<bool>(nameof(IsOnPayroll));
        public bool IsOnPayroll
        {
            get => GetProperty(IsOnPayrollProperty);
            set => SetProperty(IsOnPayrollProperty, value);
        }





        [CreateChild]
        private void Create(int id,string ruleSet,
          [Inject] IEmployeeStatusDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
             [Inject] IInActiveEmployeeStatusDal dal)
        {

            using (BypassPropertyChecks)
            {
                var data = new InActiveEmployeeStatusDto
                {
                    TenantId=parent.TenantId,
                    EmployeeId = ((EmployeeEmploymentDetails)Parent).EmployeeId,
                    Id = this.Id,
                    ReasonForInActivity = this.ReasonForInActivity,
                    DateOfInActivity = this.DateOfInActivity,
                    IsOnPayroll = this.IsOnPayroll
                };
                dal.Insert(data);
                TimeStamp = data.LastChanged;

            }

        }

        [UpdateChild]
        private void Update() { }

        [FetchChild]
        private void Fetch(string tenantId,int id, string employeeId,
            string ruleSet,
        [Inject] IInActiveEmployeeStatusDal dal)
        {
            var data = dal.Fetch(tenantId,id, employeeId);
            using (BypassPropertyChecks)
            {
                Id = data.Id;
                Name = data.Name;
                ReasonForInActivity = data.ReasonForInActivity;
                DateOfInActivity = data.DateOfInActivity;
                IsOnPayroll = data.IsOnPayroll;
                TimeStamp = data.LastChanged;
            }
        }
    }
}
