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
    internal class VariableInfoFactory : ReadOnlyBase<VariableInfoFactory>
    {
        public static readonly PropertyInfo<IVariableInfo> ResultProperty =
            RegisterProperty<IVariableInfo>(nameof(Result));
        public IVariableInfo Result
        {
            get { return GetProperty(ResultProperty); }
            private set { LoadProperty(ResultProperty, value); }
        }
        [Fetch]
        private void Fetch(int id, string itemId,
            [Inject] IDataPortal<VariableTypeTypeInfo> portal,
            [Inject] ApplicationContext appCtx)
        {

            var info = portal.Fetch(id);
            var fullTypeName = info.FullTypeName;
            fullTypeName = fullTypeName.Replace(info.Name, $"{info.Name}Info");
            var t = Type.GetType(fullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IVariableInfo)dp.FetchChild(id, itemId);
        }
    }
}
