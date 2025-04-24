using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IOrganisationProfileDal
    {
        public void Insert(OrganisationProfileDto dto);
        public void Update(OrganisationProfileDto dto);
        public OrganisationProfileDto Fetch(string id);
    }
}
