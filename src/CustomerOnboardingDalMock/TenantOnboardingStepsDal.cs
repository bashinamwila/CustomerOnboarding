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

            var step1 = MockDb.OrganisationProfileSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var step2 = MockDb.BankingDetailsSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var step3 = MockDb.ComplianceInfoSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var step4 = MockDb.CompensationComponentsSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);




            return      step1.
                        Concat(step2).
                        Concat(step3).
                        Concat(step4).
                        ToList();
        }
    }
}
