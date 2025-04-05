using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IBankingDetailsStepDal
    {
        public void Insert(BankingDetailsStepDto dto);
        public void Update(BankingDetailsStepDto dto);
        public BankingDetailsStepDto Fetch(string tenantId, int id);
    }
}
