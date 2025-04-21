using Antlr4.Runtime;
using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class PercentVariable : VariableBase<PercentVariable>
    {
        public static readonly PropertyInfo<decimal?> ValueProperty =
            RegisterProperty<decimal?>(nameof(Value));
        public decimal? Value
        {
            get => GetProperty(ValueProperty);
            set => SetProperty(ValueProperty, value);
        }

        [InsertChild]
        protected override void Insert(IEarning parent, [Inject] IVariableDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new VariableDto
                {
                    Id = Id,
                    ItemId = parent.Id,
                    Token = Token,
                    Type = parent.GetType().Name.ToUpper(),
                    Value = Value,

                };
                dal.Insert(data);
                TimeStamp = data.LastChanged!;
            }
        }

        [FetchChild]
        protected override void Fetch(VariableDto data)
        {
            using (BypassPropertyChecks)
            {
                Id = data.Id;
                Token = data.Token;
                Value = data.Value;
                TimeStamp = data.LastChanged!;
            }
            BusinessRules.CheckRules();
        }

        [UpdateChild]
        protected override void Update(IEarning parent, [Inject] IVariableDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new VariableDto
                {
                    Id = Id,
                    ItemId = parent.Id,
                    Value = Value,
                    LastChanged = TimeStamp
                };
                 dal.Update(data);
                TimeStamp = data.LastChanged;
            }
        }
    }
}
