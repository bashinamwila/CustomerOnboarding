using CustomerOnboarding.Dal;
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
        }
    }
}
