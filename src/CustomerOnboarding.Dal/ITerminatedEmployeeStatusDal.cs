using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ITerminatedEmployeeStatusDal
    {


        public TerminatedEmployeeStatusDto Fetch(string tenantId, int id, string employeeId);
        public void Insert(TerminatedEmployeeStatusDto data);

        public void Delete(string tenantId, string employeeId);

    }
}
