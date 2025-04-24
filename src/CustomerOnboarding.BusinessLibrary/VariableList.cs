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
    public class VariableList : ReadOnlyListBase<VariableList, IVariableInfo>
    {
        public IVariableInfo? GetItem(int id)
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
                          select r).Count();
            return result > 0;
        }

        [FetchChild]
        private void Fetch(string tenantId, string id, [Inject] IVariableDal dal,
            [Inject] IDataPortal<VariableInfoFactory> portal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            this.IsReadOnly = false;
            var list = dal.Fetch(tenantId, id);
            foreach (var item in list)
            {
                var variable = portal.Fetch(item.Id, item.ItemId).Result;
                this.Add(variable);
            }
            this.IsReadOnly = true;
            this.RaiseListChangedEvents = rlce;
        }
    }
}
