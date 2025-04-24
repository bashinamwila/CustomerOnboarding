using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IOrganisationProfileStepDal
    {
        public void Insert(OrganisationProfileStepDto data);
        public void Update(OrganisationProfileStepDto data);
        public OrganisationProfileStepDto Fetch(string tenantId, int id);
    }
}
