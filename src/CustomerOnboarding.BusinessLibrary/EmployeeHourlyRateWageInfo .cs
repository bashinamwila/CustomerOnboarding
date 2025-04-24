using Csla.Rules;
using Csla;
using CustomerOnboarding.BusinessLibrary.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.BaseTypes;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeeHourlyRateWageInfo : ReadOnlyBase<EmployeeHourlyRateWageInfo>, IEmployeeEarningOrAccrualTypeInfo
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

        public static readonly PropertyInfo<EmployeeVariableList> VariablesProperty =
            RegisterProperty<EmployeeVariableList>(nameof(Variables));
        public EmployeeVariableList Variables
        {
            get => GetProperty(VariablesProperty);
            private set => LoadProperty(VariablesProperty, value);
        }
        public static readonly PropertyInfo<decimal> HourlyRateProperty =
            RegisterProperty<decimal>(nameof(HourlyRate));
        public decimal HourlyRate
        {
            get => GetProperty(HourlyRateProperty);
            private set => LoadProperty(HourlyRateProperty, value);

        }

        public static readonly PropertyInfo<string> TextValueProperty =
           RegisterProperty<string>(nameof(TextValue));
        public string TextValue
        {
            get => GetProperty(TextValueProperty);
            private set => LoadProperty(TextValueProperty, value);

        }



        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new EvaluateHourlyRateExpression(HourlyRateProperty, FormularProperty, VariablesProperty) { Priority = 1 });
            BusinessRules.AddRule(new ConvertHourlyRateToText(HourlyRateProperty, TextValueProperty) { Priority = 2 });
        }




        [FetchChild]
        private void Fetch(int id, string formular,
            string employeeId, string wageId,
            [Inject] IChildDataPortal<EmployeeVariableList> portal)
        {

            Id = id;
            Formular = formular;
            Variables = portal.FetchChild(employeeId, wageId);

            BusinessRules.CheckRules(HourlyRateProperty);
        }


    }
}
