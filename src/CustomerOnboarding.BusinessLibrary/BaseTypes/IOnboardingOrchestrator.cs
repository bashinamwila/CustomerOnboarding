using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IOnboardingOrchestrator :IBusinessBase
    {
        public bool IsComplete { get; }
        public Steps Steps { get; }
        public int CurrentStepIndex { get; }
        public Task MoveNextAsync();
    }
}
