using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class TemporalEmployeeDal : ITemporalEmployeeDal
    {
        public void Delete(string tenantId, string employeeId)
        {
            throw new NotImplementedException();
        }

        public TemporalEmployeeDto Fetch(string tenantId, string employeeId)
        {
            var result = MockDb.TemporalEmployees
                        .Where(r => r.TenantId == tenantId && r.EmployeeId == employeeId)
                        .Select(r => new TemporalEmployeeDto
                        {
                            Id = r.Id,
                            ContractDuration = r.ContractDuration,
                            Interval = r.Interval,
                            ContractStartDate = r.ContractStartDate
                        }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("TemporalEmployee");
            return result;
        }

        public void Insert(TemporalEmployeeDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new TemporalEmployeeEntity
            {
                TenantId = data.TenantId,
                EmployeeId = data.EmployeeId,
                Id = data.Id,
                ContractDuration = data.ContractDuration,
                Interval = data.Interval,
                ContractStartDate = data.ContractStartDate,
                LastChanged = data.LastChanged
            };
            MockDb.TemporalEmployees.Add(newItem);
        }

        public void Update(TemporalEmployeeDto data)
        {
            var result = MockDb.TemporalEmployees
                        .Where(r => r.TenantId == data.TenantId && r.EmployeeId == data.EmployeeId)
                        .Select(r => r).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("TemporalEmployee");
            if (!result.LastChanged!.Matches(data.LastChanged!))
                throw new ConcurrencyException("TemporalEmployee");
            data.LastChanged = MockDb.GetTimeStamp();
            result.ContractDuration = data.ContractDuration;
            result.Interval = data.Interval;
            result.ContractStartDate = data.ContractStartDate;
            result.LastChanged = data.LastChanged;
        }
    }
}
