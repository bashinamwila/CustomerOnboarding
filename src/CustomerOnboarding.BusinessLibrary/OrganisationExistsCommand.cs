using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class OrganisationExistsCommand :
        CommandBase<OrganisationExistsCommand>
    {
        public static readonly PropertyInfo<bool> ExistsProperty =
            RegisterProperty<bool>(nameof(Exists));
        public bool Exists
        {
            get => ReadProperty(ExistsProperty);
            private set => LoadProperty(ExistsProperty, value);
        }

        public static readonly PropertyInfo<string> IdProperty =
           RegisterProperty<string>(nameof(Id));
        public string Id
        {
            get => ReadProperty(IdProperty);
            private set => LoadProperty(IdProperty, value);
        }

        [Create]
        private void Create(string id) => Id = id;
        /*
        [Execute]
        private void Execute([Inject] IOrganisationDal dal)
        {
            Exists = dal.Exists(Id);
        }
        */
    }
}
