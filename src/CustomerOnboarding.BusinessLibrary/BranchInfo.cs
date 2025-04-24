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
    public class BranchInfo : ReadOnlyBase<BranchInfo>
    {
        public static readonly PropertyInfo<int> IdProperty =
            RegisterProperty<int>(nameof(Id));

        public int Id
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

        public static readonly PropertyInfo<string> BranchCodeProperty =
            RegisterProperty<string>(nameof(BranchCode));
        public string BranchCode
        {
            get { return GetProperty(BranchCodeProperty); }
            private set { LoadProperty(BranchCodeProperty, value); }
        }

        [FetchChild]
        private void Fetch(BranchDto data)
        {
            this.Id = data.Id;
            this.Name = data.Name;
            this.BranchCode = data.BranchCode;

        }

        /*
        [FetchChild]
        private void Fetch(BranchModel data)
        {
            this.Id = data.Id;
            this.Name = data.Name;
            this.BranchCode = data.BranchCode;
        }
        */
    }
}
