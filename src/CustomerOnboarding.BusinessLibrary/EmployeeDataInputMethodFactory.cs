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
    public class EmployeeDataInputMethodFactory :
        ReadOnlyBase<EmployeeDataInputMethodFactory>
    {
        public static readonly PropertyInfo<IEmployeeDataInputMethodStep> ResultProperty =
            RegisterProperty<IEmployeeDataInputMethodStep>(nameof(Result));
        public IEmployeeDataInputMethodStep Result
        {
            get => GetProperty(ResultProperty);
            private set => LoadProperty(ResultProperty, value);
        }

        [Fetch]
        private void Fetch(int id,int currentStepIndex, [Inject]ApplicationContext appCtx,
            [Inject]IEmployeeDataInputMethodTypeDal dal)
        {
            var dto = dal.Fetch(id);
            var type = Type.GetType(dto.TypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeDataInputMethodStep)dp.CreateChild(id,currentStepIndex);
        }

        [Fetch]
        private void Fetch(string tenantId,int id,
            [Inject]ApplicationContext appCtx, [Inject] IEmployeeDataInputMethodTypeDal dal)
        {
            var dto = dal.Fetch(id);
            var type = Type.GetType(dto.TypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeDataInputMethodStep)dp.FetchChild(tenantId);
        }
    }
}
