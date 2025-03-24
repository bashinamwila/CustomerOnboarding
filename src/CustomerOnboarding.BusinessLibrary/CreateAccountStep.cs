using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class CreateAccountStep :
        StepBase<CreateAccountStep>
    {

        public static readonly PropertyInfo<string> FirstNameProperty =
            RegisterProperty<string>(nameof(FirstName));
        public string FirstName
        {
            get => GetProperty(FirstNameProperty);
            set => SetProperty(FirstNameProperty, value);
        }

        public static readonly PropertyInfo<string> LastNameProperty =
            RegisterProperty<string>(nameof(LastName));
        public string LastName
        {
            get => GetProperty(LastNameProperty);
            set => SetProperty(LastNameProperty, value);
        }

        public static readonly PropertyInfo<string> OrganisationNameProperty =
            RegisterProperty<string>(nameof(OrganisationName));
        public string OrganisationName
        {
            get => GetProperty(OrganisationNameProperty);
            set => SetProperty(OrganisationNameProperty, value);
        }


        public static readonly PropertyInfo<string> WorkEmailProperty =
            RegisterProperty<string>(nameof(WorkEmail));
        public string WorkEmail
        {
            get => GetProperty(WorkEmailProperty);
            set => SetProperty(WorkEmailProperty, value);
        }

        public static readonly PropertyInfo<string> OrganisationPhoneProperty =
            RegisterProperty<string>(nameof(OrganisationPhone));
        public string OrganisationPhone
        {
            get => GetProperty(OrganisationPhoneProperty);
            set => SetProperty(OrganisationPhoneProperty, value);
        }

        public static readonly PropertyInfo<string> PasswordProperty =
            RegisterProperty<string>(nameof(Password));
        public string Password
        {
            get => GetProperty(PasswordProperty);
            set => SetProperty(PasswordProperty, value);
        }

        public static readonly PropertyInfo<string> Password2Property =
            RegisterProperty<string>(nameof(Password2));
        public string Password2
        {
            get => GetProperty(Password2Property);
            set => SetProperty(Password2Property, value);
        }


        public static readonly PropertyInfo<byte[]> TimeStampProperty =
           RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }



        [CreateChild]
        private void Create(int id, [Inject] IStepTypeDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                StepIndex = 0;
                IsCompleted = false;

            }
        }

        [InsertChild]
        private async Task InsertAsync(string tenantId,[Inject]ICreateAccountStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CreateAccountStepDto
                {
                    TenantId=tenantId,
                    StepIndex=this.StepIndex,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    OrganisationName = this.OrganisationName,
                    WorkEmail = this.WorkEmail,
                    OrganisationPhone = this.OrganisationPhone,
                    Password = this.Password,
                    Name=this.Name,
                    Type=(int)this.Type,
                    IsCompleted=true
                };
                await dal.InsertAsync(data);
                TimeStamp = data.LastChanged;
            }
        }

        [InsertChild]
        private async Task UpdateAsync(string tenantId, [Inject] ICreateAccountStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CreateAccountStepDto
                {
                    TenantId = tenantId,
                    StepIndex = this.StepIndex,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    OrganisationName = this.OrganisationName,
                    WorkEmail = this.WorkEmail,
                    OrganisationPhone = this.OrganisationPhone,
                    Password = this.Password,
                    LastChanged=this.TimeStamp
                };
                await dal.UpdateAsync(data);
                TimeStamp = data.LastChanged;
            }
        }
    }
}
