using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IStep :IBusinessBase
    {
        public string Name { get; }
        public StepType Type { get; }
        public int StepIndex { get; }

        public Task ExecuteAsync();
        public bool IsCompleted { get; }
    }
}
