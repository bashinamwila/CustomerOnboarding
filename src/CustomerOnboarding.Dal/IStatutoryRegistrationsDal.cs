using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IStatutoryRegistrationsDal
    {
        public void Insert(StatutoryRegistrationsDto dto);
        public void Update(StatutoryRegistrationsDto dto);

        public  StatutoryRegistrationsDto Fetch(string tenantId);

        public bool Exists(string tenantId);
    }
}
