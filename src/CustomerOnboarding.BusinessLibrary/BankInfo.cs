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
    public class BankInfo : ReadOnlyBase<BankInfo>
    {
        public static readonly PropertyInfo<string> IdProperty =
            RegisterProperty<string>(nameof(Id));
        public string Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get { return GetProperty(NameProperty); }
            private set { LoadProperty(NameProperty, value); }
        }

        public static readonly PropertyInfo<string> SwiftCodeProperty =
           RegisterProperty<string>(nameof(SwiftCode));
        public string SwiftCode
        {
            get { return GetProperty(SwiftCodeProperty); }
            private set { LoadProperty(SwiftCodeProperty, value); }
        }

        public static readonly PropertyInfo<BranchList> BranchesProperty =
            RegisterProperty<BranchList>(nameof(Branches));
        public BranchList Branches
        {
            get { return GetProperty(BranchesProperty); }
            private set { LoadProperty(BranchesProperty, value); }
        }

        [FetchChild]
        private void Fetch(BankDto data, [Inject] IChildDataPortal<BranchList> portal)
        {
            Id = data.Id;
            Name = data.Name;
            SwiftCode = data.SwiftCode;
            Branches = portal.FetchChild(Id);
        }
    }
}
