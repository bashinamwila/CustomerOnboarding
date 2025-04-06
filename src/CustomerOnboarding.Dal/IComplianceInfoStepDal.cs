using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IComplianceInfoStepDal
    {
        public void Insert(ComplianceInfoStepDto dto);
        public void Update(ComplianceInfoStepDto dto);
        public ComplianceInfoStepDto Fetch(string tenantId, int id);
    }
}
