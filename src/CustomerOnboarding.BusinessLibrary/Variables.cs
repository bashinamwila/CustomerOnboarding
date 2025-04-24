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
    public class Variables : BusinessListBase<Variables, IVariable>
    {
        public IVariable? GetItem(int id)
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


        public IVariable? AddChild(int id, string token)
        {
            var dp = ApplicationContext.GetRequiredService<IDataPortal<VariableFactory>>();
            IVariable? variable = null;
            if (!Contains(id))
            {

                variable = dp.Fetch(id, token).Result;
                Add(variable);
            }

            return variable;
        }

        [CreateChild]
        private void Create()
        {

        }

        [UpdateChild]
        private void Update(string id, string type)
        {
            base.Child_Update(id, type);
        }

        [FetchChild]
        private void Fetch(string tenantId,string id, [Inject] IVariableDal dal,
             [Inject] IDataPortal<VariableFactory> portal)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;


            var list = dal.Fetch(tenantId,id);
            foreach (var item in list)
            {
                var variable = portal.Fetch(item).Result;
                Add(variable);
            }
            RaiseListChangedEvents = rlce;
        }
    }
}
