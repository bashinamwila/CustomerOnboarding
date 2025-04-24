using System;
using System.Collections.Generic;
using System.Text;
using Csla;
using Csla.Rules;
using CustomerOnboarding.Dal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System.Collections;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.BusinessLibrary.Rules;
using Csla.Core;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployerExpense : BusinessBase<EmployerExpense>
    {
        public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>
        (nameof(Id));
        [Display(Name = "Id")]
        public string Id
        {
            get { return GetProperty(IdProperty); }
            set { SetProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<string> NameProperty = RegisterProperty<string>
        (nameof(Name));
        [Display(Name = "Name")]
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        public static readonly PropertyInfo<bool> IsSystemDefinedProperty = RegisterProperty<bool>(nameof(IsSystemDefined));
        [Display(Name = "Is System Defined")]
        public bool IsSystemDefined
        {
            get { return GetProperty(IsSystemDefinedProperty); }
            private set { LoadProperty(IsSystemDefinedProperty, value); }
        }

        public static readonly PropertyInfo<int> TypeProperty = RegisterProperty<int>(nameof(Type));
        [Display(Name = "Type")]
        public int Type
        {
            get { return GetProperty(TypeProperty); }
            private set { SetProperty(TypeProperty, value); }
        }

        public static readonly PropertyInfo<IEmployerExpenseType> EmployerExpenseTypeProperty = RegisterProperty<IEmployerExpenseType>
       (nameof(EmployerExpenseType));
        public IEmployerExpenseType EmployerExpenseType
        {
            get { return GetProperty(EmployerExpenseTypeProperty); }
            private set { LoadProperty(EmployerExpenseTypeProperty, value); }
        }
        public static readonly PropertyInfo<ILimit> EmployerExpenseLimitTypeProperty = RegisterProperty<ILimit>
      (nameof(EmployerExpenseLimitType));
        public ILimit EmployerExpenseLimitType
        {
            get { return GetProperty(EmployerExpenseLimitTypeProperty); }
            private set { LoadProperty(EmployerExpenseLimitTypeProperty, value); }
        }
        public static readonly PropertyInfo<ILimit> OldEmployerExpenseLimitTypeProperty = RegisterProperty<ILimit>
      (nameof(OldEmployerExpenseLimitType));
        private ILimit OldEmployerExpenseLimitType
        {
            get { return GetProperty(OldEmployerExpenseLimitTypeProperty); }
            set { LoadProperty(OldEmployerExpenseLimitTypeProperty, value); }
        }

        public static readonly PropertyInfo<IEmployerExpenseType> OldEmployerExpenseTypeProperty = RegisterProperty<IEmployerExpenseType>
       (nameof(OldEmployerExpenseType));
        private IEmployerExpenseType OldEmployerExpenseType
        {
            get { return GetProperty(OldEmployerExpenseTypeProperty); }
            set { LoadProperty(OldEmployerExpenseTypeProperty, value); }
        }

        public static readonly PropertyInfo<int> LimitProperty = RegisterProperty<int>(nameof(Limit));
        public int Limit
        {
            get { return GetProperty(LimitProperty); }
            set { SetProperty(LimitProperty, value); }
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }

        public void SetType(int id)
        {
            Type = id;
            BusinessRules.CheckRules(TypeProperty);
        }

        public void SetLimit(int id)
        {
            Limit = id;
            BusinessRules.CheckRules(LimitProperty);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            BusinessRules.RuleSet = "Add EmployerExpense";
           
            //BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty, "Id is required"));
            //BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(NameProperty, "Name is required"));
            //BusinessRules.AddRule(new Csla.Rules.CommonRules.RegExMatch(IdProperty, "^2[0-9]{3}$", "The id of the Employer Expense should be between 2000 and 2999"));
            BusinessRules.AddRule(new SetOldEmployerExpenseType(TypeProperty, OldEmployerExpenseTypeProperty) { Priority = 1 });
            BusinessRules.AddRule(new SetEmployerExpenseType(TypeProperty, EmployerExpenseTypeProperty) { Priority = 2 });
            BusinessRules.AddRule(new NoDuplicates(IdProperty));
            BusinessRules.AddRule(new SetOldEmployerExpenseLimitType(LimitProperty, OldEmployerExpenseLimitTypeProperty) { Priority = 1 });
            BusinessRules.AddRule(new SetEmployerExpenseLimit(LimitProperty, EmployerExpenseLimitTypeProperty) { Priority = 2 });


        }






        [CreateChild]
        private void Create(string ruleSet)
        {
            using (BypassPropertyChecks)
            {
                IsSystemDefined = false;

            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }



        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] IEmployerExpenseDal dal,
            [Inject] ApplicationContext appCtx)
        {
            InsertData(parent, dal, appCtx);
        }

        private void InsertData(TenantOnboardingOrchestrator parent, IEmployerExpenseDal dal, ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var item = new EmployerExpenseDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    Name = this.Name,
                    IsSystemDefined = this.IsSystemDefined,
                    Type = this.Type,
                    Limit = this.Limit
                };
                dal.Insert(item);
                TimeStamp = item.LastChanged!;

                UpdateEmployerExpenseTypeChild(appCtx, EmployerExpenseType, parent);
                UpdateEmployerExpenseLimitTypeChild(appCtx, EmployerExpenseLimitType, parent);
            }
        }






        private void FetchData(string tenantId, string id, string ruleSet, IEmployerExpenseDal dal, IDataPortalFactory portalFactory)
        {
            var item = dal.Fetch(tenantId, id);
            using (BypassPropertyChecks)
            {
                this.Id = item.Id;
                this.Name = item.Name;
                this.IsSystemDefined = item.IsSystemDefined;
                this.TimeStamp = item.LastChanged!;
                this.Type = item.Type;
                this.Limit = item.Limit;
                this.EmployerExpenseType = portalFactory.GetPortal<EmployerExpenseTypeFactory>().Fetch(Type, tenantId, id, ruleSet).Result;
                this.EmployerExpenseLimitType = portalFactory.GetPortal<EmployerExpenseLimitFactory>().Fetch(Limit, tenantId, Id, ruleSet).Result;
            }
        }

        [FetchChild]
        private void Fetch(string tenantId, string id, string ruleSet, [Inject] IEmployerExpenseDal dal,
            [Inject] IDataPortalFactory portalFactory)
        {
            FetchData(tenantId, id, ruleSet, dal, portalFactory);
            BusinessRules.RuleSet = ruleSet;
        }


        private void UpdateData(TenantOnboardingOrchestrator parent, IEmployerExpenseDal dal, [Inject] ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var item = new EmployerExpenseDto
                {
                    Id = this.Id,
                    TenantId = parent.TenantId,
                    Name = this.Name,
                    Type = this.Type,
                    Limit = this.Limit,
                    LastChanged = this.TimeStamp
                };

                dal.Update(item);
                this.TimeStamp = item.LastChanged;
                if (OldEmployerExpenseType != null)
                    UpdateEmployerExpenseTypeChild(appCtx, OldEmployerExpenseType, this);
                if (OldEmployerExpenseLimitType != null)
                    UpdateEmployerExpenseLimitTypeChild(appCtx, OldEmployerExpenseLimitType, this);
                UpdateEmployerExpenseTypeChild(appCtx, EmployerExpenseType, this);
                UpdateEmployerExpenseLimitTypeChild(appCtx, EmployerExpenseLimitType, this);


            }
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] IEmployerExpenseDal dal,
            [Inject] ApplicationContext appCtx)
        {
            UpdateData(parent, dal, appCtx);
        }

        private void DeleteData(string tenantId, string id, IEmployerExpenseDal dal,
           IDataPortalFactory portalFactory,
            ApplicationContext appCtx)
        {
            var item = dal.Fetch(tenantId, id);
            var type = portalFactory.GetPortal<EmployerExpenseTypeFactory>().Fetch(item.Type, tenantId, id).Result;
            var limit = portalFactory.GetPortal<EmployerExpenseLimitFactory>().Fetch(item.Limit, tenantId, id).Result;
            type.DeleteChild();
            limit.DeleteChild();
            UpdateEmployerExpenseTypeChild(appCtx, type, tenantId, id);
            UpdateEmployerExpenseLimitTypeChild(appCtx, limit, tenantId, id);
            dal.Delete(tenantId, id);
        }

        [DeleteSelfChild]
        private void Delete(string tenantId, string id, [Inject] IEmployerExpenseDal dal, [Inject] IDataPortalFactory portalFactory,
            [Inject] ApplicationContext appCtx)
        {
            DeleteData(tenantId, id, dal, portalFactory, appCtx);
        }
        private void UpdateEmployerExpenseTypeChild(ApplicationContext appCtx, IEmployerExpenseType type, params object[] crit)
        {
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type.GetType());
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            dp.UpdateChild(type, crit);
        }

        private void UpdateEmployerExpenseLimitTypeChild(ApplicationContext appCtx, ILimit type, params object[] crit)
        {
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type.GetType());
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            dp.UpdateChild(type, crit);
        }
    }
}

