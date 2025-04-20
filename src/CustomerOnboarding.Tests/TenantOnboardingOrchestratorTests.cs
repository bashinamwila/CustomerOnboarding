using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        public async Task Onboard_Tenant()
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

            //Step 5 Deductions Step

            var step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];

            Assert.Equal(4, tenantOnboardingOrchestrator.CurrentStepIndex);
            Assert.Equal(0, step5.CurrentStepIndex);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 5 sub step 2
            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];

            var step5SubStep2 = (AddDeductionStep)step5.Steps[1];

            Assert.Equal(4, tenantOnboardingOrchestrator.CurrentStepIndex);
            Assert.Equal(1, step5.CurrentStepIndex);

            step5SubStep2.Deduction.Id = "2000";
            step5SubStep2.Deduction.Name = "Union Contribution";
            step5SubStep2.Deduction.SetType(1);
            step5SubStep2.Deduction.SetLimit(1);
            ((FixedDeduction)step5SubStep2.Deduction.DeductionType).Amount=54m;

            Assert.True(step5SubStep2.IsCompleted);


            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 5 sub step 3

            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];

            var step5SubStep3 = (AddDeductionConfirmationStep)step5.Steps[2];

            step5SubStep3.ActionTaken = ConfirmationActions.AddAnotherItem;

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];
            step5SubStep3 = (AddDeductionConfirmationStep)step5.Steps[2];

            Assert.Equal(1, step5.CurrentStepIndex);

            Assert.False(step5SubStep3.IsCompleted);

            step5SubStep2 = (AddDeductionStep)step5.Steps[1];

            Assert.False(step5SubStep2.IsCompleted);

            step5SubStep2.Deduction.Id = "2001";
            step5SubStep2.Deduction.Name = "Pension Contribution-Employee";
            step5SubStep2.Deduction.SetType(2);
            step5SubStep2.Deduction.SetLimit(2);
            ((PercentWageDeduction)step5SubStep2.Deduction.DeductionType).Percent = 0.5m;
            ((PercentWageDeduction)step5SubStep2.Deduction.DeductionType).WageId = "1000";

            ((RangeLimitDeduction)step5SubStep2.Deduction.DeductionLimitType).Minimum =100m;
            ((RangeLimitDeduction)step5SubStep2.Deduction.DeductionLimitType).Maximum = 1500;



            Assert.True(step5SubStep2.IsCompleted);


            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 5 sub step 3

            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];

             step5SubStep3 = (AddDeductionConfirmationStep)step5.Steps[2];

            step5SubStep3.ActionTaken = ConfirmationActions.AddAnotherItem;

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];
            step5SubStep2 = (AddDeductionStep)step5.Steps[1];

            step5SubStep2.Deduction.Id = "2003";
            step5SubStep2.Deduction.Name = "Test";
            step5SubStep2.Deduction.SetType(3);
            step5SubStep2.Deduction.SetLimit(1);
            ((PercentGrossDeduction)step5SubStep2.Deduction.DeductionType).Percent = 0.5m;
           



            Assert.True(step5SubStep2.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 5 sub step 3

            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];

             step5SubStep3 = (AddDeductionConfirmationStep)step5.Steps[2];

            step5SubStep3.ActionTaken = ConfirmationActions.AddAnotherItem;

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);


            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];
            step5SubStep2 = (AddDeductionStep)step5.Steps[1];

            step5SubStep2.Deduction.Id = "2004";
            step5SubStep2.Deduction.Name = "Test 2";
            step5SubStep2.Deduction.SetType(4);
            step5SubStep2.Deduction.SetLimit(1);
            




            Assert.True(step5SubStep2.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            //Step 5 sub step 3

            step5 = (DeductionsStep)tenantOnboardingOrchestrator.Steps[4];

            step5SubStep3 = (AddDeductionConfirmationStep)step5.Steps[2];

            step5SubStep3.ActionTaken = ConfirmationActions.IHaveAddedAllTheCurrentItems;

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            Assert.Equal(5, tenantOnboardingOrchestrator.CurrentStepIndex);

            var step6 = (ISkippable)tenantOnboardingOrchestrator.Steps[5];

            step6.IsSkipped = true;

            tenantOnboardingOrchestrator.Skip();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step6 = (ISkippable)tenantOnboardingOrchestrator.Steps[5];

            Assert.True(step6.IsSkipped);


            Assert.Equal(6, tenantOnboardingOrchestrator.CurrentStepIndex);

            //Step 7

            var step7 = (EmployeesStep)tenantOnboardingOrchestrator.Steps[6];

            Assert.Equal(0, step7.CurrentStepIndex);

            //Step7 sub step 1

            var step7SubStep1 = (GeneralEmployeesInformationStep)step7.Steps[0];

            step7SubStep1.InputMethod = 23;

            Assert.Equal(2, step7.Steps.Count);

            await tenantOnboardingOrchestrator.MoveNextAsync();

            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step7 = (EmployeesStep)tenantOnboardingOrchestrator.Steps[6];

            Assert.Equal(1, step7.CurrentStepIndex);
            Assert.Equal(6, tenantOnboardingOrchestrator.CurrentStepIndex);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step7 = (EmployeesStep)tenantOnboardingOrchestrator.Steps[6];

            var step7SubStep2 = (ManualEmployeeDataInputMethodStep)step7.Steps[1];

            Assert.Equal(1, step7.CurrentStepIndex);

            Assert.Equal(1, step7SubStep2.CurrentStepIndex);

            Assert.False(step7SubStep2.IsComplete);

            var step7SubStep2SubStep1 = (AddEmployeeEmploymentDetailsStep)step7SubStep2.Steps[1];

            Assert.False(step7SubStep2SubStep1.IsCompleted);

            step7SubStep2SubStep1.EmployeeEmploymentDetails.EmployeeId = "1495";
            step7SubStep2SubStep1.EmployeeEmploymentDetails.FirstName = "Dennis";
            step7SubStep2SubStep1.EmployeeEmploymentDetails.LastName = "Mwape";
            step7SubStep2SubStep1.EmployeeEmploymentDetails.HireDate.DayPart = 1;
            step7SubStep2SubStep1.EmployeeEmploymentDetails.HireDate.MonthPart = 4;
            step7SubStep2SubStep1.EmployeeEmploymentDetails.HireDate.YearPart = 2025;
            step7SubStep2SubStep1.EmployeeEmploymentDetails.JobTitle = 1;
            step7SubStep2SubStep1.EmployeeEmploymentDetails.DeptId = 1;
            step7SubStep2SubStep1.EmployeeEmploymentDetails.SetStatus(1);
            step7SubStep2SubStep1.EmployeeEmploymentDetails.EmploymentType = 2;
            ((ContractEmployee)step7SubStep2SubStep1.EmployeeEmploymentDetails.EmployeeType).ContractDuration = 24;
            ((ContractEmployee)step7SubStep2SubStep1.EmployeeEmploymentDetails.EmployeeType).Interval = 2;
            ((ContractEmployee)step7SubStep2SubStep1.EmployeeEmploymentDetails.EmployeeType).ContractStartDate.DayPart = 1;
            ((ContractEmployee)step7SubStep2SubStep1.EmployeeEmploymentDetails.EmployeeType).ContractStartDate.MonthPart = 4;
            ((ContractEmployee)step7SubStep2SubStep1.EmployeeEmploymentDetails.EmployeeType).ContractStartDate.YearPart = 2025;

            step7SubStep2SubStep1.EmployeeEmploymentDetails.Group = 1;
            step7SubStep2SubStep1.EmployeeEmploymentDetails.PayType = 1;
            step7SubStep2SubStep1.EmployeeEmploymentDetails.ReportsTo = 2;


            Assert.True(step7SubStep2SubStep1.IsCompleted);
            Assert.Equal("Dennis Mwape", step7SubStep2SubStep1.EmployeeEmploymentDetails.FullName);
            Assert.Equal(new DateTime(2027, 3, 31), ((ContractEmployee)step7SubStep2SubStep1.EmployeeEmploymentDetails.EmployeeType).ContractExpiryDate);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();

            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);


            step7 = (EmployeesStep)tenantOnboardingOrchestrator.Steps[6];

             step7SubStep2 = (ManualEmployeeDataInputMethodStep)step7.Steps[1];

            Assert.Equal(6, tenantOnboardingOrchestrator.CurrentStepIndex);
            Assert.Equal(1, step7.CurrentStepIndex);
            Assert.Equal(2, step7SubStep2.CurrentStepIndex);

            var step7SubStep2SubStep3 = (AddEmployeePersonalDetailsStep)step7SubStep2.Steps[2];

            Assert.False(step7SubStep2SubStep3.IsCompleted);

            step7SubStep2SubStep3.EmployeePersonalDetails.AddressLine1 = "Plot No. 4000,Makeni Road";
            step7SubStep2SubStep3.EmployeePersonalDetails.AddressLine2 = "Makeni Konga,Lusaka";
            step7SubStep2SubStep3.EmployeePersonalDetails.TPIN = "1234567890";
            step7SubStep2SubStep3.EmployeePersonalDetails.SSN = "9876543210";
            step7SubStep2SubStep3.EmployeePersonalDetails.NHIMAAccountNumber = "1212";
            step7SubStep2SubStep3.EmployeePersonalDetails.DateOfBirth.DayPart = 31;
            step7SubStep2SubStep3.EmployeePersonalDetails.DateOfBirth.MonthPart = 8;
            step7SubStep2SubStep3.EmployeePersonalDetails.DateOfBirth.YearPart = 1970;
            step7SubStep2SubStep3.EmployeePersonalDetails.EmailAddress = "dennis.mwape@africaninternetgroup.com";
            step7SubStep2SubStep3.EmployeePersonalDetails.PhoneNo = "0978652590";
            step7SubStep2SubStep3.EmployeePersonalDetails.NextOfKin = "Chama Kaunda";
            step7SubStep2SubStep3.EmployeePersonalDetails.RelationWithNextOfKin = "Wife";
            step7SubStep2SubStep3.EmployeePersonalDetails.NextOfKinPhoneNo = "0965377112";
            step7SubStep2SubStep3.EmployeePersonalDetails.Gender = 1;
            step7SubStep2SubStep3.EmployeePersonalDetails.MaritalStatus = 2;
            step7SubStep2SubStep3.EmployeePersonalDetails.IdType = 1;
            step7SubStep2SubStep3.EmployeePersonalDetails.Id = "276765/61/1";
            step7SubStep2SubStep3.EmployeePersonalDetails.Nationality = 17;

            Assert.True(step7SubStep2SubStep3.IsCompleted);

            await tenantOnboardingOrchestrator.MoveNextAsync();
            tenantOnboardingOrchestrator = await tenantOnboardingOrchestrator.SaveAsync();
            tenantOnboardingOrchestrator = await portal.FetchAsync(tenantId);

            step7 = (EmployeesStep)tenantOnboardingOrchestrator.Steps[6];

            Assert.True(step7.IsCompleted);

            Assert.True(tenantOnboardingOrchestrator.IsComplete);

















            // Assert.True(tenantOnboardingOrchestrator.IsComplete);

        }
    }
}
