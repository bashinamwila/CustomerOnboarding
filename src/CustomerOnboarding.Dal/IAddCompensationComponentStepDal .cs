using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface IAddCompensationComponentStepDal
    {
        public void Insert(AddCompensationComponentStepDto dto);
        public void Update(AddCompensationComponentStepDto dto);

        public AddCompensationComponentStepDto Fetch(string tenantId, int id);
    }
}
