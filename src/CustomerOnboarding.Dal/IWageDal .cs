using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IWageDal
    {
        List<WageDto> Fetch(string tenantId);
        WageDto Fetch(string tenantId,string id);
        void Insert(WageDto data);
        void Update(WageDto data);
        Task Delete(string tenantId,string id);
        bool Exists(string tenantId,string id);

    }
}
