using Csla;
using CustomerOnboarding.BusinessLibrary.Attributes;
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
    public class EmployeePersonalDetails :
        BusinessBase<EmployeePersonalDetails>
    {
        
        public static readonly PropertyInfo<int?> GenderProperty =
            RegisterProperty<int?>(nameof(Gender));
        public int? Gender
        {
            get { return GetProperty(GenderProperty); }
            set { SetProperty(GenderProperty, value); }
        }

        public static readonly PropertyInfo<int?> MaritalStatusProperty =
            RegisterProperty<int?>(nameof(MaritalStatus));
        [Display(Name = "Marital Status")]
        public int? MaritalStatus
        {
            get { return GetProperty(MaritalStatusProperty); }
            set { SetProperty(MaritalStatusProperty, value); }
        }

        public static readonly PropertyInfo<string> EmailAddressProperty =
           RegisterProperty<string>(nameof(EmailAddress));
        

        // [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string EmailAddress
        {
            get { return GetProperty(EmailAddressProperty); }
            set { SetProperty(EmailAddressProperty, value); }
        }

        public static readonly PropertyInfo<string> AddressLine1Property =
            RegisterProperty<string>(nameof(AddressLine1));
       

        public string AddressLine1
        {
            get { return GetProperty(AddressLine1Property); }
            set { SetProperty(AddressLine1Property, value); }
        }

        public static readonly PropertyInfo<string> AddressLine2Property =
            RegisterProperty<string>(nameof(AddressLine2));
       
        public string AddressLine2
        {
            get { return GetProperty(AddressLine2Property); }
            set { SetProperty(AddressLine2Property, value); }
        }

        public static readonly PropertyInfo<string> SSNProperty =
            RegisterProperty<string>(nameof(SSN));
        

        public string SSN
        {
            get { return GetProperty(SSNProperty); }
            set { SetProperty(SSNProperty, value); }
        }

        public static readonly PropertyInfo<DateSplitter> DateOfBirthProperty =
            RegisterProperty<DateSplitter>(nameof(DateOfBirth));
       

        public DateSplitter DateOfBirth
        {
            get { return GetProperty(DateOfBirthProperty); }
            set { SetProperty(DateOfBirthProperty, value); }
        }
        public static readonly PropertyInfo<int?> NationalityProperty =
            RegisterProperty<int?>(nameof(Nationality));

        public int? Nationality
        {
            get { return GetProperty(NationalityProperty); }
            set { SetProperty(NationalityProperty, value); }
        }

        public static readonly PropertyInfo<int?> IdTypeProperty =
            RegisterProperty<int?>(nameof(IdType));
      
        public int? IdType
        {
            get { return GetProperty(IdTypeProperty); }
            set { SetProperty(IdTypeProperty, value); }
        }

        public static readonly PropertyInfo<string> IdProperty =
            RegisterProperty<string>(nameof(Id));
       

        public string Id
        {
            get { return GetProperty(IdProperty); }
            set { SetProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<string> TPINProperty =
           RegisterProperty<string>(nameof(TPIN));
       

        public string TPIN
        {
            get { return GetProperty(TPINProperty); }
            set { SetProperty(TPINProperty, value); }
        }

        public static readonly PropertyInfo<string> NHIMAAccountNumberProperty =
           RegisterProperty<string>(nameof(NHIMAAccountNumber));
        
        public string NHIMAAccountNumber
        {
            get { return GetProperty(NHIMAAccountNumberProperty); }
            set { SetProperty(NHIMAAccountNumberProperty, value); }
        }

        


        public static readonly PropertyInfo<string> PhoneNoProperty =
            RegisterProperty<string>(nameof(PhoneNo));
       
        public string PhoneNo
        {
            get { return GetProperty(PhoneNoProperty); }
            set { SetProperty(PhoneNoProperty, value); }
        }

        public static readonly PropertyInfo<bool> IsDifferentlyAbledProperty = RegisterProperty<bool>(nameof(IsDifferentlyAbled));
       
        public bool IsDifferentlyAbled
        {
            get => GetProperty(IsDifferentlyAbledProperty);
            set => SetProperty(IsDifferentlyAbledProperty, value);
        }

        public static readonly PropertyInfo<string> NextOfKinProperty = RegisterProperty<string>(nameof(NextOfKin));
        
        public string NextOfKin
        {
            get => GetProperty(NextOfKinProperty);
            set => SetProperty(NextOfKinProperty, value);
        }

        public static readonly PropertyInfo<string> RelationWithNextOfKinProperty = RegisterProperty<string>(nameof(RelationWithNextOfKin));
       
        public string RelationWithNextOfKin
        {
            get => GetProperty(RelationWithNextOfKinProperty);
            set => SetProperty(RelationWithNextOfKinProperty, value);
        }

        public static readonly PropertyInfo<string> NextOfKinPhoneNoProperty = RegisterProperty<string>(nameof(NextOfKinPhoneNo));
       
        public string NextOfKinPhoneNo
        {
            get => GetProperty(NextOfKinPhoneNoProperty);
            set => SetProperty(NextOfKinPhoneNoProperty, value);
        }

        public static readonly PropertyInfo<int> AgeProperty = RegisterProperty<int>(nameof(Age));
        [DefaultValueAllowed()]
        public int Age
        {
            get => GetProperty(AgeProperty);
            private set => LoadProperty(AgeProperty, value);
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

            BusinessRules.RuleSet = "Personal Details";


            BusinessRules.AddRule(new DateRequired(DateOfBirthProperty));
            BusinessRules.AddRule(new Required(MaritalStatusProperty) { MessageText = "Marital status is required" });
            BusinessRules.AddRule(new Required(GenderProperty) { MessageText = "Gender is required" });
            BusinessRules.AddRule(new Required(IdTypeProperty) { MessageText = "Id Type is required" });
            BusinessRules.AddRule(new Required(NationalityProperty) { MessageText = "Nationality is required" });
            BusinessRules.AddRule(new Required(IdProperty) { MessageText = "NRC/Passport # is required" });
            BusinessRules.AddRule(new VerifyNRCFormat(IdProperty));
            BusinessRules.AddRule(new Required(SSNProperty) { MessageText = "Social Security # is required" });
            BusinessRules.AddRule(new Required(TPINProperty) { MessageText = "TPIN is required" });
            BusinessRules.AddRule(new Required(NHIMAAccountNumberProperty) { MessageText = "NHIMA Account Number is required" });
            BusinessRules.AddRule(new Required(PhoneNoProperty) { MessageText = "Phone # is required" });
            BusinessRules.AddRule(new Required(EmailAddressProperty) { MessageText = "Email Address is required" });
            BusinessRules.AddRule(new Required(AddressLine1Property) { MessageText = "Address  is required" });
            BusinessRules.AddRule(new ValidateEmailAddress(EmailAddressProperty));
            BusinessRules.AddRule(new Required(NextOfKinProperty) { MessageText = "Next of Kin is required" });
            BusinessRules.AddRule(new Required(NextOfKinPhoneNoProperty) { MessageText = "Phone # for Next of Kin required" });

        }


        [CreateChild]
        private void Create(string ruleSet,
            [Inject]IChildDataPortal<DateSplitter> portal)
        {
            using (BypassPropertyChecks)
            {
                DateOfBirth = portal.CreateChild();
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
            [Inject] IEmployeePersonalDetailsDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new EmployeePersonalDetailsDto
                {
                    TenantId = parent.TenantId,
                    EmployeeId = ((Steps)Parent.Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                .Select(r => (AddEmployeeEmploymentDetailsStep)r)
                                .First().EmployeeEmploymentDetails.EmployeeId,
                    Gender = this.Gender,
                    MaritalStatus = this.MaritalStatus,
                    EmailAddress = this.EmailAddress,
                    AddressLine1 = this.AddressLine1,
                    AddressLine2 = this.AddressLine2,
                    SSN = this.SSN,
                    DateOfBirth = this.DateOfBirth.Date,
                    Nationality = this.Nationality,
                    IdType = this.IdType,
                    Id = this.Id,
                    TPIN = this.TPIN,
                    NHIMAAccountNumber = this.NHIMAAccountNumber,
                    PhoneNo = this.PhoneNo,
                    IsDifferentlyAbled = this.IsDifferentlyAbled,
                    NextOfKin = this.NextOfKin,
                    RelationWithNextOfKin = this.RelationWithNextOfKin,
                    NextOfKinPhoneNo = this.NextOfKinPhoneNo

                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
            }
        }

            [UpdateChild]
            private void Update(TenantOnboardingOrchestrator parent,
             [Inject] IEmployeePersonalDetailsDal dal)
            {
                using (BypassPropertyChecks)
                {
                    var dto = new EmployeePersonalDetailsDto
                    {
                        TenantId=parent.TenantId,
                        EmployeeId = ((Steps)Parent.Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                .Select(r => (AddEmployeeEmploymentDetailsStep)r)
                                .First().EmployeeEmploymentDetails.EmployeeId,
                        Gender = this.Gender,
                        MaritalStatus = this.MaritalStatus,
                        EmailAddress = this.EmailAddress,
                        AddressLine1 = this.AddressLine1,
                        AddressLine2 = this.AddressLine2,
                        SSN = this.SSN,
                        DateOfBirth = this.DateOfBirth.Date,
                        Nationality = this.Nationality,
                        IdType = this.IdType,
                        Id = this.Id,
                        TPIN = this.TPIN,
                        NHIMAAccountNumber = this.NHIMAAccountNumber,
                        PhoneNo = this.PhoneNo,
                        IsDifferentlyAbled = this.IsDifferentlyAbled,
                        NextOfKin = this.NextOfKin,
                        RelationWithNextOfKin = this.RelationWithNextOfKin,
                        NextOfKinPhoneNo = this.NextOfKinPhoneNo,
                        LastChanged=this.TimeStamp

                    };
                    dal.Update(dto);
                    TimeStamp = dto.LastChanged;
                }
            }

        [FetchChild]
        private void Fetch(string tenantId,string employeeId,string ruleSet,
            [Inject] IEmployeePersonalDetailsDal dal,
            [Inject]IChildDataPortal<DateSplitter> portal)
        {
            var dto = dal.Fetch(tenantId, employeeId);
            using (BypassPropertyChecks)
            {
                Gender = dto.Gender;
                MaritalStatus = dto.MaritalStatus;
                EmailAddress = dto.EmailAddress;
                AddressLine1 = dto.AddressLine1;
                AddressLine2 = dto.AddressLine2;
                SSN = dto.SSN;
                DateOfBirth = portal.FetchChild(dto.DateOfBirth);
                Nationality = dto.Nationality;
                IdType = dto.IdType;
                Id = dto.Id;
                TPIN = dto.TPIN;
                NHIMAAccountNumber = dto.NHIMAAccountNumber;
                PhoneNo = dto.PhoneNo;
                IsDifferentlyAbled = dto.IsDifferentlyAbled;
                NextOfKin = dto.NextOfKin;
                RelationWithNextOfKin = dto.RelationWithNextOfKin;
                NextOfKinPhoneNo = dto.NextOfKinPhoneNo;
                TimeStamp = dto.LastChanged;
            }

            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

    }


    
    
}
