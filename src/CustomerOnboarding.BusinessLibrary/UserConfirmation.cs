using Csla;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class UserConfirmation :
        BusinessBase<UserConfirmation>
    {
        public static readonly PropertyInfo<string> FirstNameProperty
          = RegisterProperty<string>(nameof(FirstName));
        public string FirstName
        {
            get { return GetProperty(FirstNameProperty); }
            private set { LoadProperty(FirstNameProperty, value); }
        }

        public static readonly PropertyInfo<string> LastNameProperty
           = RegisterProperty<string>(nameof(LastName));
        public string LastName
        {
            get { return GetProperty(LastNameProperty); }
            private set { LoadProperty(LastNameProperty, value); }
        }

        public static readonly PropertyInfo<string> FullNameProperty
           = RegisterProperty<string>(nameof(FullName));
        public string FullName
        {
            get { return GetProperty(FullNameProperty); }
            private set { LoadProperty(FullNameProperty, value); }
        }

        public static readonly PropertyInfo<string> EmailProperty
            = RegisterProperty<string>(nameof(Email));

        public string Email
        {
            get => GetProperty(EmailProperty);
            private set => LoadProperty(EmailProperty, value);
        }

        public static readonly PropertyInfo<string> PhoneNumberProperty
           = RegisterProperty<string>(nameof(PhoneNumber));

        public string PhoneNumber
        {
            get => GetProperty(PhoneNumberProperty);
            private set => LoadProperty(PhoneNumberProperty, value);
        }

        public static readonly PropertyInfo<string> PasswordProperty
            = RegisterProperty<string>(nameof(Password));
        public string Password
        {
            get => GetProperty(PasswordProperty);
            private set => LoadProperty(PasswordProperty, value);
        }


        public static readonly PropertyInfo<bool> IsConfirmedProperty
            = RegisterProperty<bool>(nameof(IsConfirmed));

        public bool IsConfirmed
        {
            get => GetProperty(IsConfirmedProperty);
            private set => SetProperty(IsConfirmedProperty, value);
        }

        public void MarkAsConfirmed()
        {
            IsConfirmed = true;
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
            BusinessRules.RuleSet = "Confirm Account";
            BusinessRules.AddRule(new IsUserAccountConfirmed(IsConfirmedProperty));
        }

        [CreateChild]
        private void Create(string ruleSet)
        {
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(UserOnboardingOrchestrator parent,
            [Inject]IUserDal dal)
        {

        }

        [UpdateChild]
        private void Update(UserOnboardingOrchestrator parent,
            [Inject] IUserDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new UserDto
                {
                    TenantId = parent.TenantId,
                    Email = this.Email,
                    IsConfirmed = this.IsConfirmed,
                    LastChanged=this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

            }
        }

        [FetchChild]
        private void Fetch(string tenantId,string ruleSet, [Inject] IUserDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId);
                FirstName = data.FirstName;
                LastName = data.LastName;
                Email = data.Email;
                PhoneNumber = data.PhoneNumber;
                Password = data.Password;
                TimeStamp = data.LastChanged;
                IsConfirmed = data.IsConfirmed;
                BusinessRules.RuleSet = ruleSet;
            }
           BusinessRules.CheckRules();
        }

    }
}
