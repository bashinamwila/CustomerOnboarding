using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IEmployeeWageDal
    {
        public void Insert(EmployeeWageDto data);
        public void Update(EmployeeWageDto data);
        public List<EmployeeWageDto> Fetch(string tenantId,string employeeId);
        public EmployeeWageDto Fetch(string tenantId,string employeeId, string id);
        public void Delete(string tenantId, string employeeId, string id);
        public void UpdateYTD(string tenantId, string employeeId, string id, decimal ytd, byte[] lastchanged);
    }
}
