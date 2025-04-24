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
    public class TerminatedEmployeeStatus : BusinessBase<TerminatedEmployeeStatus>, IEmployeeStatus
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

        public static readonly PropertyInfo<int> ReasonForTerminationProperty =
         RegisterProperty<int>(nameof(ReasonForTermination));
        public int ReasonForTermination
        {
            get => GetProperty(ReasonForTerminationProperty);
            set => SetProperty(ReasonForTerminationProperty, value);
        }


        public static readonly PropertyInfo<DateTime> DateOfTerminationProperty =
          RegisterProperty<DateTime>(nameof(DateOfTermination));
        public DateTime DateOfTermination
        {
            get => GetProperty(DateOfTerminationProperty);
            set => SetProperty(DateOfTerminationProperty, value);
        }






        [CreateChild]
        private void Create(int id,
          [Inject] IEmployeeStatusDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
            }
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
             [Inject] ITerminatedEmployeeStatusDal dal)
        {

            using (BypassPropertyChecks)
            {
                var data = new TerminatedEmployeeStatusDto
                {
                    Id = this.Id,
                    TenantId=parent.TenantId,
                    EmployeeId = ((EmployeeEmploymentDetails)Parent).EmployeeId,
                    ReasonForTermination = this.ReasonForTermination,
                    DateOfTermination = this.DateOfTermination,

                };
                dal.Insert(data);
                TimeStamp = data.LastChanged;

            }

        }

        [UpdateChild]
        private void Update() { }

        [FetchChild]
        private void Fetch(string tenantId,int id, string employeeId,
        [Inject] ITerminatedEmployeeStatusDal dal)
        {
            var data = dal.Fetch(tenantId,id, employeeId);
            using (BypassPropertyChecks)
            {
                Id = data.Id;
                Name = data.Name;
                ReasonForTermination = data.ReasonForTermination;
                DateOfTermination = data.DateOfTermination;
                TimeStamp = data.LastChanged;
            }
        }
    }
}
