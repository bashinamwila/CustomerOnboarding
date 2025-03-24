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
        public Task InsertAsync(CreateAccountStepDto data);
        public Task UpdateAsync(CreateAccountStepDto data);
    }
}
