using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class SendEmailNotificationStep :
        StepBase<SendEmailNotificationStep>
    {

        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }
        [CreateChild]
        private async Task CreateAsync([Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                Name = "Send Email Notification";
                Type = StepType.Automatic;
                StepIndex = 1;
                await Task.CompletedTask;
            }
        }
    }
}
