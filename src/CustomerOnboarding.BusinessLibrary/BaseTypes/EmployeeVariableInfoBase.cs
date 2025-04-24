using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    [Serializable]
    public abstract class EmployeeVariableInfoBase<T> : ReadOnlyBase<T>, IEmployeeVariableInfo
        where T : ReadOnlyBase<T>
    {
        public static readonly PropertyInfo<int> IdProperty =
           RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            protected set { LoadProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<string> TokenProperty =
            RegisterProperty<string>(nameof(Token));
        public string Token
        {
            get { return GetProperty(TokenProperty); }
            protected set { LoadProperty(TokenProperty, value); }
        }

        public static readonly PropertyInfo<decimal?> ValueProperty = RegisterProperty<decimal?>(nameof(Value));

        public decimal? Value
        {
            get { return GetProperty(ValueProperty); }
            protected set { LoadProperty(ValueProperty, value); }
        }
        public static readonly PropertyInfo<string> ParentTypeProperty =
           RegisterProperty<string>(nameof(ParentType));
        public string ParentType
        {
            get { return GetProperty(ParentTypeProperty); }
            protected set { LoadProperty(ParentTypeProperty, value); }
        }

        [FetchChild]
        protected virtual void Fetch(string employeeId, string wageId, int id,
            [Inject] IEmployeeVariableDal dal)
        {

            var data = dal.Fetch(employeeId, wageId, id);
            Id = data.Id;
            Token = data.Token;
            Value = data.Value;

        }
    }
}
