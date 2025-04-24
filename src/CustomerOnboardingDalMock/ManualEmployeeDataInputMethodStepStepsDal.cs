using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class ManualEmployeeDataInputMethodStepStepsDal : IManualEmployeeDataInputMethodStepStepsDal
    {
        public List<StepDto> Fetch(string tenantId)
        {

            var step1 = MockDb.GeneralEmployeeDetailsInformationSteps
                .Where(r => r.TenantId == tenantId)
                .Select(r=>new StepDto
                {
                    TenantId=r.TenantId,
                    Id=r.Id
                }).ToList();



            StepDto OnboardinStep(dynamic r) => new StepDto
            {
                TenantId = r.TenantId,
                Id = r.Id,
                Counter=r.Counter
            };

            

            var step2 = MockDb.AddEmployeeEmploymentDetailsSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);

            var step3 = MockDb.AddEmployeePersonalDetailsSteps
                .Where(r => r.TenantId == tenantId)
                .Select(OnboardinStep);


            var step4 = MockDb.GeneralEmployeeWagesInformationSteps
                .Where(r => r.TenantId == tenantId)
                .Select(r => new StepDto
                {
                    TenantId = r.TenantId,
                    Id = r.Id
                }).ToList();




            return  step1.
                    Concat(step2).
                    Concat(step3).
                    Concat(step4).
                    ToList();
        }
    }
}
