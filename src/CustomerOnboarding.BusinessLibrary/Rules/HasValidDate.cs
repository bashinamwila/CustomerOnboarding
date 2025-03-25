using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class HasValidDate : BusinessRule
    {
        public HasValidDate(Csla.Core.IPropertyInfo primaryProperty)
        : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var target = (DateSplitter)context.Target;
            if (target.DayPart > 0 && target.MonthPart > 0 && target.YearPart > 0)
            {
                if (IsNotValidDate(target.DayPart.Value, target.MonthPart.Value, target.YearPart.Value))
                {
                    context.AddOutValue(PrimaryProperty, false);
                }
                else
                {
                    context.AddOutValue(PrimaryProperty, true);
                }

            }
            else
            {
                context.AddOutValue(PrimaryProperty, false);
            }
        }

        private bool IsNotValidDate(int day, int month, int year)
        {
            bool isNotValid = default;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (day > 31)
                        isNotValid = true;
                    break;
                case 2:
                    if (DateTime.IsLeapYear(year))
                    {
                        if (day > 29)
                            isNotValid = true;
                    }
                    else
                    {
                        if (day > 28)
                            isNotValid = true;
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (day > 30) isNotValid = true;
                    break;
                default:
                    isNotValid = false;
                    break;
            }
            return isNotValid;
        }


    }
}
