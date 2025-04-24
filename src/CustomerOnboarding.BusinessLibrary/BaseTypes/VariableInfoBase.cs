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
    public abstract class VariableInfoBase<T> : ReadOnlyBase<T>, IVariableInfo
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




        [FetchChild]
        protected virtual void Fetch(string tenantId, int id, string itemId,
             [Inject] IVariableDal dal)
        {

            var data = dal.Fetch(tenantId,id, itemId);
            Id = data.Id;
            Token = data.Token;



        }
    }
}
