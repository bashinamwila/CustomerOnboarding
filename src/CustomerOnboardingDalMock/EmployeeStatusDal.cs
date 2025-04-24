using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class EmployeeStatusDal : IEmployeeStatusDal
    {
        public List<EmployeeStatusDto> Fetch()
        {
            throw new NotImplementedException();
        }

        public EmployeeStatusDto Fetch(int id)
        {
            var result = (from r in MockDb.StatusTypes
                          where r.Id == id
                          select new EmployeeStatusDto
                          {
                              Id = r.Id,
                              Name = r.FriendlyName,
                          }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("EmployeeStatus");
            return result;
        }
    }
}
