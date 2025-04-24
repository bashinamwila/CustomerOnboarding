using Csla.Core;
using Csla.Rules;
using Csla;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    [Serializable]
    public abstract class EmployeeVariableBase<T> : BusinessBase<T>, IEmployeeVariable
        where T : BusinessBase<T>
    {
        public static readonly PropertyInfo<int> IdProperty =
           RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            protected set { LoadProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<string> TokenProperty =
            RegisterProperty<string>(nameof(Token));
        public string Token
        {
            get { return GetProperty(TokenProperty); }
            protected set { LoadProperty(TokenProperty, value); }
        }

        public static readonly PropertyInfo<decimal?> ValueProperty = RegisterProperty<decimal?>(nameof(Value));

        public decimal? Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }

        public static readonly PropertyInfo<bool> CanSetValueProperty =
           RegisterProperty<bool>(nameof(CanSetValue));
        public bool CanSetValue
        {
            get { return GetProperty(CanSetValueProperty); }
            protected set { LoadProperty(CanSetValueProperty, value); }
        }

        public static readonly PropertyInfo<byte[]> TimeStamProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStamProperty); }
            set { SetProperty(TimeStamProperty, value); }
        }

        public void CheckBusinessRules()
        {
            BusinessRules.CheckRules(ValueProperty);
        }

        [CreateChild]
        protected virtual void Create(IVariableInfo variable, [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                this.Id = variable.Id;
                this.Token = variable.Token;


                if (variable is IDefaultValueInfo variablewithDefaultValue)
                    this.Value = variablewithDefaultValue.Value;

            }
        }

        [InsertChild]
        protected virtual void Insert(TenantOnboardingOrchestrator parent,
            [Inject] IEmployeeVariableDal dal,
            [Inject] ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var data = new EmployeeVariableDto
                {
                    TenantId=parent.TenantId,
                    EmployeeId = ((Steps)Parent.Parent.Parent.Parent).Where(r=>r is AddEmployeeEmploymentDetailsStep)
                                    .Select(r=>(AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                    .EmployeeEmploymentDetails.EmployeeId!,
                    ItemId = ((IEmployeeWageOrAccrual)Parent.Parent.Parent).Id,
                    Id = this.Id,
                    Value = this.Value

                };
                dal.Insert(data);
                this.TimeStamp = data.LastChanged;
            }
        }

        [FetchChild]
        protected virtual void Fetch(string employeeId, string wageId, int id,
            [Inject] IEmployeeVariableDal dal,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(employeeId, wageId, id);
                Id = data.Id;
                Token = data.Token;
                Value = data.Value;
                TimeStamp = data.LastChanged;

            }
        }

        [UpdateChild]
        protected virtual void Update(TenantOnboardingOrchestrator parent,
            [Inject] IEmployeeVariableDal dal, [Inject] ApplicationContext appCtx)
        { }


    }
}
