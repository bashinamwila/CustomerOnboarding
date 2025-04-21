using Csla;
using CustomerOnboarding.BusinessLibrary.Types;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeePersonalDetailsExistsCommand :
        CommandBase<EmployeePersonalDetailsExistsCommand>
    {
        public static readonly PropertyInfo<bool> ExistsProperty =
            RegisterProperty<bool>(nameof(Exists));
        public bool Exists
        {
            get => ReadProperty(ExistsProperty);
            private set => LoadProperty(ExistsProperty, value);
        }

        [Execute]
        private void Execute(string employeeId,
            [Inject]IEmployeePersonalDetailsDal dal)
        {
            Exists = dal.Exists(employeeId);
        }
    }
}
