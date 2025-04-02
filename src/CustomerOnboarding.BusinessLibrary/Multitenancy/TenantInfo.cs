using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Multitenancy
{
    [Serializable]
    public class TenantInfo :
        ReadOnlyBase<TenantInfo>,ITenantInfo
    {
        public static readonly PropertyInfo<string> IdProperty =
            RegisterProperty<string>(nameof(Id));
        public string Id
        {
            get => GetProperty(IdProperty);
            private set=>LoadProperty(IdProperty, value);
        }


        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get => GetProperty(NameProperty);
            private set => LoadProperty(NameProperty, value);
        }


        [Fetch]
        private void Fetch(string id, [Inject]IOrganisationDal dal)
        {
            var dto = dal.Fetch(id);
            Id = dto.Id;
            Name = dto.Name;
        }

    }
}
