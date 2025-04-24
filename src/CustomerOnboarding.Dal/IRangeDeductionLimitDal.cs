using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IRangeDeductionLimitDal
    {
        public LimitDto Fetch(string tenantId, string id);
        public void Insert(LimitDto data);
        public void Update(LimitDto data);
        public void Delete(string tenantId,string id);
    }
}
