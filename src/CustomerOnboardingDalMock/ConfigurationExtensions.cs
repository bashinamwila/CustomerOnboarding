using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public static class ConfigurationExtensions
    {
        public static void AddDalMock(this IServiceCollection services)
        {
            services.AddTransient<IStepTypeDal, StepTypeDal>();
            services.AddTransient<IUserOnboardingOrchestratorDal,UserOnboardingOrchestratorDal>();
            services.AddTransient<ICreateAccountStepDal, CreateAccountStepDal>();
            services.AddTransient<IStepDal, StepDal>();
            services.AddTransient<ISendEmailNotificationStepDal, SendEmailNotificationStepDal>();
            services.AddTransient<IOrganisationDal, OrganisationDal>();
            services.AddTransient<IUserDal, UserDal>();
            services.AddTransient<ICountryDal, CountryDal>();
            services.AddTransient<IEmailTemplateDal, EmailTemplateDal>();
            services.AddTransient<IConfirmEmailStepDal, ConfirmEmailStepDal>();
            services.AddTransient<ITenantOnboardingOrchestratorDal, TenantOnboardingOrchestratorDal>();
            services.AddTransient<IOrganisationProfileStepDal,OrganisationProfileStepDal>();
            services.AddTransient<IOrganisationProfileDal, OrganisationProfileDal>(); 
            services.AddTransient<IBankingDetailsStepDal, BankingDetailsStepDal>();
            services.AddTransient<IBankingDetailsDal, BankingDetailsDal>();
            services.AddTransient<ITenantOnboardingStepsDal, TenantOnboardingStepsDal>();
            services.AddTransient<IBankDal, BankDal>();
            services.AddTransient<IBranchDal, BranchDal>();
            services.AddTransient<IComplianceInfoStepDal, ComplianceInfoStepDal>();
            services.AddTransient<IStatutoryRegistrationsDal, StatutoryRegistrationsDal>();
            services.AddTransient<IStatutoryRegistrationsStepDal, StatutoryRegistrationsStepDal>();
            services.AddTransient<IGeneralComplianceInformationDal, GeneralComplianceInformationDal>();
            services.AddTransient<IComplianceInfoStepStepsDal, ComplianceInfoStepStepsDal>();
            services.AddTransient<ICompensationComponentsStepStepsDal, CompensationComponentsStepStepsDal>();
            services.AddTransient<ICompensationComponentsStepDal, CompensationComponentsStepDal>();
            services.AddTransient<IGeneralCompensationComponentsInformationDal, GeneralCompensationComponentsInformationDal>();
            services.AddTransient<IWageDal, WageDal>();
            services.AddTransient<IAddCompensationComponentStepDal, AddCompensationComponentStepDal>();
            services.AddTransient<IAddCompensationComponentConfirmationStepDal, AddCompensationComponentConfirmationStepDal>();
            services.AddTransient<IDeductionsStepDal,DeductionsStepDal>();
            services.AddTransient<IGeneralDeductionsInformationDal,GeneralDeductionsInformationDal>();
            services.AddTransient<IAddDeductionConfirmationStepDal, AddDeductionConfirmationStepDal>();
            services.AddTransient<IAddDeductionsStepDal, AddDeductionsStepDal>();
            services.AddTransient<IDeductionDal,DeductionDal>();
            services.AddTransient<IFixedDeductionDal, FixedDeductionDal>();
            services.AddTransient<IPercentGrossDeductionDal, PercentGrossDeductionDal>();
            services.AddTransient<IPercentWageDeductionDal, PercentWageDeductionDal>();
            services.AddTransient<IUserEnteredDeductionDal, UserEnteredDeductionDal>();
            services.AddTransient<INoDeductionLimitDal,NoDeductionLimitDal>();
            services.AddTransient<IRangeDeductionLimitDal,RangeDeductionLimitDal>(); 
            services.AddTransient<IDeductionsStepStepsDal,DeductionsStepStepsDal>();
            services.AddTransient<IDeductionLimitTypeDal, DeductionLimitTypeDal>();
            services.AddTransient<IDeductionTypeDal, DeductionTypeDal>();
            services.AddTransient<IEmployerExpensesStepDal, EmployerExpensesStepDal>();
            services.AddTransient<IGeneralEmployerExpensesInformationDal, GeneralEmployerExpensesInformationDal>();
            services.AddTransient<IAddEmployerExpenseConfirmationStepDal, AddEmployerExpenseConfirmationStepDal>();
            services.AddTransient<IAddEmployerExpensesStepDal, AddEmployerExpensesStepDal>();
            services.AddTransient<IEmployerExpenseDal, EmployerExpenseDal>();
            services.AddTransient<IFixedEmployerExpenseDal, FixedEmployerExpenseDal>();
            services.AddTransient<IPercentGrossEmployerExpenseDal, PercentGrossEmployerExpenseDal>();
            services.AddTransient<IPercentWageEmployerExpenseDal, PercentWageEmployerExpenseDal>();
            services.AddTransient<IUserEnteredEmployerExpenseDal, UserEnteredEmployerExpenseDal>();
            services.AddTransient<INoEmployerExpenseLimitDal, NoEmployerExpenseLimitDal>();
            services.AddTransient<IRangeEmployerExpenseLimitDal, RangeEmployerExpenseLimitDal>();
            services.AddTransient<IEmployerExpensesStepStepsDal, EmployerExpensesStepStepsDal>();
            services.AddTransient<IEmployerExpenseLimitTypeDal, EmployerExpenseLimitTypeDal>();
            services.AddTransient<IEmployerExpenseTypeDal, EmployerExpenseTypeDal>();
            services.AddTransient<IAddEmployeeEmploymentDetailsStepDal, AddEmployeeEmploymentDetailsStepDal>();
            services.AddTransient<IAddEmployeePersonalDetailsStepDal, AddEmployeePersonalDetailsStepDal>();
            services.AddTransient<IEmployeeDataInputMethodTypeDal, EmployeeDataInputMethodTypeDal>();
            services.AddTransient<IEmployeeEmploymentDetailsDal, EmployeeEmploymentDetailsDal>();
            services.AddTransient<IEmployeePersonalDetailsDal, EmployeePersonalDetailsDal>();
            services.AddTransient<IEmployeesStepDal, EmployeesStepDal>();
            services.AddTransient<IEmployeesStepStepsDal, EmployeesStepStepsDal>();
            services.AddTransient<IEmployeeTypeTypeDal, EmployeeTypeTypeDal>();
            services.AddTransient<IManualEmployeeDataInputMethodStepDal, ManualEmployeeDataInputMethodStepDal>();
            services.AddTransient<IManualEmployeeDataInputMethodStepStepsDal, ManualEmployeeDataInputMethodStepStepsDal>();
            services.AddTransient<IStatusTypeDal, StatusTypeDal>();
            services.AddTransient<IActiveEmployeeStatusDal, ActiveEmployeeStatusDal>();
            services.AddTransient<INewEmployeeStatusDal, NewEmployeeStatusDal>();
            services.AddTransient<IInActiveEmployeeStatusDal, InActiveEmployeeStatusDal>();
            services.AddTransient<IContractEmployeeDal, ContractEmployeeDal>();
            services.AddTransient<IPermanentEmployeeDal, PermanentEmployeeDal>();
            services.AddTransient<ITemporalEmployeeDal, TemporalEmployeeDal>();
            services.AddTransient<IGeneralEmployeesInformationDal, GeneralEmployeesInformationDal>();
            services.AddTransient<IGeneralEmployeeDetailsInformationStepDal, GeneralEmployeeDetailsInformationStepDal>();
            services.AddTransient<IEmployeeStatusDal, EmployeeStatusDal>();
            services.AddTransient<IAddEmployeeWageConfirmationStepDal, AddEmployeeWageConfirmationStepDal>();
            services.AddTransient<IAddEmployeeWageStepDal, AddEmployeeWageStepDal>();
            services.AddTransient<IEmployeeWagesStepDal, EmployeeWagesStepDal>();
            services.AddTransient<ISelectEmployeeWageStepDal, SelectEmployeeWageStepDal>();
            services.AddTransient<IVariableTypeDal, VariableTypeDal>();
            services.AddTransient<IVariableDal, VariableDal>();
            services.AddTransient<IGeneralEmployeeWagesInformationStepDal, GeneralEmployeeWagesInformationStepDal>();


        }
    }
}
