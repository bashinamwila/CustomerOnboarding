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
    public class StepFactory :
        ReadOnlyBase<StepFactory>
    {
        public static readonly PropertyInfo<IStep> ResultProperty =
            RegisterProperty<IStep>(nameof(Result));
        public IStep Result
        {
            get => GetProperty(ResultProperty);
            private set => LoadProperty(ResultProperty, value);
        }


        [Fetch]
        private async Task FetchAsync(int id,
            [Inject]IDataPortalFactory portal,
            [Inject]ApplicationContext appCtx)
        {
            var info= await portal.GetPortal<StepTypeTypeInfo>().FetchAsync(id);
            var type = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IStep)await dp.CreateChildAsync(id);
        }

        [Fetch]
        private async Task FetchAsync(string tenantId,int id,
            [Inject] IDataPortalFactory portal,
            [Inject] ApplicationContext appCtx)
        {
            var info = await portal.GetPortal<StepTypeTypeInfo>().FetchAsync(id);
            var type = Type.GetType(info.FullTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IStep)await dp.FetchChildAsync(tenantId,id);
        }

    }
}
