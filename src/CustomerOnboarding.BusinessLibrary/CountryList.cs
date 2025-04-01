using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{

    [Serializable]
    public class CountryList : ReadOnlyListBase<CountryList, CountryInfo>
    {
        public bool Contains(string id)
        {
            var count = (from r in this
                         where r.Id == id
                         select r).Count();
            return count > 0;
        }
        [Fetch]
        private void Fetch([Inject] ICountryDal dal,
            [Inject] IChildDataPortal<CountryInfo> portal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            IsReadOnly = false;
            var list = dal.Fetch();
            foreach (var item in list)
            {
                this.Add(portal.FetchChild(item));
            }
            this.RaiseListChangedEvents = rlce;
            this.IsReadOnly = true;
        }
    }
}
