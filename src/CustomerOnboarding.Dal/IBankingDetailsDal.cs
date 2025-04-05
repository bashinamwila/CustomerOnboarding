using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IBankingDetailsDal
    {
        BankingDetailsDto Fetch(string tenantId);
        void Insert(BankingDetailsDto data);
        void Update(BankingDetailsDto data);
        bool Exists(string tenantId);
    }
}
