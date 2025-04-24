using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    public class Wages :
        BusinessListBase<Wages,Wage>
    {

        [CreateChild]
        private void Create() { }

        [FetchChild]
        private void Fetch(string tenantId,string ruleSet,
            [Inject]IWageDal dal,
            [Inject]IChildDataPortal<Wage>portal)
        {
            var list = dal.Fetch(tenantId);
            foreach (var item in list)
                this.Add(portal.FetchChild(item, ruleSet));

        }

    }
}
