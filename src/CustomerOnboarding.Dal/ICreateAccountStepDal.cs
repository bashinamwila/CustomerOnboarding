using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ICreateAccountStepDal
    {
        public void Insert(CreateAccountStepDto data);
        public void Update(CreateAccountStepDto data);
        public CreateAccountStepDto Fetch(string tenantId, int id);
    }
}
