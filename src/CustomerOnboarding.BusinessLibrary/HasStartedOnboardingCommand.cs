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
    public class HasStartedOnboardingCommand :
        CommandBase<HasStartedOnboardingCommand>
    {
        public static readonly PropertyInfo<bool> HasStartedOnboardingProperty =
            RegisterProperty<bool>(nameof(HasStartedOnboarding));
        public bool HasStartedOnboarding
        {
            get => ReadProperty(HasStartedOnboardingProperty);
            private set => LoadProperty(HasStartedOnboardingProperty, value);
        }
        [Execute]
        private void Execute(string id,
            [Inject] ITenantOnboardingOrchestratorDal dal)
        {

            HasStartedOnboarding = dal.Exits(id);
        }
    }
}
