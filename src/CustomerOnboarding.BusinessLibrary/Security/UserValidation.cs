using Csla;
using CustomerOnboarding.Dal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Security
{
    [Serializable]
    public class UserValidation :
        CommandBase<UserValidation>
    {
        public static readonly PropertyInfo<bool> IsValidProperty =
            RegisterProperty<bool>(nameof(IsValid));
        public bool IsValid
        {
            get=>ReadProperty(IsValidProperty);
            private set=>LoadProperty(IsValidProperty, value);
        }

        public static readonly PropertyInfo<string>TenantIdProperty=
            RegisterProperty<string>(nameof(TenantId));

        public string TenantId
        {
            get => ReadProperty(TenantIdProperty);
            private set=> LoadProperty(TenantIdProperty, value);
        }

        public static readonly PropertyInfo<string> EmailProperty
           = RegisterProperty<string>(nameof(Email));

        public string Email
        {
            get => ReadProperty(EmailProperty);
            private set => LoadProperty(EmailProperty, value);
        }

        public static readonly PropertyInfo<string> FirstNameProperty
          = RegisterProperty<string>(nameof(FirstName));
        public string FirstName
        {
            get { return ReadProperty(FirstNameProperty); }
            private set { LoadProperty(FirstNameProperty, value); }
        }

        public static readonly PropertyInfo<string> LastNameProperty
           = RegisterProperty<string>(nameof(LastName));
        public string LastName
        {
            get { return ReadProperty(LastNameProperty); }
            private set { LoadProperty(LastNameProperty, value); }
        }


        [Execute]
        private void Execute(string email, [Inject]IUserDal dal)
        {
            var user=dal.Fetch(email);
            TenantId = user.TenantId;
            Email=user.Email;
            FirstName = user.FirstName; 
            LastName = user.LastName;
        }

        [Execute]
        private void Execute(string email,string password, [Inject]IUserDal dal)
        {
            var user = dal.Fetch(email, password);
            IsValid = (user is not null);
            if (IsValid)
            {
                TenantId = user!.TenantId;
                Email = user!.Email;
                FirstName = user!.FirstName;
                LastName = user!.LastName;
            }
                
        }

    }
}
