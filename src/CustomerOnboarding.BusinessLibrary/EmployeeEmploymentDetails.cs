using Csla;
using CustomerOnboarding.BusinessLibrary.Attributes;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class EmployeeEmploymentDetails :
        BusinessBase<EmployeeEmploymentDetails>, IFullName
    {
        public static readonly PropertyInfo<string> EmployeeIdProperty = RegisterProperty<string>(nameof(EmployeeId));
        public string EmployeeId
        {
            get { return GetProperty(EmployeeIdProperty); }
            set { SetProperty(EmployeeIdProperty, value); }
        }

        public static readonly PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(nameof(FirstName));

        public string FirstName
        {
            get { return GetProperty(FirstNameProperty); }
            set { SetProperty(FirstNameProperty, value); }
        }

        public static readonly PropertyInfo<string> LastNameProperty = RegisterProperty<string>(nameof(LastName));

        public string LastName
        {
            get { return GetProperty(LastNameProperty); }
            set { SetProperty(LastNameProperty, value); }
        }

        public static readonly PropertyInfo<string> FullNameProperty = RegisterProperty<string>(nameof(FullName));

        public string FullName
        {
            get { return GetProperty(FullNameProperty); }
            private set { LoadProperty(FullNameProperty, value); }
        }

        public static readonly PropertyInfo<IEmployeeStatus> StatusProperty = RegisterProperty<IEmployeeStatus>(nameof(Status));

        public IEmployeeStatus Status
        {
            get { return GetProperty(StatusProperty); }
            private set { LoadProperty(StatusProperty, value); }
        }

        public static readonly PropertyInfo<IEmployeeStatus> OldStatusProperty = RegisterProperty<IEmployeeStatus>(nameof(OldStatus));

        public IEmployeeStatus OldStatus
        {
            get { return GetProperty(OldStatusProperty); }
            private set { LoadProperty(OldStatusProperty, value); }
        }


        public static readonly PropertyInfo<int?> StatusIdProperty = RegisterProperty<int?>(nameof(StatusId));

        public int? StatusId
        {
            get { return GetProperty(StatusIdProperty); }
            set { SetProperty(StatusIdProperty, value); }
        }

        public static readonly PropertyInfo<int?> PayTypeProperty =
            RegisterProperty<int?>(nameof(PayType));
        public int? PayType
        {
            get { return GetProperty(PayTypeProperty); }
            set { SetProperty(PayTypeProperty, value); }
        }

        public static readonly PropertyInfo<int?> EmploymentTypeProperty =
            RegisterProperty<int?>(nameof(EmploymentType));
        public int? EmploymentType
        {
            get { return GetProperty(EmploymentTypeProperty); }
            set { SetProperty(EmploymentTypeProperty, value); }
        }

        public static readonly PropertyInfo<DateSplitter> HireDateProperty =
           RegisterProperty<DateSplitter>(nameof(HireDate));
        public DateSplitter HireDate
        {
            get { return GetProperty(HireDateProperty); }
            set { SetProperty(HireDateProperty, value); }
        }

        public static readonly PropertyInfo<int> GroupProperty =
            RegisterProperty<int>(nameof(Group));
        public int? Group
        {
            get { return GetProperty(GroupProperty); }
            set { SetProperty(GroupProperty, value); }
        }

        public static readonly PropertyInfo<int> DeptIdProperty =
            RegisterProperty<int>(nameof(DeptId));
        public int? DeptId
        {
            get { return GetProperty(DeptIdProperty); }
            set { SetProperty(DeptIdProperty, value); }
        }

        public static readonly PropertyInfo<int?> JobTitleProperty =
            RegisterProperty<int?>(nameof(JobTitle));
        public int? JobTitle
        {
            get { return GetProperty(JobTitleProperty); }
            set { SetProperty(JobTitleProperty, value); }
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }

        public static readonly PropertyInfo<int?> ReportsToProperty = RegisterProperty<int?>(nameof(ReportsTo));
        public int? ReportsTo
        {
            get => GetProperty(ReportsToProperty);
            set => SetProperty(ReportsToProperty, value);
        }

        public static readonly PropertyInfo<string?> ReportsToEmployeeIdProperty = RegisterProperty<string?>(nameof(ReportsToEmployeeId));

        [DefaultValueAllowed()]
        public string? ReportsToEmployeeId
        {
            get => GetProperty(ReportsToEmployeeIdProperty);
            private set => LoadProperty(ReportsToEmployeeIdProperty, value);
        }

        public static readonly PropertyInfo<IEmployeeType> EmployeeTypeProperty =
          RegisterProperty<IEmployeeType>(nameof(EmployeeType));
        public IEmployeeType EmployeeType
        {
            get { return GetProperty(EmployeeTypeProperty); }
            private set { LoadProperty(EmployeeTypeProperty, value); }
        }

        public static readonly PropertyInfo<IEmployeeType> OldEmployeeTypeProperty =
            RegisterProperty<IEmployeeType>(nameof(OldEmployeeType));
        private IEmployeeType OldEmployeeType
        {
            get { return GetProperty(OldEmployeeTypeProperty); }
            set { LoadProperty(OldEmployeeTypeProperty, value); }
        }

        public void SetStatus(int id)
        {
            StatusId = id;
            BusinessRules.CheckRules(StatusIdProperty);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.RuleSet = "Employment Details";

            BusinessRules.AddRule(new DateRequired(HireDateProperty));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(EmploymentTypeProperty, OldEmployeeTypeProperty, EmployeeTypeProperty));
            BusinessRules.AddRule(new SetOldEmployeeType(
                 OldEmployeeTypeProperty));
            BusinessRules.AddRule(new CreateEmployeeType(EmployeeTypeProperty));
            BusinessRules.AddRule(new Required(JobTitleProperty) { MessageText = "Job Title is required" });
            BusinessRules.AddRule(new Required(EmploymentTypeProperty) { MessageText = "Employment type is required" });
            BusinessRules.AddRule(new Required(PayTypeProperty) { MessageText = "Pay Type is required" });
            BusinessRules.AddRule(new Required(EmployeeIdProperty) { MessageText = "Employee Id is required" });
            BusinessRules.AddRule(
          new BusinessLibrary.Rules.FullNameRule(FirstNameProperty, FullNameProperty));
            BusinessRules.AddRule(
               new BusinessLibrary.Rules.FullNameRule(LastNameProperty, FullNameProperty));
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(StatusIdProperty, OldStatusProperty, StatusProperty));
          //  BusinessRules.AddRule(new SetOldStatus(OldStatusProperty));
            BusinessRules.AddRule(new SetEmployeeStatus(StatusIdProperty, StatusProperty));

            BusinessRules.RuleSet = "Default";
            BusinessRules.AddRule(
         new BusinessLibrary.Rules.FullNameRule(FirstNameProperty, FullNameProperty));
            BusinessRules.AddRule(
               new BusinessLibrary.Rules.FullNameRule(LastNameProperty, FullNameProperty));
        }

        [CreateChild]
        private void Create(string ruleSet, [Inject] IChildDataPortal<DateSplitter> portal)
        {
            using (BypassPropertyChecks)
            {
               // this.StatusId = 1;
                HireDate = portal.CreateChild();
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
            [Inject] IEmployeeEmploymentDetailsDal dal,
            [Inject]ApplicationContext appCtx)
        {
            using (BypassPropertyChecks)
            {
                var dto = new EmployeeEmploymentDetailsDto
                {
                    TenantId=parent.TenantId,
                    EmployeeId=this.EmployeeId,
                    FirstName=this.FirstName,
                    LastName=this.LastName,
                    StatusId=this.StatusId,
                    PayType=this.PayType,
                    EmploymentType=this.EmploymentType,
                    HireDate=this.HireDate.Date,
                    Group=this.Group,
                    DeptId=this.DeptId,
                    JobTitle=this.JobTitle,
                    ReportsTo=this.ReportsTo,
                    ReportsToEmployeeId=this.ReportsToEmployeeId
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;

                UpdateEmployeeStatus(Status, appCtx, parent);
                UpdateEmployeeType(EmployeeType, appCtx, parent);
                
            }

        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent,
            [Inject] IEmployeeEmploymentDetailsDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new EmployeeEmploymentDetailsDto
                {
                    TenantId = parent.TenantId,
                    EmployeeId = this.EmployeeId,
                    FirstName = this.FirstName,
                    LastName = this.LastName,
                    StatusId = this.StatusId,
                    PayType = this.PayType,
                    EmploymentType = this.EmploymentType,
                    HireDate = this.HireDate.Date,
                    Group = this.Group,
                    DeptId = this.DeptId,
                    JobTitle = this.JobTitle,
                    ReportsTo = this.ReportsTo,
                    ReportsToEmployeeId = this.ReportsToEmployeeId,
                    LastChanged=this.TimeStamp
                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
            }

        }

        [FetchChild]
        private void Fetch(string tenantId,string employeeId,
            string ruleSet,
            [Inject]IEmployeeEmploymentDetailsDal dal,
            [Inject] IChildDataPortal<DateSplitter> portal,
            [Inject]IDataPortalFactory factory)
        {
            using (BypassPropertyChecks)
            {
                var dto = dal.Fetch(tenantId, employeeId);
                EmployeeId = dto.EmployeeId;
                FirstName = dto.FirstName;
                LastName = dto.LastName;
                StatusId = dto.StatusId;
                PayType = dto.PayType;
                EmploymentType = dto.EmploymentType;
                HireDate = portal.FetchChild(dto.HireDate);
                Group = dto.Group;
                DeptId = dto.DeptId;
                JobTitle = dto.JobTitle;
                ReportsTo = dto.ReportsTo;
                ReportsToEmployeeId = dto.ReportsToEmployeeId;
                TimeStamp = dto.LastChanged;

                Status = factory.GetPortal<EmployeeStatusFactory>().Fetch(tenantId,StatusId,employeeId,ruleSet).Result;
                EmployeeType = factory.GetPortal<EmployeeTypeFactory>().Fetch(tenantId, employeeId, EmploymentType).Result;
            }
            BusinessRules.RuleSet = ruleSet;
            BusinessRules.CheckRules();
        }

        private void UpdateEmployeeType(IEmployeeType type, ApplicationContext appCtx,
            params object[] parameters)
        {
            if (type != null)
            {
                var dpType = typeof(IChildDataPortal<>).MakeGenericType(type.GetType());
                var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
                dp.UpdateChild(type, parameters);
            }



        }

        private void UpdateEmployeeStatus(IEmployeeStatus status, ApplicationContext appCtx,
           params object[] parameters)
        {
            if (status != null)
            {
                var dpType = typeof(IChildDataPortal<>).MakeGenericType(status.GetType());
                var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);
                dp.UpdateChild(status, parameters);
            }



        }

    }
}
