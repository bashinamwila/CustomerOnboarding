using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class Required : BusinessRule
    {
        public string MessageText { get; set; } = string.Empty;
        public Required(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }

        protected override void Execute(IRuleContext context)
        {

            if (PrimaryProperty.Type == typeof(int?))
            {
                var input = context.GetInputValue<int?>(PrimaryProperty);
                if (!input.HasValue)
                {
                    context.AddErrorResult(MessageText);
                }
            }
            else
            {
                if (PrimaryProperty.Type == typeof(string))
                {
                    var input = context.GetInputValue<string>(PrimaryProperty);
                    if (string.IsNullOrEmpty(input))
                    {
                        context.AddErrorResult(MessageText);
                    }
                }
                else
                {
                    if (PrimaryProperty.Type == typeof(decimal?))
                    {
                        var input = context.GetInputValue<decimal?>(PrimaryProperty);
                        if (!input.HasValue)
                        {
                            context.AddErrorResult(MessageText);
                        }
                    }
                }
            }

        }

    }
}
