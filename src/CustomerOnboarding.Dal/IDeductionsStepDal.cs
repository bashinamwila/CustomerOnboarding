using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IDeductionsStepDal
    {
        public void Insert(DeductionsStepDto dto);
        public void Update(DeductionsStepDto dto);
        public DeductionsStepDto Fetch(string tenantId, int id);
    }
}
