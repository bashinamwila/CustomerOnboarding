using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeeVariableInfoFactory : ReadOnlyBase<EmployeeVariableInfoFactory>
    {
        public static readonly PropertyInfo<IEmployeeVariableInfo> ResultProperty =
            RegisterProperty<IEmployeeVariableInfo>(nameof(Result));
        public IEmployeeVariableInfo Result
        {
            get { return GetProperty(ResultProperty); }
            private set { LoadProperty(ResultProperty, value); }
        }

        [Fetch]
        private void Fetch(string employeeId,
            string itemId,
            int id,
            [Inject] IDataPortal<VariableTypeTypeInfo> dp,
           [Inject] ApplicationContext appCtx)
        {
            var info = dp.Fetch(id);
            var fullTypeName = info.FullTypeName;
            fullTypeName = fullTypeName.Replace(".Admin", "Variables");
            fullTypeName = fullTypeName.Replace(info.Name, $"Employee{info.Name}Info");
            Type? t = Type.GetType(fullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var childDp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeVariableInfo)childDp.FetchChild(employeeId, itemId, id);

        }
    }
}
