using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Types;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    internal class VariableFactory : ReadOnlyBase<VariableFactory>
    {
        public static readonly PropertyInfo<IVariable> ResultProperty =
            RegisterProperty<IVariable>(nameof(Result));
        public IVariable Result
        {
            get { return GetProperty(ResultProperty); }
            private set { LoadProperty(ResultProperty, value); }
        }

        [Fetch]
        private void Fetch(VariableDto data,
            [Inject] IDataPortal<VariableTypeTypeInfo> portal,
            [Inject] ApplicationContext appCtx)
        {

            var info = portal.Fetch(data.Id);
            var t = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IVariable)dp.FetchChild(data);
        }


        [Fetch]
        private void Fetch(int id, string token,
            [Inject] IDataPortal<VariableTypeTypeInfo> portal,
            [Inject] ApplicationContext appCtx)
        {

            var info = portal.Fetch(id);
            var t = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IVariable)dp.CreateChild(id, token);
        }
    }
}
