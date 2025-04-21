using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeePersonalDetailsFactory :
        ReadOnlyBase<EmployeePersonalDetailsFactory>
    {
        public static readonly PropertyInfo<EmployeePersonalDetails> ResultProperty =
           RegisterProperty<EmployeePersonalDetails>(nameof(Result));
        public EmployeePersonalDetails Result
        {
            get => GetProperty(ResultProperty);
            private set => LoadProperty(ResultProperty, value);

        }

        [Fetch]
        private void Fetch(string tenantId, string employeeId, string ruleSet,
            [Inject] IChildDataPortal<EmployeePersonalDetails> portal,
            [Inject]IDataPortal<EmployeePersonalDetailsExistsCommand>factory)
        {
            var cmd = factory.Execute(employeeId);
            if (cmd.Exists)
                Result = portal.FetchChild(tenantId, employeeId, ruleSet);
            else
                Result = portal.CreateChild(ruleSet);
        }
    }
}
