using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IStatutoryRegistrationsStepDal
    {
        public void Insert(StatutoryRegistrationsStepDto dto);
        public void Update(StatutoryRegistrationsStepDto dto);

        public StatutoryRegistrationsStepDto Fetch(string tenantId, int id);
    }
}
