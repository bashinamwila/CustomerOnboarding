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
    public class NewEmployeeStatus : BusinessBase<NewEmployeeStatus>, IEmployeeStatus
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
        private void InsertAsync(TenantOnboardingOrchestrator parent,
             [Inject] INewEmployeeStatusDal dal)
        {

            using (BypassPropertyChecks)
            {
                var data = new EmployeeStatusDto
                {
                    Id = this.Id,
                    EmployeeId = ((EmployeeEmploymentDetails)Parent).EmployeeId,
                    TenantId=parent.TenantId
                };
                dal.Insert(data);
                TimeStamp = data.LastChanged;

            }

        }

        [DeleteSelfChild]
        private void DeleteSelf(string tenantId,int id, string employeeId,
            [Inject] INewEmployeeStatusDal dal)
        {
            dal.Delete(tenantId,id, employeeId);
        }


        [DeleteSelfChild]
        private void Delete(TenantOnboardingOrchestrator parent,
            [Inject] INewEmployeeStatusDal dal)
        {
            dal.Delete(parent.TenantId,this.Id, ((EmployeeEmploymentDetails)Parent).EmployeeId);
        }



        [UpdateChild]
        private void Update() { }

        [FetchChild]
        private void Fetch(string tenantId,int id, string employeeId,string ruleSet,
        [Inject] INewEmployeeStatusDal dal)
        {
            var data = dal.Fetch(tenantId,id, employeeId);
            using (BypassPropertyChecks)
            {
                Id = data.Id;
                Name = data.Name;
                TimeStamp = data.LastChanged;
            }

            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }
    }
}
