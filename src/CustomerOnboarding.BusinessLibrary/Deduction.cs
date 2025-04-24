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
    public class Deduction : BusinessBase<Deduction>
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

        public static readonly PropertyInfo<IDeductionType> DeductionTypeProperty = RegisterProperty<IDeductionType>
       (nameof(DeductionType));
        public IDeductionType DeductionType
        {
            get { return GetProperty(DeductionTypeProperty); }
            private set { LoadProperty(DeductionTypeProperty, value); }
        }
        public static readonly PropertyInfo<ILimit> DeductionLimitTypeProperty = RegisterProperty<ILimit>
      (nameof(DeductionLimitType));
        public ILimit DeductionLimitType
        {
            get { return GetProperty(DeductionLimitTypeProperty); }
            private set { LoadProperty(DeductionLimitTypeProperty, value); }
        }
        public static readonly PropertyInfo<ILimit> OldDeductionLimitTypeProperty = RegisterProperty<ILimit>
      (nameof(OldDeductionLimitType));
        private ILimit OldDeductionLimitType
        {
            get { return GetProperty(OldDeductionLimitTypeProperty); }
            set { LoadProperty(OldDeductionLimitTypeProperty, value); }
        }

        public static readonly PropertyInfo<IDeductionType> OldDeductionTypeProperty = RegisterProperty<IDeductionType>
       (nameof(OldDeductionType));
        private IDeductionType OldDeductionType
        {
            get { return GetProperty(OldDeductionTypeProperty); }
            set { LoadProperty(OldDeductionTypeProperty, value); }
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

            BusinessRules.RuleSet = "Add Deduction";
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(IdProperty, "Id is required"));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Required(NameProperty, "Name is required"));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.RegExMatch(IdProperty, "^2[0-9]{3}$", "The id of the deduction should be between 2000 and 2999"));
            BusinessRules.AddRule(new SetOldDeductionType(TypeProperty, OldDeductionTypeProperty) { Priority = 1 });
            BusinessRules.AddRule(new SetDeductionType(TypeProperty, DeductionTypeProperty) { Priority = 2 });
            BusinessRules.AddRule(new NoDuplicates(IdProperty));
            BusinessRules.AddRule(new SetOldDeductionLimitType(LimitProperty, OldDeductionLimitTypeProperty) { Priority = 1 });
            BusinessRules.AddRule(new SetDeductionLimit(LimitProperty, DeductionLimitTypeProperty) { Priority = 2 });


        }

        




        [CreateChild]
        private void Create(string ruleSet)
        {
            using (BypassPropertyChecks)
            {
                IsSystemDefined = false;

            }
            BusinessRules.RuleSet=ruleSet;
            BusinessRules.CheckRules();
        }

       

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,[Inject] IDeductionDal dal,
            [Inject] ApplicationContext appCtx)
        {
             InsertData(parent,dal, appCtx);
        }

        private void InsertData(TenantOnboardingOrchestrator parent,IDeductionDal dal, ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var item = new DeductionDto
                {
                    Id = this.Id,
                    TenantId=parent.TenantId,
                    Name = this.Name,
                    IsSystemDefined = this.IsSystemDefined,
                    Type = this.Type,
                    Limit = this.Limit
                };
                 dal.Insert(item);
                TimeStamp = item.LastChanged!;

                UpdateDeductionTypeChild(appCtx, DeductionType, parent);
                UpdateDeductionLimitTypeChild(appCtx, DeductionLimitType, parent);
            }
        }


       



        private void FetchData(string tenantId,string id, string ruleSet,IDeductionDal dal, IDataPortalFactory portalFactory)
        {
            var item = dal.Fetch(tenantId,id);
            using (BypassPropertyChecks)
            {
                this.Id = item.Id;
                this.Name = item.Name;
                this.IsSystemDefined = item.IsSystemDefined;
                this.TimeStamp = item.LastChanged!;
                this.Type = item.Type;
                this.Limit = item.Limit;
                this.DeductionType = portalFactory.GetPortal<DeductionTypeFactory>().Fetch(Type,tenantId, id,ruleSet).Result;
                this.DeductionLimitType = portalFactory.GetPortal<DeductionLimitFactory>().Fetch(Limit,tenantId, Id,ruleSet).Result;
            }
        }

        [FetchChild]
        private void Fetch(string tenantId,string id,string ruleSet, [Inject] IDeductionDal dal,
            [Inject] IDataPortalFactory portalFactory)
        {
            FetchData(tenantId,id,ruleSet,dal, portalFactory);
            BusinessRules.RuleSet= ruleSet;
        }
        

        private void UpdateData(TenantOnboardingOrchestrator parent,IDeductionDal dal, [Inject] ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var item = new DeductionDto
                {
                    Id = this.Id,
                    TenantId=parent.TenantId,
                    Name = this.Name,
                    Type = this.Type,
                    Limit = this.Limit,
                    LastChanged = this.TimeStamp
                };

                dal.Update(item);
                this.TimeStamp = item.LastChanged;
                if (OldDeductionType != null)
                    UpdateDeductionTypeChild(appCtx, OldDeductionType, this);
                if (OldDeductionLimitType != null)
                    UpdateDeductionLimitTypeChild(appCtx, OldDeductionLimitType, this);
                UpdateDeductionTypeChild(appCtx, DeductionType, this);
                UpdateDeductionLimitTypeChild(appCtx, DeductionLimitType, this);


            }
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent,[Inject] IDeductionDal dal,
            [Inject] ApplicationContext appCtx)
        {
             UpdateData(parent,dal, appCtx);
        }
        
        private void DeleteData(string tenantId,string id, IDeductionDal dal,
           IDataPortalFactory portalFactory,
            ApplicationContext appCtx)
        {
            var item = dal.Fetch(tenantId,id);
            var type = portalFactory.GetPortal<DeductionTypeFactory>().Fetch(item.Type,tenantId, id).Result;
            var limit = portalFactory.GetPortal<DeductionLimitFactory>().Fetch(item.Limit,tenantId, id).Result;
            type.DeleteChild();
            limit.DeleteChild();
            UpdateDeductionTypeChild(appCtx, type,tenantId, id);
            UpdateDeductionLimitTypeChild(appCtx, limit,tenantId, id);
             dal.Delete(tenantId,id);
        }
       
        [DeleteSelfChild]
        private void Delete(string tenantId,string id, [Inject] IDeductionDal dal, [Inject] IDataPortalFactory portalFactory,
            [Inject] ApplicationContext appCtx)
        {
            DeleteData(tenantId,id, dal, portalFactory, appCtx);
        }
        private void UpdateDeductionTypeChild(ApplicationContext appCtx, IDeductionType type, params object[] crit)
        {
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type.GetType());
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            dp.UpdateChild(type, crit);
        }

        private void UpdateDeductionLimitTypeChild(ApplicationContext appCtx, ILimit type, params object[] crit)
        {
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type.GetType());
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            dp.UpdateChild(type, crit);
        }
    }
}

