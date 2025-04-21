using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployeeVariableDal
    {
        public void Insert(EmployeeVariableDto data);
        public void Update(EmployeeVariableDto data);
        public List<EmployeeVariableDto> Fetch(string tenantId,string employeeId, string id);
        public EmployeeVariableDto Fetch(string employeeId, string wageId, int id);
        public Task DeleteAsync(string employeeId, string id, int type);
    }
}
