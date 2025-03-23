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
    public class CreateAccountStep :
        StepBase<CreateAccountStep>
    {
        [CreateChild]
        private async Task CreateAsync([Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                Name = "Create Account";
                Type = StepType.Manual;
                StepIndex = 0;
                await Task.CompletedTask;
            }
        }
    }
}
