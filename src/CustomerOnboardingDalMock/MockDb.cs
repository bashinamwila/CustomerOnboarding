using CustomerOnboarding.DalMock.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public static class MockDb
    {
        public static List<StepEntity> Steps { get; private set; } = default!;
        public static List<UserOnboardingEntity> Customers { get; private set; } = default!;
        public static List<CreateAccountStepEntity>CreateAccounts { get; private set; }=default!;
        public static List<SendEmailNotificationStepEntity> SendEmailNotifications { get; private set; } = default!;
        public static List<OrganisationEntity> Organisations { get; private set; } = default!;
        public static List<UserEntity> Users { get; private set; } = default!;
        public static List<CountryEntity> Countries { get; private set; } = default!;
        public static List<EmailTemplateEntity> EmailTemplates { get; private set; }
        public static List<ConfirmEmailStepEntity> EmailConfirmations { get; private set;} = default!;  
        static MockDb()
        {
            Steps = new List<StepEntity>
            {
                new StepEntity{Id=1,Name="Create Account",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.CreateAccountStep,CustomerOnboarding.BusinessLibrary",RuleSet="Create Account",LastChanged=GetTimeStamp()},
                new StepEntity{Id=2,Name="Send Email Notification",Type=2,FullTypeName="CustomerOnboarding.BusinessLibrary.SendEmailNotificationStep,CustomerOnboarding.BusinessLibrary",LastChanged=GetTimeStamp()},
                new StepEntity{Id=3,Name="Confirm Account",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.ConfirmEmailStep,CustomerOnboarding.BusinessLibrary",RuleSet="Confirm Email", LastChanged = GetTimeStamp()}
            };
            Customers = new List<UserOnboardingEntity>();
            CreateAccounts = new List<CreateAccountStepEntity>();
            SendEmailNotifications = new List<SendEmailNotificationStepEntity>();
            EmailConfirmations = new List<ConfirmEmailStepEntity>();
            Organisations =new();
            Users = new();
            Countries = new List<CountryEntity>
            {
                new CountryEntity
                {
                    Id = "ZM",
                    Name="Zambia",
                    LastChanged=GetTimeStamp()
                },
                new CountryEntity
                {
                    Id = "ZA",
                    Name="South Africa",
                    LastChanged=GetTimeStamp()
                },
                new CountryEntity
                {
                    Id = "NG",
                    Name="Nigeria",
                    LastChanged=GetTimeStamp()
                },
                new CountryEntity
                { Id = "KR",
                Name="Kenya",
                LastChanged=GetTimeStamp()
                },
                new CountryEntity
                {
                    Id = "GH",
                    Name="Ghana",
                    LastChanged=GetTimeStamp()
                }


            };
            EmailTemplates = new List<EmailTemplateEntity>
            {
                new EmailTemplateEntity
                {
                    Id=1,
                    TemplateName="ConfirmEmailTemplate",
                    AssemblyQualifiedName="CustomerOnboarding.BusinessLibrary.Templates.Email.EmailConfirmationTemplate,CustomerOnboarding.BusinessLibrary",
                    LastChanged=GetTimeStamp()
                }
            };

        }

        private static long _lastTimeStamp = 1;

        public static byte[] GetTimeStamp()
        {
            var stamp = System.Threading.Interlocked.Add(ref _lastTimeStamp, 1);
            return System.Text.ASCIIEncoding.ASCII.GetBytes(stamp.ToString());
        }

    }
}
