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
    internal class EmployeeTypeFactory : ReadOnlyBase<EmployeeTypeFactory>
    {
        public static readonly PropertyInfo<IEmployeeType> ResultProperty =
            RegisterProperty<IEmployeeType>(nameof(Result));
        public IEmployeeType Result
        {
            get { return GetProperty(ResultProperty); }
            private set { LoadProperty(ResultProperty, value); }
        }

        [Fetch]
        private void Fetch(int id, [Inject] IDataPortalFactory portalFactory)
        {
            var info = portalFactory.GetPortal<CustomerOnboarding.BusinessLibrary.Types.EmployeeTypeTypeInfo>().Fetch(id);
            var type = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);
            var dp = (IChildDataPortal)ApplicationContext.GetRequiredService(dpType);
            Result = (IEmployeeType)dp.CreateChild(id);
        }

        [Fetch]
        private void Fetch(string tenantId,string employeeId, int id,
            [Inject] IDataPortalFactory portalFactory)
        {
            var info = portalFactory.GetPortal<CustomerOnboarding.BusinessLibrary.Types.EmployeeTypeTypeInfo>().Fetch(id);
            var type = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);
            var dp = (IChildDataPortal)ApplicationContext.GetRequiredService(dpType);
            Result = (IEmployeeType)dp.FetchChild(tenantId,employeeId);
        }

    }
}
