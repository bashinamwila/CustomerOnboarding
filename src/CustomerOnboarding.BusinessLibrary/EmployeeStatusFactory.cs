using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeeStatusFactory : ReadOnlyBase<EmployeeStatusFactory>
    {
        public static readonly PropertyInfo<IEmployeeStatus> ResultProperty = RegisterProperty<IEmployeeStatus>
      (nameof(Result));
        public IEmployeeStatus Result
        {
            get { return GetProperty(ResultProperty); }
            private set { LoadProperty(ResultProperty, value); }
        }



        [Fetch]
        private void Fetch(string tenantId,int id, string employeeId,string ruleSet,
            [Inject] IDataPortalFactory portalFactory,
            [Inject] ApplicationContext appCtx)
        {
            var _info = portalFactory.GetPortal<CustomerOnboarding.BusinessLibrary.Types.StatusTypeTypeInfo>().Fetch(id);
            var objType = Type.GetType(_info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(objType!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeStatus)dp.FetchChild(tenantId,id, employeeId,ruleSet);
        }

        [Fetch]
        private void Fetch(int id,string ruleSet,
            [Inject] IDataPortalFactory portalFactory,
            [Inject] ApplicationContext appCtx)
        {
            var _info = portalFactory.GetPortal<CustomerOnboarding.BusinessLibrary.Types.StatusTypeTypeInfo>().Fetch(id);
            var objType = Type.GetType(_info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(objType!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeStatus)dp.CreateChild(id,ruleSet);
        }

    }
}
