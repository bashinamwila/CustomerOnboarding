using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IDeductionDal
    {
        void Insert(DeductionDto item);
         DeductionDto Fetch(string tenantId, string id);
        void Update(DeductionDto item);
        void Delete(string tenantId, string id);
        List<DeductionDto> Fetch(string tenantId);
        bool Exists(string tenantId, string id);


    }
}
