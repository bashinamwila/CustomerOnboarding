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
        public static List<CustomerOnboardingEntity> Customers { get; private set; } = default!;
        public static List<CreateAccountStepEntity>CreateAccounts { get; private set; }=default!;
        public static List<SendEmailNotificationStepEntity> SendEmailNotifications { get; private set; } = default!;
        public static List<OrganisationEntity> Organisations { get; private set; } = default!;
        public static List<UserEntity> Users { get; private set; } = default!;
        static MockDb()
        {
            Steps = new List<StepEntity>
            {
                new StepEntity{Id=1,Name="Create Account",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.CreateAccountStep,CustomerOnboarding.BusinessLibrary",RuleSet="Create Account",LastChanged=GetTimeStamp()},
                new StepEntity{Id=2,Name="Send Email Notification",Type=2,FullTypeName="CustomerOnboarding.BusinessLibrary.SendEmailNotificationStep,CustomerOnboarding.BusinessLibrary",LastChanged=GetTimeStamp()},
                new StepEntity{Id=3,Name="Confirm Email",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.ConfirmEmailStep,CustomerOnboarding.BusinessLibrary", LastChanged = GetTimeStamp()}
            };
            Customers = new List<CustomerOnboardingEntity>();
            CreateAccounts = new List<CreateAccountStepEntity>();
            SendEmailNotifications = new List<SendEmailNotificationStepEntity>();
            Organisations =new();
            Users = new();
        }

        private static long _lastTimeStamp = 1;

        public static byte[] GetTimeStamp()
        {
            var stamp = System.Threading.Interlocked.Add(ref _lastTimeStamp, 1);
            return System.Text.ASCIIEncoding.ASCII.GetBytes(stamp.ToString());
        }

    }
}
