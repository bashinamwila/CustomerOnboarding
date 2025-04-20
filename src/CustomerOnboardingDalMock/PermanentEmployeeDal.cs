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
    public class PermanentEmployeeDal : IPermanentEmployeeDal
    {
        public void Delete(string tenantId, string employeeId)
        {
            throw new NotImplementedException();
        }

        public PermanentEmployeeDto Fetch(string tenantId, string employeeId)
        {
            var result = MockDb.PermanentEmployees
                        .Where(r => r.TenantId == tenantId && r.EmployeeId == employeeId)
                        .Select(r => new PermanentEmployeeDto
                        {
                            Id = r.Id,
                        }).FirstOrDefault();

            if (result is null)
                throw new DataNotFoundException("PermanentEmployee");
            return result;
        }

        public void Insert(PermanentEmployeeDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new PermanentEmployeeEntity
            {
                TenantId = data.TenantId,
                EmployeeId = data.EmployeeId,
                Id = data.Id,
                LastChanged = data.LastChanged
            };
            MockDb.PermanentEmployees.Add(newItem);
        }
    }
}
