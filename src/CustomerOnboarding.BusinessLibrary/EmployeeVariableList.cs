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
    public class EmployeeVariableList : ReadOnlyListBase<EmployeeVariableList, IEmployeeVariableInfo>
    {
        public IEmployeeVariableInfo? GetItem(int id)
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

        public int? GetIdFromToken(string token)
        {
            var id = (from r in this
                      where r.Token == token
                      select r.Id).FirstOrDefault();
            return id;
        }

        [FetchChild]
        private void Fetch(string tenantId,string employeeId, string id, [Inject] IEmployeeVariableDal dal,
           [Inject] IDataPortal<EmployeeVariableInfoFactory> portal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            this.IsReadOnly = false;
            var list = dal.Fetch(tenantId,employeeId, id);
            foreach (var item in list)
            {
                var child = portal.Fetch(item.EmployeeId, item.ItemId, item.Id).Result;
                this.Add(child);
            }
            this.IsReadOnly = true;
            this.RaiseListChangedEvents = rlce;
        }
    }
}
