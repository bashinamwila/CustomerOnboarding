using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CalculateContractExpiryDate : BusinessRule
    {

#pragma warning disable CSLA0017
        public CalculateContractExpiryDate(Csla.Core.IPropertyInfo primaryProperty, Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {

            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }
        protected override void Execute(IRuleContext context)
        {

            var target = (ITerm)context.Target;
            var startDate = target.ContractStartDate;
            var interval = target.Interval;
            var duration = target.ContractDuration;



            DateTime expiryDate = DateTime.Now;

            if ((interval.HasValue && interval.Value != 0) && 
                (duration.HasValue && duration.Value != 0) && (startDate.DayPart != 0
                && startDate.MonthPart != 0 && startDate.YearPart != 0
                && startDate.Date.HasValue))
            {
                if (interval.Value == 1)
                {
                    expiryDate = startDate.Date!.Value.AddDays((duration.Value * 7)).AddDays(-1);
                }
                else
                {
                    if (interval.Value == 2)
                    {
                        expiryDate = startDate.Date!.Value.AddMonths(duration.Value).AddDays(-1);
                    }
                    else
                    {
                        if (interval.Value == 3)
                        {
                            expiryDate = startDate!.Date!.Value.AddYears(duration.Value).AddDays(-1);
                        }
                    }
                }
                LoadProperty(context.Target, AffectedProperties[1], expiryDate);
#pragma warning restore CSLA0017

            }

        }
    }
}
