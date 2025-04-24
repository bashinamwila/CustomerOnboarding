using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeeEmploymentDetailsFactory :
        ReadOnlyBase<EmployeeEmploymentDetailsFactory>
    {
        public static readonly PropertyInfo<EmployeeEmploymentDetails> ResultProperty =
            RegisterProperty<EmployeeEmploymentDetails>(nameof(Result));
        public EmployeeEmploymentDetails Result
        {
            get => GetProperty(ResultProperty);
            private set => LoadProperty(ResultProperty, value);

        }

        [Fetch]
        private void Fetch(string tenantId,string employeeId,string ruleSet,
            [Inject]IChildDataPortal<EmployeeEmploymentDetails> portal)
        {
            if (!string.IsNullOrEmpty(employeeId))
                Result = portal.FetchChild(tenantId, employeeId,ruleSet);
            else
                Result = portal.CreateChild(ruleSet);
        }
    }
}
