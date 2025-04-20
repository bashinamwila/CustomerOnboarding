using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IActiveEmployeeStatusDal
    {

        public void Insert(EmployeeStatusDto data);
        public EmployeeStatusDto Fetch(string tenantId, int id, string employeeId);

        public void Delete(string tenantId, string employeeId);



    }
}
