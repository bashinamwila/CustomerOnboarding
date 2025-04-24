using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IEarningInfo : IReadOnlyBase
    {
        public string Id { get; }
        public string Name { get; }
        public int Type { get; }
        public string Formular { get; }
       // public VariableList Variables { get; }
    }
}
