using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class ComplianceInfoStepStepsDal : IComplianceInfoStepStepsDal
    {
        public List<StepDto> Fetch(string tenantId)
        {
            StepDto OnboardinStep(dynamic r) => new StepDto
            {
                TenantId = r.TenantId,
                Id = r.Id
            };

            var complianceInfo = MockDb.GeneralComplianceInformationSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var registrations = MockDb.StatutoryRegistrationsSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

           


            return complianceInfo.Concat(registrations).ToList(); ;
        }
    }
}
