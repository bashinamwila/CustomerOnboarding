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
    public class UserOrganisation : BusinessBase<UserOrganisation>
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

        public static readonly PropertyInfo<string> CountryProperty =
           RegisterProperty<string>(nameof(Country));
        public string Country
        {
            get => GetProperty(CountryProperty);
            set => SetProperty(CountryProperty, value);
        }

        
        
        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }

       

        

        public static readonly PropertyInfo<int> NumberOfEmployeesProperty =
            RegisterProperty<int>(nameof(NumberOfEmployees));
        public int NumberOfEmployees
        {
            get => GetProperty(NumberOfEmployeesProperty);
            set => SetProperty(NumberOfEmployeesProperty, value);
        }



        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.RuleSet = "Create Account";
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(NameProperty) { MessageText = "Organisation Name is required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(CountryProperty) { MessageText = "Country is required" });
            BusinessRules.AddRule(new CustomerOnboarding.BusinessLibrary.Rules.Required(NumberOfEmployeesProperty) { MessageText = "Number of Employees is required" });

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
        private void Insert(UserOnboardingOrchestrator parent,
            [Inject]IOrganisationDal dal)
        {
            var data = new OrganisationDto
            {
                Id = parent.TenantId,
                Name = this.Name,
                Country = this.Country,
                NumberOfEmployees=this.NumberOfEmployees
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
                this.Country = data.Country;
                this.NumberOfEmployees = data.NumberOfEmployees;
                this.TimeStamp = data.LastChanged!;

                BusinessRules.RuleSet = ruleSet;

            }
        }
    }
}
