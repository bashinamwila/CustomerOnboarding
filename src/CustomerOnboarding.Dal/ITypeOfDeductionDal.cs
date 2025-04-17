using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ITypeOfDeductionDal
    {
        public TypeOfDeductionDto Fetch(string tenantId,string id);
        public Task InsertAsync(TypeOfDeductionDto data);
        public Task UpdateAsync(TypeOfDeductionDto data);
        public Task DeleteAsync(string tenantId, string id);
    }
}
