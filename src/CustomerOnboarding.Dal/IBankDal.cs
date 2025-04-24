using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IBankDal
    {
        void Insert(BankDto data);
        void  Update(BankDto data);
        BankDto Fetch(string id);
        List<BankDto> Fetch();
        Task DeleteAsync(string id);
    }
}
