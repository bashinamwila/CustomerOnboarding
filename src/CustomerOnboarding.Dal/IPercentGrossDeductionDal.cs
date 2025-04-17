using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IPercentGrossDeductionDal
    {
        public TypeOfDeductionDto Fetch(string tenantId, string id);
        public void Insert(TypeOfDeductionDto data);
        public void Update(TypeOfDeductionDto data);
        public void Delete(string tenantId, string id);
    }
}
