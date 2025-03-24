using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName
        {
            get => GetProperty(FirstNameProperty);
            set => SetProperty(FirstNameProperty, value);
        }

        public static readonly PropertyInfo<string> LastNameProperty =
            RegisterProperty<string>(nameof(LastName));
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName
        {
            get => GetProperty(LastNameProperty);
            set => SetProperty(LastNameProperty, value);
        }

        public static readonly PropertyInfo<string> OrganisationNameProperty =
            RegisterProperty<string>(nameof(OrganisationName));
        [Required(ErrorMessage = "Organisation Name is required")]
        public string OrganisationName
        {
            get => GetProperty(OrganisationNameProperty);
            set => SetProperty(OrganisationNameProperty, value);
        }


        public static readonly PropertyInfo<string> WorkEmailProperty =
            RegisterProperty<string>(nameof(WorkEmail));
       [Required(ErrorMessage = "Work Email is required")]
        public string WorkEmail
        {
            get => GetProperty(WorkEmailProperty);
            set => SetProperty(WorkEmailProperty, value);
        }

        public static readonly PropertyInfo<string> OrganisationPhoneProperty =
            RegisterProperty<string>(nameof(OrganisationPhone));
        [Required(ErrorMessage = "Organisation Phone is required")]
        public string OrganisationPhone
        {
            get => GetProperty(OrganisationPhoneProperty);
            set => SetProperty(OrganisationPhoneProperty, value);
        }

        public static readonly PropertyInfo<string> PasswordProperty =
            RegisterProperty<string>(nameof(Password));
        [Required(ErrorMessage = "Password is required")]
        public string Password
        {
            get => GetProperty(PasswordProperty);
            set => SetProperty(PasswordProperty, value);
        }

        public static readonly PropertyInfo<string> Password2Property =
            RegisterProperty<string>(nameof(Password2));
        [Required(ErrorMessage = "Password is required")]
        public string Password2
        {
            get => GetProperty(Password2Property);
            set => SetProperty(Password2Property, value);
        }

        public static readonly PropertyInfo<int?> NumberOfEmployeesProperty =
            RegisterProperty<int?>(nameof(NumberOfEmployeesProperty));
        //[Required(ErrorMessage = "Number of Employees is required")]
        public int? NumberOfEmployees
        {
            get => GetProperty(NumberOfEmployeesProperty);
            set => SetProperty(NumberOfEmployeesProperty, value);
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

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new CheckIfStepIsComplete(FirstNameProperty,IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(LastNameProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(OrganisationNameProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(WorkEmailProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(OrganisationPhoneProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(PasswordProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(Password2Property, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(WorkEmailProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(NumberOfEmployeesProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfPasswordsMatch(PasswordProperty, Password2Property));

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
            BusinessRules.CheckRules();
        }

        [FetchChild]
        private void Fetch(string tenantId, int id, [Inject] ICreateAccountStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                FirstName = data.FirstName;
                LastName = data.LastName;
                OrganisationName = data.OrganisationName;
                WorkEmail = data.WorkEmail;
                OrganisationPhone = data.OrganisationPhone;
                Password = data.Password;
                NumberOfEmployees = data.NumberOfEmployees;
                StepIndex=data.StepIndex;
                Name = data.Name;
                Type=(StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                TimeStamp = data.LastChanged;
            }
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(CustomerOnboardingOrchestrator parent,[Inject]ICreateAccountStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CreateAccountStepDto
                {
                    TenantId=parent.TenantId,
                    StepId=this.Id,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    OrganisationName = this.OrganisationName,
                    WorkEmail = this.WorkEmail,
                    OrganisationPhone = this.OrganisationPhone,
                    Password = this.Password,
                    NumberOfEmployees=this.NumberOfEmployees!.Value
                   
                };
                dal.Insert(data);
                TimeStamp = data.LastChanged;
            }
        }

        [UpdateChild]
        private void Update(CustomerOnboardingOrchestrator parent, [Inject] ICreateAccountStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = new CreateAccountStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    OrganisationName = this.OrganisationName,
                    WorkEmail = this.WorkEmail,
                    OrganisationPhone = this.OrganisationPhone,
                    Password = this.Password,
                    NumberOfEmployees=this.NumberOfEmployees!.Value,
                    LastChanged =this.TimeStamp
                };
                 dal.Update(data);
                TimeStamp = data.LastChanged;
            }
        }
    }
}
