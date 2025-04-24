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
    public class PermanentEmployee : BusinessBase<PermanentEmployee>, IEmployeeType
    {
        public static readonly PropertyInfo<int> IdProperty =
            RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<byte[]> TimeStampProperty =
            RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }



        [CreateChild]
        private void Create(int id)
        {
            using (BypassPropertyChecks)
            {
                this.Id = id;
            }
        }
        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] IPermanentEmployeeDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new PermanentEmployeeDto
                {
                    Id = this.Id,
                    EmployeeId = ((EmployeeEmploymentDetails)Parent).EmployeeId,
                    TenantId=parent.TenantId
                };
                 dal.Insert(data);
                TimeStamp = data.LastChanged!;
            }
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] IPermanentEmployeeDal dal)
        {
            //Do Nothing
        }

        [FetchChild]
        private void Fetch(string tenantId,string employeeId, [Inject] IPermanentEmployeeDal dal)
        {
            var data = dal.Fetch(tenantId,employeeId);
            using (BypassPropertyChecks)
            {
                this.Id = data.Id;
                this.TimeStamp = data.LastChanged!;
            }
        }
        [DeleteSelfChild]
        private void Delete(TenantOnboardingOrchestrator parent, [Inject] IPermanentEmployeeDal dal)
        {
             dal.Delete(parent.TenantId,((EmployeeEmploymentDetails)Parent).EmployeeId);
        }
    }
}
