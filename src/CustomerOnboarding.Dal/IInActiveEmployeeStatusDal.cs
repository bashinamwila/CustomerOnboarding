using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IInActiveEmployeeStatusDal
    {

        public void Insert(InActiveEmployeeStatusDto data);
        public InActiveEmployeeStatusDto Fetch(string tenantId, int id, string employeeId);

        public void Delete(string tenantId, string employeeId);


    }

}
