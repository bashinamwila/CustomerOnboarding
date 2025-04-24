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
    public class BranchList : ReadOnlyListBase<BranchList, BranchInfo>
    {
        public BranchInfo? GetItem(int id)
        {
            var result = (from r in this
                          where r.Id == id
                          select r).FirstOrDefault();
            return result;
        }

        public bool Contains(int id)
        {
            var result = (from r in this
                          where r.Id == id
                          select r.Id).Count();
            return result > 0;
        }

        [FetchChild]
        private void Fetch(string id, [Inject] IBranchDal dal,
            [Inject] IChildDataPortal<BranchInfo> portal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            this.IsReadOnly = false;
            var list = dal.Fetch(id);
            foreach (var item in list)
                this.Add(portal.FetchChild(item));
            this.RaiseListChangedEvents = rlce;
            this.IsReadOnly = true;
        }

        /*
        [FetchChild]
        private void Fetch(List<BranchModel> branches,
            [Inject] IChildDataPortal<BranchInfo> portal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            this.IsReadOnly = false;
            foreach (var branch in branches)
                this.Add(portal.FetchChild(branch));
            this.RaiseListChangedEvents = rlce;
            this.IsReadOnly = true;
        }
        */
    }
}
