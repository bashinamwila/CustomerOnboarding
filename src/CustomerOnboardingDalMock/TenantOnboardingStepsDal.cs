using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class TenantOnboardingStepsDal :
        ITenantOnboardingStepsDal
    {
        public List<StepDto> Fetch(string tenantId)
        {
            StepDto OnboardinStep(dynamic r) => new StepDto
            {
                TenantId = r.TenantId,
                Id = r.Id
            };

            var orgSteps = MockDb.OrganisationProfileSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var bankingSteps = MockDb.BankingDetailsSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            return orgSteps.Concat(bankingSteps).ToList();
        }
    }
}
