using Csla.Core;
using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.Rules;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeeSalary : BusinessBase<EmployeeSalary>, IEmployeeEarningOrAccrualType
    {

        public static readonly PropertyInfo<int> IdProperty =
            RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get => GetProperty(IdProperty);
            private set => LoadProperty(IdProperty, value);
        }
        public static readonly PropertyInfo<string> FormularProperty =
            RegisterProperty<string>(nameof(Formular));
        public string Formular
        {
            get => GetProperty(FormularProperty);
            private set => LoadProperty(FormularProperty, value);
        }

        public static readonly PropertyInfo<EmployeeVariables> VariablesProperty =
            RegisterProperty<EmployeeVariables>(nameof(Variables));
        public EmployeeVariables Variables
        {
            get => GetProperty(VariablesProperty);
            private set => LoadProperty(VariablesProperty, value);
        }
        public static readonly PropertyInfo<decimal> AmountProperty =
            RegisterProperty<decimal>(nameof(Amount));
        public decimal Amount
        {
            get => GetProperty(AmountProperty);
            private set => LoadProperty(AmountProperty, value);

        }

        public static readonly PropertyInfo<string> TextValueProperty =
           RegisterProperty<string>(nameof(TextValue));
        public string TextValue
        {
            get => GetProperty(TextValueProperty);
            private set => LoadProperty(TextValueProperty, value);

        }

        public void CheckBusinessRules()
        {
            foreach (var variable in Variables)
                variable.CheckBusinessRules();
            BusinessRules.CheckRules(AmountProperty);

        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new EvaluateAmountExpression(AmountProperty, FormularProperty, VariablesProperty) { Priority = 1 });
            BusinessRules.AddRule(new ConvertAmountToText(AmountProperty, TextValueProperty) { Priority = 2 });
        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is IEmployeeVariable)
            {
                BusinessRules.CheckRules(AmountProperty);
            }
            base.OnChildChanged(e);
        }

        [CreateChild]
        private void Create(WageInfo info,
            [Inject] IChildDataPortal<EmployeeVariables> portal)
        {
            using (BypassPropertyChecks)
            {
                this.Id = info.Type;
                this.Formular = info.Formular;
                this.Variables = portal.CreateChild(info.Variables);
            }
            BusinessRules.CheckRules();
        }

        [FetchChild]
        private void Fetch(string tenantId,int id, string formular,
            string employeeId, string wageId,
            [Inject] IChildDataPortal<EmployeeVariables> portal)
        {
            using (BypassPropertyChecks)
            {
                this.Id = id;
                this.Formular = formular;
                this.Variables = portal.FetchChild(tenantId,employeeId, wageId);
            }
        }

        [InsertChild]
        private async Task InsertAsync(TenantOnboardingOrchestrator parent,
            [Inject] IChildDataPortal<EmployeeVariables> portal)
        {
            await portal.UpdateChildAsync(Variables, parent);
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent,
             [Inject] IChildDataPortal<EmployeeVariables> portal)
        {
            //TO DO:Implement logic to update Employee Variables
        }
    }
}
