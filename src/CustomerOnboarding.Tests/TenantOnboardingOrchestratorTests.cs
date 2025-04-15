using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Tests
{
    public class TenantOnboardingOrchestratorTests :
        IClassFixture<CslaTestFixture>
    {
        private readonly IServiceProvider _serviceProvider;
        public TenantOnboardingOrchestratorTests(CslaTestFixture fixture)
        {
            _serviceProvider = fixture.Services;
        }

        private string tenantId = "123werqop070905mnbfghjkl";

        [Fact]
        public async Task Onboard_Tenant_Upto_Step4()
        {
            var portal = _serviceProvider.GetRequiredService<IDataPortal<TenantOnboardingOrchestrator>>();
            var tenantOnboardingOrchestrator = await portal.CreateAsync(tenantId);

            //Step1

            var step1 = (OrganisationProfileStep)tenantOnboardingOrchestrator.Steps[0];

            Assert.Equal(0, tenantOnboardingOrchestrator.CurrentStepIndex);
            Assert.Equal(0, step1.StepIndex);

            step1.OrganisationProfile.AddressLine1 = "Some where in Lusaka";
            step1.OrganisationProfile.AddressLine2 = "Makeni Konga";
            step1.OrganisationProfile.Email = "hello@example.com";
            step1.OrganisationProfile.PhoneNumber = "0977100000";
            step1.OrganisationProfile.City = "Lusaka";

            Assert.True(step1.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 2

            Assert.Equal(1, tenantOnboardingOrchestrator.CurrentStepIndex);

            var step2 = (BankingDetailsStep)tenantOnboardingOrchestrator.Steps[1];

            Assert.Equal(1, step2.StepIndex);

            step2.BankingDetails.BankId = "26";
            step2.BankingDetails.BranchId = 307;
            step2.BankingDetails.BankAccountNumber = "1234567890";

            Assert.True(step2.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 3

            var step3 = (ComplianceInfoStep)tenantOnboardingOrchestrator.Steps[2];

            Assert.Equal(2, tenantOnboardingOrchestrator.CurrentStepIndex);
            Assert.Equal(2, step3.StepIndex);
            Assert.Equal(StepTypes.MultiStep, step3.Type);
            Assert.Equal(0, step3.CurrentStepIndex);

            //Step 3 sub Step 1

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step3 = (ComplianceInfoStep)tenantOnboardingOrchestrator.Steps[2];

            Assert.Equal(2, step3.StepIndex);
            Assert.Equal(1, step3.CurrentStepIndex);

            //Step 3 sub step 2

            var step3SubStep2 = (StatutoryRegistrationsStep)step3.Steps[1];

            step3SubStep2.StatutoryRegistrations.TPIN = "1234567890";
            step3SubStep2.StatutoryRegistrations.NAPSAAccountNumber = "0987654321";
            step3SubStep2.StatutoryRegistrations.NHIMAAccountNumber = "12222222222222";
           // ((ICheckRules)step3SubStep2.StatutoryRegistrations).CheckRules();

            Assert.True(step3SubStep2.IsCompleted);
            Assert.True(step3.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 4

            var step4 = (CompensationComponentsStep)tenantOnboardingOrchestrator.Steps[3];

            Assert.Equal(3, tenantOnboardingOrchestrator.CurrentStepIndex);
            Assert.Equal(0, step4.CurrentStepIndex);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 4 sub step 2
            step4 = (CompensationComponentsStep)tenantOnboardingOrchestrator.Steps[3];

            var step4SubStep2 = (AddCompensationComponentStep)step4.Steps[1];

            Assert.Equal(3, tenantOnboardingOrchestrator.CurrentStepIndex);
            Assert.Equal(1, step4.CurrentStepIndex);

            step4SubStep2.Wage.Id = "1000";
            step4SubStep2.Wage.Name = "Housing Allowance";
            step4SubStep2.Wage.Type = 1;
            step4SubStep2.Wage.Formular = "30% of Basic Salary";

            Assert.True(step4SubStep2.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 4 sub step 3

            step4 = (CompensationComponentsStep)tenantOnboardingOrchestrator.Steps[3];

            var step4SubStep3 = (AddCompensationComponentConfirmationStep)step4.Steps[2];

            step4SubStep3.ActionTaken = ConfirmationActions.AddAnotherItem;

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step4 = (CompensationComponentsStep)tenantOnboardingOrchestrator.Steps[3];
            step4SubStep3 = (AddCompensationComponentConfirmationStep)step4.Steps[2];

            Assert.Equal(1, step4.CurrentStepIndex);

            Assert.False(step4SubStep3.IsCompleted);

            step4SubStep2 = (AddCompensationComponentStep)step4.Steps[1];

            Assert.False(step4SubStep2.IsCompleted);

            step4SubStep2.Wage.Id = "1001";
            step4SubStep2.Wage.Name = "Sales Commission";
            step4SubStep2.Wage.Type = 3;
            step4SubStep2.Wage.Formular = "2.5% of Total Sales";

            Assert.True(step4SubStep2.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step4 = (CompensationComponentsStep)tenantOnboardingOrchestrator.Steps[3];

            Assert.Equal(2, step4.CurrentStepIndex);


            var _portal = _serviceProvider.GetRequiredService<IDataPortal<WageList>>();
            var wageList = await _portal.FetchAsync(tenantId);
            Assert.Equal(2, wageList.Count);


           

            step4SubStep3 = (AddCompensationComponentConfirmationStep)step4.Steps[2];

            step4SubStep3.ActionTaken = ConfirmationActions.IHaveAddedAllTheCurrentItems;

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            Assert.True(tenantOnboardingOrchestrator.IsComplete);
        }
    }
}
