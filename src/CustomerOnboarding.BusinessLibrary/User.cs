﻿using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
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
    public class User : BusinessBase<User>, IFullName
    {
        public static readonly PropertyInfo<string> FirstNameProperty
           = RegisterProperty<string>(nameof(FirstName));
        public string FirstName
        {
            get { return GetProperty(FirstNameProperty); }
            set { SetProperty(FirstNameProperty, value); }
        }

        public static readonly PropertyInfo<string> LastNameProperty
           = RegisterProperty<string>(nameof(LastName));
        public string LastName
        {
            get { return GetProperty(LastNameProperty); }
            set { SetProperty(LastNameProperty, value); }
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
            set => SetProperty(EmailProperty, value);
        }

        public static readonly PropertyInfo<string> PhoneNumberProperty
           = RegisterProperty<string>(nameof(PhoneNumber));

        public string PhoneNumber
        {
            get => GetProperty(PhoneNumberProperty);
            set => SetProperty(PhoneNumberProperty, value);
        }

        public static readonly PropertyInfo<string> PasswordProperty
            = RegisterProperty<string>(nameof(Password));
        public string Password
        {
            get => GetProperty(PasswordProperty);
            set => SetProperty(PasswordProperty, value);
        }

        public static readonly PropertyInfo<string> ConfirmPasswordProperty
            = RegisterProperty<string>(nameof(ConfirmPassword));
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword
        {
            get => GetProperty(ConfirmPasswordProperty);
            set => SetProperty(ConfirmPasswordProperty, value);
        }

        
        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }



        override protected void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.RuleSet= "Create Account";
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(EmailProperty) { MessageText = "Email is Required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(PasswordProperty) { MessageText = "Password is required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(ConfirmPasswordProperty) { MessageText = "Password confirmation required" });
            BusinessRules.AddRule(new ContainsAtLeastOneNumericCharacter(PasswordProperty));
            BusinessRules.AddRule(new ValidateEmailAddress(EmailProperty));
            BusinessRules.AddRule(new ContainsAtLeastOneUpperCaseCharacter(PasswordProperty));
            BusinessRules.AddRule(new ContainsAtLeastOneLowerCaseCharacter(PasswordProperty));
            BusinessRules.AddRule(new ContainsAtLeastOneNonAlphaNumericCharacter(PasswordProperty));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.MinLength(PasswordProperty, 8));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(PasswordProperty, ConfirmPasswordProperty));
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(FirstNameProperty) { MessageText = "First Name is required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(PhoneNumberProperty) { MessageText = "Phone No. is required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(LastNameProperty) { MessageText = "Last Name is required" });
            BusinessRules.AddRule(new FullNameRule(FirstNameProperty, FullNameProperty));
            BusinessRules.AddRule(new FullNameRule(LastNameProperty, FullNameProperty));
            BusinessRules.AddRule(new CheckIfPasswordsMatch(PasswordProperty, ConfirmPasswordProperty));

            
           
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
            var data = new UserDto
            {
                TenantId=parent.TenantId,
                FirstName=this.FirstName,
                LastName=this.LastName,
                Email=this.Email,
                PhoneNumber=this.PhoneNumber,
                Password=this.Password,
               
            };
            dal.Insert(data);
            TimeStamp = data.LastChanged;
        }

        [UpdateChild]
        private void Update(UserOnboardingOrchestrator parent,
            [Inject] IUserDal dal)
        {
            var data = new UserDto
            {
                TenantId = parent.TenantId,
                Email = this.Email,
                LastChanged=this.TimeStamp
            };
            dal.Update(data);
            TimeStamp = data.LastChanged;
        }

        [FetchChild]
        private void Fetch(string tenantId,string ruleSet,
            [Inject]IUserDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId);
                FirstName = data.FirstName;
                LastName = data.LastName;
                PhoneNumber = data.PhoneNumber;
                Email = data.Email;
                Password = data.Password;
                TimeStamp = data.LastChanged;
                BusinessRules.RuleSet = ruleSet;

            }
            BusinessRules.CheckRules();
        }
    }
}
