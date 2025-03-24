using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    public interface ICustomerOnboardingOrchestratorDal
    {
        public Task InsertAsync(CustomerOnboardingOrchestratorDto data);
        public Task UpdateAsync(CustomerOnboardingOrchestratorDto data);
        public Task<CustomerOnboardingOrchestratorDto> FetchAsync(string tenantId);
    }
}
