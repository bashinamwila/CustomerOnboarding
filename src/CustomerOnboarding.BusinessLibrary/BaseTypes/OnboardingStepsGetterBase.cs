using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    [Serializable]
    public abstract class OnboardingStepsGetterBase<T> :
        ReadOnlyBase<T>
        where T : OnboardingStepsGetterBase<T>

    {
        public static readonly PropertyInfo<Steps>StepsProperty=
                        RegisterProperty<Steps>(nameof(Steps));
        public Steps Steps
        {
            get => GetProperty(StepsProperty);
            protected set => LoadProperty(StepsProperty, value);
        }
    }
}
