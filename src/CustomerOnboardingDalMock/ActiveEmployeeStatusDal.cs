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
    public class ActiveEmployeeStatusDal : IActiveEmployeeStatusDal
    {
        public void Delete(string tenantId, string employeeId)
        {
            throw new NotImplementedException();
        }

        public EmployeeStatusDto Fetch(string tenantId, int id, string employeeId)
        {
            var result = (from r in MockDb.ActiveEmployees
                          join s in MockDb.StatusTypes on r.Id equals s.Id
                          where r.TenantId == tenantId && r.Id == id && r.EmployeeId == employeeId
                          select new EmployeeStatusDto
                          {
                              Id=r.Id,
                              Name=s.FriendlyName,
                              LastChanged=r.LastChanged
                          }).FirstOrDefault();
            if (result is null)
                throw new DataNotFoundException("ActiveEmployeeStatus");
            return result;
        }

        public void Insert(EmployeeStatusDto data)
        {
            data.LastChanged = MockDb.GetTimeStamp();
            var newItem = new ActiveEmployeeStatusEntity
            {
                Id = data.Id,
                TenantId = data.TenantId,
                EmployeeId = data.EmployeeId,
                LastChanged = data.LastChanged
            };
            MockDb.ActiveEmployees.Add(newItem);
        }
    }
}
