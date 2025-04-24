using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class DateRequired : BusinessRule
    {
        public DateRequired(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }
        protected override void Execute(IRuleContext context)
        {
            var dateSplitter = context.GetInputValue<DateSplitter>(PrimaryProperty);
            if (dateSplitter.DayPart == 0 || dateSplitter.MonthPart == 0 || dateSplitter.YearPart == 0)
                context.AddErrorResult("Date is required");
        }
    }
}
