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
    public class InActiveEmployeeStatusDal : IInActiveEmployeeStatusDal
    {
        public void Delete(string tenantId, string employeeId)
        {
            throw new NotImplementedException();
        }

        public InActiveEmployeeStatusDto Fetch(string tenantId, int id, string employeeId)
        {
            var result = (from r in MockDb.InActiveEmployees
                          join s in MockDb.StatusTypes on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id && r.EmployeeId == employeeId
                          select new InActiveEmployeeStatusDto
                          {
                              Id = r.Id,
                              Name = s.FriendlyName,
                              ReasonForInActivity=r.ReasonForInActivity,
                              DateOfInActivity=r.DateOfInActivity,
                              IsOnPayroll=r.IsOnPayroll,
                              LastChanged = r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("NewEmployeeStatus");
            return result;
        }

        public void Insert(InActiveEmployeeStatusDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new InActiveEmployeeStatusEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployeeId = data.EmployeeId,
                ReasonForInActivity=data.ReasonForInActivity,
                DateOfInActivity=data.DateOfInActivity,
                IsOnPayroll=data.IsOnPayroll,
                LastChanged = data.LastChanged
            };
            MockDb.InActiveEmployees.Add(newItem);
        }
    }
}
