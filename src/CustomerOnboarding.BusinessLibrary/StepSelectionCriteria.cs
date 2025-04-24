using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public record StepSelectionCriteria(int StepId,string TenantId, int CurrentStepIndex,int Counter=0);
    
}
