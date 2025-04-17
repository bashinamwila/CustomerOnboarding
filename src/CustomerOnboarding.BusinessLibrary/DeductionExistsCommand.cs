using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class DeductionExistsCommand : CommandBase<DeductionExistsCommand>
    {
        public static readonly PropertyInfo<bool> ExistsProperty =
            RegisterProperty<bool>(nameof(Exists));
        public bool Exists
        {
            get => ReadProperty(ExistsProperty);
            private set => LoadProperty(ExistsProperty, value);
        }

        
        [Execute]
        private void Execute(string tenantId,string id,[Inject] IDeductionDal dal)
        {
            Exists = dal.Exists(tenantId,id);
        }
    }
}
