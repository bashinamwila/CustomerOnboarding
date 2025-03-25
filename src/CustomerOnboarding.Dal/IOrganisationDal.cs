using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IOrganisationDal
    {
        public void Insert(OrganisationDto data);
        public OrganisationDto Fetch(string id);
    }
}
