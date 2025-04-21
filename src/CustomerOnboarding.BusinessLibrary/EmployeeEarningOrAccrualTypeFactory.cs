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
    public class EmployeeEarningOrAccrualTypeFactory
        : ReadOnlyBase<EmployeeEarningOrAccrualTypeFactory>
    {
        public static readonly PropertyInfo<IEmployeeEarningOrAccrualType> ResultProperty =
            RegisterProperty<IEmployeeEarningOrAccrualType>(nameof(Result));
        public IEmployeeEarningOrAccrualType Result
        {
            get => GetProperty(ResultProperty);
            private set => LoadProperty(ResultProperty, value);
        }

        [Fetch]
        private void Fetch(IEarningInfo info, string type,
            [Inject] IDataPortalFactory portal,
            [Inject] ApplicationContext appCtx)
        {

            string typeName = "";
            if (type == "WAGE")
            {
                var t = portal.GetPortal<WageTypeTypeInfo>().Fetch(info.Type);
                typeName = t.FullTypeName;
                typeName = typeName.Replace($"{t.Name}", $"Employee{t.Name}")
                            .Replace(".Admin", "");
            }
            else
            {
                if (type == "ACCRUAL")
                {
                    var t = portal.GetPortal<AccrualTypeTypeInfo>().Fetch(info.Type);
                    typeName = t.FullTypeName;
                    typeName = typeName.Replace($"{t.Name}", $"Employee{t.Name}")
                         .Replace(".Admin", ""); ;
                }
            }

            var earningType = Type.GetType(typeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(earningType!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeEarningOrAccrualType)dp.CreateChild(info);

        }

        [Fetch]
        private void Fetch(int id, string formular,
            string employeeId, string wageId,
            [Inject] ApplicationContext appCtx,
            [Inject] IDataPortal<WageTypeTypeInfo> portal)
        {
            var type = portal.Fetch(id);
            var typeName = type.FullTypeName;
            typeName = typeName.Replace($"{type.Name}", $"Employee{type.Name}")
                       .Replace(".Admin", "");
            var t = Type.GetType(typeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeEarningOrAccrualType)dp.FetchChild(id, formular, employeeId, wageId);

        }

        [Fetch]
        private void Fetch(int type, string employeeId, string id,
            [Inject] ApplicationContext appCtx, [Inject] IDataPortal<AccrualTypeTypeInfo> portal)
        {
            var accrualType = portal.Fetch(type);
            var accrualTypeName = accrualType.FullTypeName;
            accrualTypeName = accrualTypeName.Replace($"{accrualType.Name}", $"Employee{accrualType.Name}")
                       .Replace(".Admin", "");
            var t = Type.GetType(accrualTypeName);
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(t!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            Result = (IEmployeeEarningOrAccrualType)dp.FetchChild(employeeId, id);
        }

    }
}
