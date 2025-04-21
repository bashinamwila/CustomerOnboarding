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
    public class WageExistsCommand : CommandBase<WageExistsCommand>
    {
        public static readonly PropertyInfo<bool> ExistsProperty =
            RegisterProperty<bool>(nameof(Exists));
        public bool Exists
        {
            get => ReadProperty(ExistsProperty);
            private set => LoadProperty(ExistsProperty, value);
        }

        
        [Execute]
        private void Execute(string id,[Inject] IWageDal dal)
        {
            Exists = dal.Exists(id);
        }

    }
}
