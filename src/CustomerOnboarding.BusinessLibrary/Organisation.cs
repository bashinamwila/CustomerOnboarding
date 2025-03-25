using Csla;
using Csla.Core;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class Organisation : BusinessBase<Organisation>
    {

        public static readonly PropertyInfo<string> IdProperty =
           RegisterProperty<string>(nameof(Id));
        public string Id
        {
            get => GetProperty(IdProperty);
            private set => LoadProperty(IdProperty, value);
        }
        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(nameof(Name));
        [Display(Name = "Organisation Name")]
        public string Name
        {
            get => GetProperty(NameProperty);
            set => SetProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<int?> CountryProperty =
           RegisterProperty<int?>(nameof(Country));
        public int? Country
        {
            get => GetProperty(CountryProperty);
            set => SetProperty(CountryProperty, value);
        }

        public static readonly PropertyInfo<byte[]?> LogoProperty =
           RegisterProperty<byte[]?>(nameof(Logo));
        public byte[]? Logo
        {
            get => GetProperty(LogoProperty);
            set => SetProperty(LogoProperty, value);
        }

        public static readonly PropertyInfo<string> AddressLine1Property =
           RegisterProperty<string>(nameof(AddressLine1));
        [Display(Name = "Address Line 1")]
        public string AddressLine1
        {
            get => GetProperty(AddressLine1Property);
            set => SetProperty(AddressLine1Property, value);
        }

        public static readonly PropertyInfo<string> AddressLine2Property =
          RegisterProperty<string>(nameof(AddressLine2));
        [Display(Name = "Address Line 2 (Optional)")]
        public string AddressLine2
        {
            get => GetProperty(AddressLine2Property);
            set => SetProperty(AddressLine2Property, value);
        }

        public static readonly PropertyInfo<string?> CityProperty =
           RegisterProperty<string?>(nameof(City));
        public string? City
        {
            get => GetProperty(CityProperty);
            set => SetProperty(CityProperty, value);
        }

        public static readonly PropertyInfo<string> DbConnectionStringProperty =
           RegisterProperty<string>(nameof(DbConnectionString));
        public string DbConnectionString
        {
            get => GetProperty(DbConnectionStringProperty);
            private set => LoadProperty(DbConnectionStringProperty, value);
        }

        public static readonly PropertyInfo<string> PhoneNoProperty =
          RegisterProperty<string>(nameof(PhoneNo));
        public string PhoneNo
        {
            get => GetProperty(PhoneNoProperty);
            set => SetProperty(PhoneNoProperty, value);
        }

        public static readonly PropertyInfo<string> EmailProperty =
           RegisterProperty<string>(nameof(Email));
        [EmailAddress]
        public string Email
        {
            get => GetProperty(EmailProperty);
            set => SetProperty(EmailProperty, value);
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }

        public static readonly PropertyInfo<string> BankIdProperty =
           RegisterProperty<string>(nameof(BankId));
        [Display(Name = "Bank Name")]
        public string BankId
        {
            get { return GetProperty(BankIdProperty); }
            set { SetProperty(BankIdProperty, value); }
        }
        public static readonly PropertyInfo<int> BranchIdProperty =
            RegisterProperty<int>(nameof(BranchId));
        [Display(Name = "Branch Name")]
        public int BranchId
        {
            get { return GetProperty(BranchIdProperty); }
            set { SetProperty(BranchIdProperty, value); }
        }


        public static readonly PropertyInfo<string> BranchCodeProperty =
            RegisterProperty<string>(nameof(BranchCode));
        public string BranchCode
        {
            get { return GetProperty(BranchCodeProperty); }
            private set { LoadProperty(BranchCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> BankAccountNumberProperty =
           RegisterProperty<string>(nameof(BankAccountNumber));
        [Display(Name = "Account #")]
        public string BankAccountNumber
        {
            get { return GetProperty(BankAccountNumberProperty); }
            set { SetProperty(BankAccountNumberProperty, value); }
        }

        public static readonly PropertyInfo<int> NumberOfEmployeesProperty =
            RegisterProperty<int>(nameof(NumberOfEmployees));

        public int NumberOfEmployees
        {
            get => GetProperty(NumberOfEmployeesProperty);
            set => SetProperty(NumberOfEmployeesProperty, value);
        }

        public static readonly PropertyInfo<string> TPINProperty = RegisterProperty<string>(nameof(TPIN));
        public string TPIN
        {
            get => GetProperty(TPINProperty);
            set => SetProperty(TPINProperty, value);
        }

        // NAPSAAccountNumber Property
        public static readonly PropertyInfo<string> NAPSAAccountNumberProperty = RegisterProperty<string>(nameof(NAPSAAccountNumber));
        [Display(Name = "NAPSA A/C #")]
        public string NAPSAAccountNumber
        {
            get => GetProperty(NAPSAAccountNumberProperty);
            set => SetProperty(NAPSAAccountNumberProperty, value);
        }

        // NHIMAAccountNumber Property
        public static readonly PropertyInfo<string> NHIMAAccountNumberProperty = RegisterProperty<string>(nameof(NHIMAAccountNumber));
        [Display(Name = "NHIMA A/C #")]
        public string NHIMAAccountNumber
        {
            get => GetProperty(NHIMAAccountNumberProperty);
            set => SetProperty(NHIMAAccountNumberProperty, value);
        }

        public static readonly PropertyInfo<DateSplitter> FirstPayDayProperty =
            RegisterProperty<DateSplitter>(nameof(FirstPayDay));
        [Display(Name = "What is your anticipated first Benefito payday")]
        public DateSplitter FirstPayDay
        {
            get => GetProperty(FirstPayDayProperty);
            set => SetProperty(FirstPayDayProperty, value);
        }

        public static readonly PropertyInfo<int?> CurrencyProperty =
            RegisterProperty<int?>(nameof(Currency));
        public int? Currency
        {
            get => GetProperty(CurrencyProperty);
            set => SetProperty(CurrencyProperty, value);
        }

        public static readonly PropertyInfo<int?> PayFrequencyProperty =
            RegisterProperty<int?>(nameof(PayFrequency));
        [Display(Name = "How often do you pay your employees ?")]
        public int? PayFrequency
        {
            get => GetProperty(PayFrequencyProperty);
            set => SetProperty(PayFrequencyProperty, value);
        }

        public static readonly PropertyInfo<decimal?> RegularHoursProperty =
            RegisterProperty<decimal?>(nameof(RegularHours));
        public decimal? RegularHours
        {
            get => GetProperty(RegularHoursProperty);
            set => SetProperty(RegularHoursProperty, value);
        }


        public static readonly PropertyInfo<bool> AllowNegativePayProperty =
           RegisterProperty<bool>(nameof(AllowNegativePay));
        public bool AllowNegativePay
        {
            get => GetProperty(AllowNegativePayProperty);
            set => SetProperty(AllowNegativePayProperty, value);
        }

        public static readonly PropertyInfo<int?> NumberOfWorkingDaysPerPayPeriodProperty =
            RegisterProperty<int?>(nameof(NumberOfWorkingDaysPerPayPeriod));
        public int? NumberOfWorkingDaysPerPayPeriod
        {
            get => GetProperty(NumberOfWorkingDaysPerPayPeriodProperty);
            set => SetProperty(NumberOfWorkingDaysPerPayPeriodProperty, value);
        }



        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.RuleSet = "Create Account";
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(NameProperty) { MessageText = "Organisation Name is required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(CountryProperty) { MessageText = "Country is required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(NumberOfEmployeesProperty) { MessageText = "Number of Employee is required" });

        }


        [CreateChild]
        private void Create(string tenantId,string ruleSet)
        {
            using(BypassPropertyChecks)
            {
                Id = tenantId;
                BusinessRules.RuleSet = ruleSet;
            }
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(CustomerOnboardingOrchestrator parent,
            [Inject]IOrganisationDal dal)
        {
            var data = new OrganisationDto
            {
                Id = parent.TenantId,
                Name = this.Name,
                Country = this.Country!.Value
            };
            dal.Insert(data);
            TimeStamp = data.LastChanged;
        }

        [FetchChild]
        private void Fetch(string tenantId,string ruleSet, [Inject]IOrganisationDal dal,
            [Inject]IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId);
                this.Id = data.Id;
                this.Name = data.Name;
                this.Logo = data.Logo;
                this.Country = data.Country;
                this.AddressLine1 = data.AddressLine1;
                this.AddressLine2 = data.AddressLine2;
                this.PhoneNo = data.PhoneNo;
                this.Email = data.Email;
                this.TPIN = data.TPIN;
                this.NAPSAAccountNumber = data.NAPSAAccountNumber;
                this.NHIMAAccountNumber = data.NHIMAAccountNumber;
                this.BankAccountNumber = data.BankAccountNumber;
                this.DbConnectionString = data.DbConnectionString;
                this.BankId = data.BankId;
                this.BranchId = data.BranchId;
                this.BranchCode = data.BranchCode;
                FirstPayDay = data.FirstPayday.HasValue ? portal.GetPortal<DateSplitter>().FetchChild(data.FirstPayday.Value) :
                    portal.GetPortal<DateSplitter>().CreateChild();
                Currency = data.Currency;
                NumberOfWorkingDaysPerPayPeriod = data.NumberOfWorkingDaysPerPayPeriod;
                PayFrequency = data.PayFrequency;
                RegularHours = data.RegularHours;
                AllowNegativePay = data.AllowNegativePay;
                this.DbConnectionString = data.DbConnectionString;
                this.TimeStamp = data.LastChanged!;

                BusinessRules.RuleSet = ruleSet;

            }
        }
    }
}
