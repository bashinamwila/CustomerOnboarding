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
    public class ContractEmployeeDal : IContractEmployeeDal
    {
        public void Delete(string tenantId, string employeeId)
        {
            throw new NotImplementedException();
        }

        public ContractEmployeeDto Fetch(string tenantId, string employeeId)
        {
            var result = MockDb.ContractEmployees
                        .Where(r => r.TenantId == tenantId && r.EmployeeId == employeeId)
                        .Select(r => new ContractEmployeeDto
                        {
                            Id=r.Id,
                            ContractDuration=r.ContractDuration,
                            Interval=r.Interval,
                            ContractStartDate=r.ContractStartDate
                        }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("ContractEmployee");
            return result;
        }

        public void Insert(ContractEmployeeDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new ContractEmployeeEntity
            {
                TenantId = data.TenantId,
                EmployeeId = data.EmployeeId,
                Id = data.Id,
                ContractDuration = data.ContractDuration,
                Interval = data.Interval,
                ContractStartDate = data.ContractStartDate,
                LastChanged = data.LastChanged
            };
            MockDb.ContractEmployees.Add(newItem);
        }

        public void Update(ContractEmployeeDto data)
        {
            var result = MockDb.ContractEmployees
                        .Where(r => r.TenantId == data.TenantId && r.EmployeeId == data.EmployeeId)
                        .Select(r => r).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("ContractEmployee");
            if (!result.LastChanged!.Matches(data.LastChanged!))
                throw new ConcurrencyException("ContractEmployee");
            data.LastChanged = MockDb.GetTimeStamp();
            result.ContractDuration = data.ContractDuration;
            result.Interval = data.Interval;
            result.ContractStartDate = data.ContractStartDate;
            result.LastChanged = data.LastChanged;

        }
    }
}
