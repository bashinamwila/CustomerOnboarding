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
    internal class DeductionTypeFactory : ReadOnlyBase<DeductionTypeFactory>
    {
        public static readonly PropertyInfo<IDeductionType> ResultProperty =
            RegisterProperty<IDeductionType>(nameof(Result));
        public IDeductionType Result
        {
            get { return GetProperty(ResultProperty); }
            private set { LoadProperty(ResultProperty, value); }
        }

        [Fetch]
        private void Fetch(int id,string ruleSet, [Inject] IDataPortal<DeductionTypeTypeInfo> dp,
            [Inject] ApplicationContext appCtx)
        {
            var info = dp.Fetch(id);
            Type? t = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var childDp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IDeductionType)childDp.CreateChild(ruleSet);

        }

        [Fetch]
        private void Fetch(int type, string tenantId,string id,
            string ruleSet,
            [Inject] IDataPortal<DeductionTypeTypeInfo> dp,
            [Inject] ApplicationContext appCtx)
        {
            var info = dp.Fetch(type);
            Type? t = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var childDp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IDeductionType)childDp.FetchChild(tenantId,id,ruleSet);

        }
    }
}
