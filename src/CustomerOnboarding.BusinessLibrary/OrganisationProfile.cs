using Csla;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
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
    public class OrganisationProfile :
        BusinessBase<OrganisationProfile>
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

        public static readonly PropertyInfo<string> PhoneNumberProperty =
          RegisterProperty<string>(nameof(PhoneNumber));
        public string PhoneNumber
        {
            get => GetProperty(PhoneNumberProperty);
            set => SetProperty(PhoneNumberProperty, value);
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

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(NameProperty) { MessageText = "Organisation Name is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(EmailProperty) { MessageText = "Email Address is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(AddressLine1Property) { MessageText = "Address Line 1 is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(PhoneNumberProperty) { MessageText = "Phone Number is required" });
            BusinessRules.AddRule(new ValidateEmailAddress(EmailProperty));
        }


        [CreateChild]
        private async Task CreateAsync(string id, [Inject]IOrganisationDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = dal.Fetch(id);
                Id = dto.Id;
                Name = dto.Name;
            }
            await BusinessRules.CheckRulesAsync();
        }

    }
}
