using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class Steps :BusinessListBase<Steps, IStep>
    {
        [CreateChild]
        private void Create() { }

        [FetchChild]
        private async Task FetchAsync(string tenantId, [Inject]IDataPortal<StepFactory> portal,
            [Inject]IStepDal dal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            var list=  dal.Fetch(tenantId);
            foreach(var step in list)
            {
                var factory=await portal.FetchAsync(step.TenantId, step.Id);
                this.Add(factory.Result);
            }
            this.RaiseListChangedEvents = rlce;
        }
    }
}
