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
    public class WageList : ReadOnlyListBase<WageList, WageInfo>
    {
        public WageInfo? GetItem(string id)
        {
            var item = (from _ in this
                        where _.Id == id
                        select _).FirstOrDefault();
            return item;
        }

        public bool Contains(string id)
        {
            var count = (from _ in this
                         where _.Id == id
                         select _.Id).Count();
            return count > 0;
        }

        public string? GetId(string name)
        {
            var id = (from _ in this
                      where _.Name.ToLower() == name.ToLower()
                      select _.Id).FirstOrDefault();
            return id;
        }



        [Fetch]
        private void Fetch(string tenantId,[Inject] IWageDal dal,
            [Inject] IChildDataPortal<WageInfo> dp)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            this.IsReadOnly = false;
            var list = dal.Fetch(tenantId);
            foreach (var item in list)
                this.Add(dp.FetchChild(item));
            this.IsReadOnly = rlce;
            this.RaiseListChangedEvents = rlce;
        }
    }
}
