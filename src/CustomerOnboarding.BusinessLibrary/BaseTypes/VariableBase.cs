using Csla.Rules;
using Csla;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    [Serializable]
    public abstract class VariableBase<T> : BusinessBase<T>, IVariable
        where T : BusinessBase<T>
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
            set { SetProperty(TokenProperty, value); }
        }



        public static readonly PropertyInfo<byte[]> TimeStamProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStamProperty); }
            set { SetProperty(TimeStamProperty, value); }
        }
        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            // BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(TokenProperty, AmountProperty));
            // BusinessRules.AddRule(new PayrollGenius.BusinessLibrary.Rules.ExtractAmountFromToken(AmountProperty, TokenProperty));
        }



        [CreateChild]
        protected virtual void Create(int id, string token)
        {
            using (BypassPropertyChecks)
            {
                Id = id;
                Token = token;

            }
            BusinessRules.CheckRules();
        }

        [InsertChild]
        protected virtual void Insert(IEarning parent, [Inject] IVariableDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new VariableDto
                {
                    Id = Id,
                    ItemId = parent.Id,
                    Type = parent.GetType().Name.ToUpper(),
                    Token = Token,


                };
                dal.Insert(data);
                TimeStamp = data.LastChanged!;
            }
        }
        [FetchChild]
        protected virtual void Fetch(VariableDto data)
        {

            using (BypassPropertyChecks)
            {
                Id = data.Id;
                Token = data.Token;
                TimeStamp = data.LastChanged!;
            }
            BusinessRules.CheckRules();
        }

        [UpdateChild]
        protected virtual void Update(IEarning parent, [Inject] IVariableDal dal)
        {
            //Do Nothing
            
        }

        [DeleteSelfChild]
        protected virtual async Task DeleteSelf(IEarning parent, [Inject] IVariableDal dal)
        {
            await dal.DeleteAsync(parent.Id, this.Id);
        }
        [DeleteSelfChild]
        protected virtual async Task DeleteSelf(string id, [Inject] IVariableDal dal)
        {
            await dal.DeleteAsync(id, this.Id);
        }
    }
}
