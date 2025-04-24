using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using CustomerOnboarding.BusinessLibrary.Rules;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class Wage : BusinessBase<Wage>, IEarning, IFormular
    {
        public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(nameof(Id), "Id");
       
        public string Id
        {
            get { return GetProperty(IdProperty); }
            set { SetProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>(nameof(Name), "Name");
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        public static readonly PropertyInfo<int?> TypeProperty = RegisterProperty<int?>(nameof(Type), "Type");
        [Display(Name = "Type")]
        public int? Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }

        public static readonly PropertyInfo<bool> IsSystemDefinedProperty =
            RegisterProperty<bool>(nameof(IsSystemDefined));
        [Display(Name = "Is System Defined")]
        public bool IsSystemDefined
        {
            get { return GetProperty(IsSystemDefinedProperty); }
            private set { LoadProperty(IsSystemDefinedProperty, value); }
        }



        public static readonly PropertyInfo<byte[]> TimeStamProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStamProperty); }
            set { SetProperty(TimeStamProperty, value); }
        }

        public static readonly PropertyInfo<string> FormularProperty = RegisterProperty<string>(nameof(Formular));
        [Display(Name = "Equals")]
        public string Formular
        {
            get { return GetProperty(FormularProperty); }
            set { SetProperty(FormularProperty, value); }
        }

        public static readonly PropertyInfo<Variables> VariablesProperty =
            RegisterProperty<Variables>(nameof(Variables));
        public Variables Variables
        {
            get => GetProperty(VariablesProperty);
            private set => LoadProperty(VariablesProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            BusinessRules.RuleSet = "Add Compensation Component";
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty, "Id is required"));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.RegExMatch(IdProperty, "^1[0-9]{3}$", "The id of the wage should be between 1000 and 1999"));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(NameProperty, "Name is required"));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(FormularProperty, "Formular is required"));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(FormularProperty, VariablesProperty));
            BusinessRules.AddRule(
                new ExtractVariablesFromFormular(VariablesProperty, FormularProperty));
            BusinessRules.AddRule(new EnsureThatFormularIsValid(FormularProperty));
            BusinessRules.AddRule(new NoDuplicates(IdProperty));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(FormularProperty, TypeProperty));
            BusinessRules.AddRule(new SalesCommissionShouldHaveTotalSalesVariable(FormularProperty, TypeProperty));
            BusinessRules.AddRule(new HourlyWageShouldContainAtLeastMultiplierAndTotalHoursWorkedVariables(FormularProperty, TypeProperty));
            BusinessRules.AddRule(new SalaryCannotContainMultiplierTotalSalesAndTotalHoursWorkedVariables(FormularProperty, TypeProperty));
            BusinessRules.AddRule(new EditVariablesToReflectChangesInFormular(VariablesProperty, FormularProperty));
        }

        [CreateChild]
        private void Create(string ruleSet,[Inject] IChildDataPortal<Variables> portal)
        {
            using (BypassPropertyChecks)
            {
                IsSystemDefined = false;

                Variables = portal.CreateChild();

            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();

        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,[Inject] IWageDal dal,
            [Inject] IChildDataPortal<Variables> portal)
        {
            using (BypassPropertyChecks)
            {
                var data = new WageDto
                {
                    TenantId=parent.TenantId,
                    Id = this.Id,
                    Name = this.Name,
                    Type = this.Type!.Value,
                    IsSystemDefined = this.IsSystemDefined,
                    Formular = this.Formular
                };
                dal.Insert(data);
                this.TimeStamp = data.LastChanged!;
                await portal.UpdateChildAsync(Variables, parent);


            }
        }

        [FetchChild]
        private void Fetch(string tenantId,string id,string ruleSet, [Inject] IWageDal dal,
            [Inject] IChildDataPortal<Variables> portal)
        {
            var data = dal.Fetch(tenantId,id);
            using (BypassPropertyChecks)
            {
                this.Id = data.Id;
                this.Name = data.Name;
                this.Type = data.Type;
                this.IsSystemDefined = data.IsSystemDefined;
                this.Formular = data.Formular;
                this.TimeStamp = data.LastChanged!;
                this.Variables = portal.FetchChild(id);

            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [UpdateChild]
        private async Task UpdateAsync(TenantOnboardingOrchestrator parent,[Inject] IWageDal dal,
            [Inject] IChildDataPortal<Variables> portal)
        {
            using (BypassPropertyChecks)
            {
                var data = new WageDto
                {
                    TenantId=parent.TenantId,
                    Id = this.Id,
                    Name = this.Name,
                    Type = this.Type!.Value,
                    Formular = this.Formular,
                    LastChanged = this.TimeStamp

                };
                dal.Update(data);
                TimeStamp = data.LastChanged;
                await portal.UpdateChildAsync(Variables, parent);


            }
        }

        [DeleteSelfChild]
        private async Task DeleteAsync(string tenantId,string id, [Inject] IWageDal dal,
            [Inject] IChildDataPortal<Variables> portal)
        {
            using (BypassPropertyChecks)
            {
                var variables = await portal.FetchChildAsync(tenantId,id);
                variables.Clear();
                await portal.UpdateChildAsync(variables, id);
                await dal.Delete(tenantId,id);

            }
        }




    }
}
