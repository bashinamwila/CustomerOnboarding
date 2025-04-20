using Csla.Core;
using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class TemporalEmployee : EmployeeTypeBase<TemporalEmployee>
    {
        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] ITemporalEmployeeDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new TemporalEmployeeDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    EmployeeId = ((EmployeeEmploymentDetails)Parent).EmployeeId,
                    ContractDuration = this.ContractDuration,
                    Interval = this.Interval,
                    ContractStartDate = this.ContractStartDate.Date!.Value

                };
                dal.Insert(data);
                TimeStamp = data.LastChanged!;
            }
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] ITemporalEmployeeDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new TemporalEmployeeDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    EmployeeId = ((EmployeeEmploymentDetails)Parent).EmployeeId,
                    ContractDuration = this.ContractDuration,
                    Interval = this.Interval,
                    ContractStartDate = this.ContractStartDate.Date!.Value,
                    LastChanged = this.TimeStamp
                };
                dal.Update(data);
                TimeStamp = data.LastChanged;
            }
        }

        [FetchChild]
        private void Fetch(string tenantId, string employeeId, [Inject] ITemporalEmployeeDal dal,
               [Inject] IChildDataPortal<DateSplitter> portal)
        {
            var data = dal.Fetch(tenantId, employeeId);
            using (BypassPropertyChecks)
            {
                this.Id = data.Id;
                this.ContractDuration = data.ContractDuration;
                this.Interval = data.Interval;
                this.TimeStamp = data.LastChanged!;
                this.ContractStartDate = portal.FetchChild(data.ContractStartDate);

            }
            BusinessRules.CheckRules(ContractExpiryDateProperty);
        }
        [DeleteSelfChild]
        private void Delete(TenantOnboardingOrchestrator parent, [Inject] ITemporalEmployeeDal dal)
        {
            dal.Delete(parent.TenantId, ((EmployeeEmploymentDetails)Parent).EmployeeId);
        }
    }
}
