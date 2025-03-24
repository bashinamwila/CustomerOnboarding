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
        static MockDb()
        {
            Steps = new List<StepEntity>
            {
                new StepEntity{Id=1,Name="Create Account",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.CreateAccountStep,CustomerOnboarding.BusinessLibrary"},
                new StepEntity{Id=2,Name="Send Email Notification",Type=2,FullTypeName="CustomerOnboarding.BusinessLibrary.SendEmailNotificationStep,CustomerOnboarding.BusinessLibrary"},
                new StepEntity{Id=3,Name="Confirm Email",Type=1,FullTypeName="CustomerOnboarding.BusinessLibrary.ConfirmEmailStep,CustomerOnboarding.BusinessLibrary"}
            };
        }

    }
}
