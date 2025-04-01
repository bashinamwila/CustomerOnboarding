using Csla;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class CountryInfo : ReadOnlyBase<CountryInfo>
    {
        public static readonly PropertyInfo<string> IdProperty =
            RegisterProperty<string>(nameof(Id));
        public string Id
        {
            get => GetProperty(IdProperty);
            private set => LoadProperty(IdProperty, value);
        }
        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get => GetProperty(NameProperty);
            private set => LoadProperty(NameProperty, value);
        }

        [FetchChild]
        private void Fetch(CountryDto data)
        {
            Id = data.Id;
            Name = data.Name;
        }
    }
}
