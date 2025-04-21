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
    internal class EmployeeVariableFactory : ReadOnlyBase<EmployeeVariableFactory>
    {
        public static readonly PropertyInfo<IEmployeeVariable> ResultProperty =
            RegisterProperty<IEmployeeVariable>(nameof(Result));
        public IEmployeeVariable Result
        {
            get { return GetProperty(ResultProperty); }
            private set { LoadProperty(ResultProperty, value); }
        }

        [Fetch]
        private void Fetch(IVariableInfo variable, [Inject] IDataPortal<VariableTypeTypeInfo> dp,
            [Inject] ApplicationContext appCtx)
        {
            var info = dp.Fetch(variable.Id);
            var fullTypeName = info.FullTypeName;
            fullTypeName = fullTypeName.Replace(".Admin", "");
            fullTypeName = fullTypeName.Replace(info.Name, $"Employee{info.Name}");
            Type? t = Type.GetType(fullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var childDp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeVariable)childDp.CreateChild(variable);

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
            fullTypeName = fullTypeName.Replace(info.Name, $"Employee{info.Name}");
            Type? t = Type.GetType(fullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var childDp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeVariable)childDp.FetchChild(employeeId, itemId, id);

        }
    }
}
