using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class CompensationComponentsStepStepsDal :
        ICompensationComponentsStepStepsDal
    {
        public List<StepDto> Fetch(string tenantId)
        {
            StepDto OnboardinStep(dynamic r) => new StepDto
            {
                TenantId = r.TenantId,
                Id = r.Id
            };

            var step1 = MockDb.GeneralCompensationComponentsInformationSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var  step2 = MockDb.AddCompensationComponentSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var step3 = MockDb.AddCompensationComponentConfirmationSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);






            return step1.Concat(step2).Concat(step3).ToList(); ;
        }
    }
}
