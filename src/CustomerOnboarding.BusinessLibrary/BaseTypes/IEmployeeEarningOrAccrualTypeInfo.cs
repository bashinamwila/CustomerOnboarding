using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    public interface IEmployeeEarningOrAccrualTypeInfo : IReadOnlyBase
    {
        public int Id { get; }
        public string Formular { get; }
        public EmployeeVariableList Variables { get; }
        // internal void CheckBusinessRules();
    }
}
