using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployeeStatusDal
    {
        public List<EmployeeStatusDto> Fetch();
        public EmployeeStatusDto Fetch(int id);
    }
}
