using Csla.Rules.CommonRules;
using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.Rules;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class DateSplitter : BusinessBase<DateSplitter>
    {
        public static readonly PropertyInfo<int?> DayPartProperty = RegisterProperty<int?>(nameof(DayPart));
        public int? DayPart
        {
            get => GetProperty(DayPartProperty);
            set => SetProperty(DayPartProperty, value);
        }

        public static readonly PropertyInfo<int?> MonthPartProperty = RegisterProperty<int?>(nameof(MonthPart));
        public int? MonthPart
        {
            get => GetProperty(MonthPartProperty);
            set => SetProperty(MonthPartProperty, value);
        }

        public static readonly PropertyInfo<int?> YearPartProperty = RegisterProperty<int?>(nameof(YearPart));
        public int? YearPart
        {
            get => GetProperty(YearPartProperty);
            set => SetProperty(YearPartProperty, value);
        }

        public static readonly PropertyInfo<bool> HasValidDateValueProperty =
            RegisterProperty<bool>(nameof(HasValidDateValue));
        public bool HasValidDateValue
        {
            get => GetProperty(HasValidDateValueProperty);
            private set => LoadProperty(HasValidDateValueProperty, value);
        }

        public static readonly PropertyInfo<DateTime?> DateProperty = RegisterProperty<DateTime?>(nameof(Date));
        [Required]
        public DateTime? Date
        {
            get => GetProperty(DateProperty);
            private set => LoadProperty(DateProperty, value);
        }

        private class SplitDate : Csla.Rules.BusinessRule
        {
            public SplitDate(Csla.Core.IPropertyInfo primaryProperty,
                   Csla.Core.IPropertyInfo affectedProperty)
                : base(primaryProperty)
            {
                InputProperties.Add(PrimaryProperty);
                AffectedProperties.Add(affectedProperty);
            }
            protected override void Execute(IRuleContext context)
            {
                var target = (DateSplitter)context.Target;
                if (target.DayPart.HasValue &&
                    target.MonthPart.HasValue
                    && target.YearPart.HasValue)
                {
                    var date = new DateTime(target.YearPart.Value, target.MonthPart.Value, target.DayPart.Value);
                    context.AddOutValue(AffectedProperties[1], date);
                }
            }
        }



        private class CheckDateValidity : BusinessRule
        {
            public CheckDateValidity(Csla.Core.IPropertyInfo primaryProperty,
                Csla.Core.IPropertyInfo dependentProperty1,
                Csla.Core.IPropertyInfo dependentProperty2,
                Csla.Core.IPropertyInfo dependantProperty3)
                : base(primaryProperty)
            {
                InputProperties.AddRange(new[] { primaryProperty, dependentProperty1, dependentProperty2, dependantProperty3 });
            }
            protected override void Execute(IRuleContext context)
            {
                var target = (DateSplitter)context.Target;
                var day = target.DayPart;
                var month = target.MonthPart;
                var year = target.YearPart;
                if (day != 0 && month != 0)
                {
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
                                context.AddErrorResult("Invalid day of the month");
                            break;
                        case 4:
                        case 6:
                        case 9:
                        case 11:
                            if (day > 30)
                                context.AddErrorResult("Invalid day of the month");
                            break;
                    }

                }
                if (day == 0 && month == 0 && year == 0)
                {
                    context.AddErrorResult($"{PrimaryProperty.Name} is required");
                }
                if (day != 0 && month != 0 && year != 0)
                {
                    if (month == 2)
                    {
                        if (!DateTime.IsLeapYear(year!.Value) && day == 29)
                            context.AddErrorResult($"{year} is not a leap year");
                        else
                            if (day > 28)
                        {
                            context.AddErrorResult("Invalid day of the month");
                        }
                    }
                }
            }
        }

        protected override void AddBusinessRules()
        {
            BusinessRules.ProcessThroughPriority = 1;
            base.AddBusinessRules();
            BusinessRules.AddRule(new CheckDateValidity(DateProperty, DayPartProperty, MonthPartProperty, YearPartProperty) { Priority = 1 });
            BusinessRules.AddRule(new SplitDate(DayPartProperty, DateProperty) { Priority = 2 });
            BusinessRules.AddRule(new SplitDate(MonthPartProperty, DateProperty) { Priority = 2 });
            BusinessRules.AddRule(new SplitDate(YearPartProperty, DateProperty) { Priority = 2 });
            BusinessRules.AddRule(new HasValidDate(HasValidDateValueProperty));
            BusinessRules.AddRule(new Dependency(DayPartProperty, HasValidDateValueProperty));
            BusinessRules.AddRule(new Dependency(MonthPartProperty, HasValidDateValueProperty));
            BusinessRules.AddRule(new Dependency(YearPartProperty, HasValidDateValueProperty));

        }

        [CreateChild]
        private void Create() { }
        [CreateChild]
        private void Create(int day, int month, int year)
        {
            using (BypassPropertyChecks)
            {
                this.DayPart = day;
                this.MonthPart = month;
                this.YearPart = year;

            }
            BusinessRules.CheckRules(DateProperty);
            BusinessRules.CheckRules(HasValidDateValueProperty);
        }



        [FetchChild]
        private void Fetch(DateTime date)
        {
            using (BypassPropertyChecks)
            {
                this.DayPart = date.Day;
                this.MonthPart = date.Month;
                this.YearPart = date.Year;
                this.Date = date;
            }
            BusinessRules.CheckRules(HasValidDateValueProperty);
        }

        [FetchChild]
        private void Fetch(DateTime? date)
        {
            using (BypassPropertyChecks)
            {
                if (date.HasValue)
                {
                    this.DayPart = date.Value.Day;
                    this.MonthPart = date.Value.Month;
                    this.YearPart = date.Value.Year;
                    this.Date = date;
                }

            }
            BusinessRules.CheckRules(HasValidDateValueProperty);
        }

    }
}
