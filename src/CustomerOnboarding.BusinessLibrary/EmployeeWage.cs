using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using CustomerOnboarding.BusinessLibrary.Rules;

namespace CustomerOnboarding.BusinessLibrary
{
    public class EmployeeWage :BusinessBase<EmployeeWage>,
        IEmployeeWageOrAccrual
    {
        public static readonly PropertyInfo<string> IdProperty = RegisterProperty<string>(nameof(Id));

        public string Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }


        public static readonly PropertyInfo<string> NameProperty =
           RegisterProperty<string>(nameof(Name));

        public string Name
        {
            get { return GetProperty(NameProperty); }
            private set { LoadProperty(NameProperty, value); }
        }

        public static readonly PropertyInfo<DateSplitter> EffectiveDateProperty =
           RegisterProperty<DateSplitter>(nameof(EffectiveDate));

        [Display(Name = "Effective Date")]
        public DateSplitter EffectiveDate
        {
            get { return GetProperty(EffectiveDateProperty); }
            set { SetProperty(EffectiveDateProperty, value); }
        }
        public static readonly PropertyInfo<bool> IsSystemDefinedProperty = RegisterProperty<bool>(nameof(IsSystemDefined));

        public bool IsSystemDefined
        {
            get { return GetProperty(IsSystemDefinedProperty); }
            private set { LoadProperty(IsSystemDefinedProperty, value); }
        }

        public static readonly PropertyInfo<decimal> YTDProperty = RegisterProperty<decimal>(nameof(YTD));
        public decimal YTD
        {
            get => GetProperty(YTDProperty);
            private set => LoadProperty(YTDProperty, value);
        }

        public static readonly PropertyInfo<IEmployeeEarningOrAccrualType> TypeProperty =
            RegisterProperty<IEmployeeEarningOrAccrualType>(nameof(Type));
        public IEmployeeEarningOrAccrualType Type
        {
            get => GetProperty(TypeProperty);
            private set => LoadProperty(TypeProperty, value);
        }

        public static readonly PropertyInfo<byte[]>
           TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }



        protected override void OnChildChanged(ChildChangedEventArgs e)
        {

            if (e.ChildObject is DateSplitter)
            {
                BusinessRules.CheckRules(EffectiveDateProperty);
            }
            base.OnChildChanged(e);
        }


        internal void CheckBusinessRules()
        {
            foreach (var variable in Type.Variables)
            {
                variable.CheckBusinessRules();
            }
            Type.CheckBusinessRules();
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new DateRequired(EffectiveDateProperty));
        }

        [CreateChild]
        private void Create(WageInfo info,
            [Inject] IChildDataPortal<DateSplitter> portal,
            [Inject] IDataPortal<EmployeeEarningOrAccrualTypeFactory> dp)
        {
            using (BypassPropertyChecks)
            {
                this.Id = info.Id;
                this.Name = info.Name;
                this.IsSystemDefined = info.IsSystemDefined;
                this.EffectiveDate = portal.CreateChild();
                this.Type = dp.Fetch(info, "WAGE").Result;

            }
            BusinessRules.CheckRules();
        }



        [CreateChild]
        private void Create(string id,
            [Inject] IDataPortal<WageInfo> portal,
             [Inject] IChildDataPortal<DateSplitter> childPortal,
             [Inject] IDataPortal<EmployeeEarningOrAccrualTypeFactory> dp
            )
        {
            Create(portal.Fetch(id), childPortal, dp);
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent, [Inject] IEmployeeWageDal dal,
            [Inject] ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var data = new EmployeeWageDto
                {
                    TenantId=parent.TenantId,
                    EmployeeId = ((Steps)Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                .EmployeeEmploymentDetails.EmployeeId!,
                    WageId = this.Id,
                    EffectiveDate = this.EffectiveDate.Date!.Value,

                };
                dal.Insert(data);
                this.TimeStamp = data.LastChanged!;
                UpdateChild(appCtx, Type, parent);
            }


        }


        private void UpdateChild(ApplicationContext appCtx,
            IEmployeeEarningOrAccrualType type, TenantOnboardingOrchestrator parent)
        {
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type.GetType());
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
            dp.UpdateChild(type, parent);
        }
        [FetchChild]
        private void Fetch(EmployeeWageDto data,
            [Inject] IChildDataPortal<DateSplitter> portal,
            [Inject] IDataPortal<EmployeeEarningOrAccrualTypeFactory> portalFactory)
        {

            using (BypassPropertyChecks)
            {
                this.Id = data.WageId;
                this.Name = data.Name;
                this.EffectiveDate = portal.FetchChild(data.EffectiveDate);
                this.IsSystemDefined = data.IsSystemDefined;
                this.YTD = data.YTD;
                this.TimeStamp = data.LastChanged!;

                Type = portalFactory.Fetch(data.Type, data.Formular, data.EmployeeId, data.WageId).Result;
            }

            BusinessRules.CheckRules();
        }

        [FetchChild]
        private void Fetch(string tenantId,string employeeId, string id,
            [Inject] IChildDataPortal<DateSplitter> portal,
            [Inject] IDataPortal<EmployeeEarningOrAccrualTypeFactory> portalFactory,
            [Inject] IEmployeeWageDal dal)
        {

            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId,employeeId, id);
                this.Id = data.WageId;
                this.Name = data.Name;
                this.EffectiveDate = portal.FetchChild(data.EffectiveDate);
                this.IsSystemDefined = data.IsSystemDefined;
                this.YTD = data.YTD;
                this.TimeStamp = data.LastChanged!;

                Type = portalFactory.Fetch(data.Type, data.Formular, data.EmployeeId, data.WageId).Result;
            }

            BusinessRules.CheckRules();
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent, [Inject] IEmployeeWageDal dal,
           [Inject] ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var data = new EmployeeWageDto
                {
                    TenantId=parent.TenantId,
                    EmployeeId = ((Steps)Parent).Where(r=>r is AddEmployeeEmploymentDetailsStep)
                                .Select(r=>(AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                .EmployeeEmploymentDetails.EmployeeId!,
                    WageId = this.Id,
                    EffectiveDate = this.EffectiveDate.Date!.Value,
                    LastChanged = this.TimeStamp

                };
                dal.Update(data);
                this.TimeStamp = data.LastChanged;
                UpdateChild(appCtx, Type, parent);
            }


        }

        /*[FetchChild]
        private void Fetch(string employeeId,
            string wageId, [Inject] IEmployeeEarningDal dal,
             [Inject] IChildDataPortal<DateSplitter> portal)
        {
            var data = dal.Fetch(employeeId, wageId);
            using (BypassPropertyChecks)
            {
                this.Id = data.WageId;
                this.Name = data.Name;
                this.EffectiveDate = portal.FetchChild(data.EffectiveDate);
                this.IsSystemDefined = data.IsSystemDefined;
                this.YTD = data.YTD;
                this.TimeStamp = data.LastChanged!;
                

            }

            BusinessRules.CheckRules();
        }

       
        [DeleteSelfChild]
        private void Delete(Employee parent, [Inject] IEmployeeEarningDal dal,
            [Inject] ApplicationContext appCtx)
        {
            WageType.DeleteChild();
            UpdateChild(appCtx, WageType, parent);
            dal.Delete(parent.EmployeeId, this.Id);
        }*/


    }
}
