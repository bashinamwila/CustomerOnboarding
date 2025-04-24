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
    public class EmployeeVariables : BusinessListBase<EmployeeVariables, IEmployeeVariable>
    {
        public IEmployeeVariable? GetItem(int id)
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




        [CreateChild]
        private void Create(VariableList variables,
            [Inject] IDataPortal<EmployeeVariableFactory> portal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            foreach (var variable in variables)
            {
                var child = portal.Fetch(variable).Result;
                this.Add(child);

            }

            this.RaiseListChangedEvents = rlce;
        }
        


        [FetchChild]
        private void Fetch(string tenantId,string employeeId, string id, [Inject] IEmployeeVariableDal dal,
            [Inject] IDataPortal<EmployeeVariableFactory> portal)
        {
            var rlce = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;
            var list = dal.Fetch(tenantId,employeeId, id);
            foreach (var item in list)
            {
                var child = portal.Fetch(item.EmployeeId, item.ItemId, item.Id).Result;
                this.Add(child);
            }
            this.RaiseListChangedEvents = rlce;
        }

        /*  [FetchChild]
          private void Fetch() { }*/

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent)
        {
            base.Child_Update(parent);
        }

    }   
}
