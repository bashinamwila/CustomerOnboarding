using CustomerOnboarding.BusinessLibrary.BaseTypes;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class SelectEmployeeWageStep :
        StepBase<SelectEmployeeWageStep>
    {
        public static readonly PropertyInfo<WageList> WageListProperty =
            RegisterProperty<WageList>(nameof(WageList), RelationshipTypes.LazyLoad);

        public WageList WageList
        {
            get => LazyGetProperty<WageList>(WageListProperty,
                () => ApplicationContext.GetRequiredService<IDataPortal<WageList>>().Fetch("123werqop070905mnbfghjkl"));
        }

        public static readonly PropertyInfo<string> WageIdProperty =
            RegisterProperty<string>(nameof(WageId));
        public string WageId
        {
            get => GetProperty(WageIdProperty);
            set => SetProperty(WageIdProperty, value);
        }

        public static readonly PropertyInfo<string> RuleSetProperty =
            RegisterProperty<string>(nameof(RuleSet));

        /// <summary>
        /// Name of the rule set used to validate this step's children.
        /// </summary>
        public string RuleSet
        {
            get => GetProperty(RuleSetProperty);
            private set => LoadProperty(RuleSetProperty, value);
        }
        public static readonly PropertyInfo<byte[]> TimeStampProperty =
            RegisterProperty<byte[]>(nameof(TimeStamp));

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }

        public static readonly PropertyInfo<int> CounterProperty =
           RegisterProperty<int>(nameof(Counter));
        public int Counter
        {
            get => GetProperty(CounterProperty);
            private set => LoadProperty(CounterProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new StepIsCompletedIfValidWageIsSelected(WageIdProperty, IsCompletedProperty) {Priority=0 });
            BusinessRules.AddRule(new AddEmployeeWageStepWhenWageIsSelected(WageIdProperty) { Priority = 10 });

        }

        [CreateChild]
        private async Task CreateAsync(
           int id,
           int currentStepIndex,
         [Inject] IStepTypeDal dal,
           [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = 1;
                IsCompleted = false;
               

            }

            await BusinessRules.CheckRulesAsync();
        }

        [InsertChild]
        private void Insert(TenantOnboardingOrchestrator parent,
            [Inject] ISelectEmployeeWageStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new SelectEmployeeWageStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    EmployeeId = ((Steps)Parent.Parent.Parent).Where(r=>r is AddEmployeeEmploymentDetailsStep)
                                                        .Select(r=>(AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                                        .EmployeeEmploymentDetails.EmployeeId!,
                    WageId=this.WageId

                };
                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
                Counter = dto.Counter;
;
            }
        }

        [FetchChild]
        private async Task FetchAsync(
           string tenantId, int id, int currentStepIndex, int counter,
          [Inject] ISelectEmployeeWageStepDal dal,
           [Inject] IDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id, counter);
                Id = data.StepId;
                Name = data.Name;
                Type = (StepTypes)Enum.Parse(typeof(StepTypes), data.Type.ToString());
                StepIndex = data.StepIndex;
                Counter = data.Counter;
                TimeStamp = data.LastChanged;
                WageId = data.WageId;



            }


            await BusinessRules.CheckRulesAsync();
        }

        [UpdateChild]
        private void Update(TenantOnboardingOrchestrator parent,
            [Inject] ISelectEmployeeWageStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new SelectEmployeeWageStepDto
                {
                    TenantId = parent.TenantId,
                    Counter = this.Counter,
                    StepId = this.Id,
                    StepIndex = this.StepIndex,
                    IsCompleted = this.IsCompleted,
                    EmployeeId = ((Steps)Parent.Parent.Parent).Where(r => r is AddEmployeeEmploymentDetailsStep)
                                                        .Select(r => (AddEmployeeEmploymentDetailsStep)r).FirstOrDefault()?
                                                        .EmployeeEmploymentDetails.EmployeeId!,
                    WageId=this.WageId,
                    LastChanged = this.TimeStamp
                };
                dal.Update(dto);
                TimeStamp = dto.LastChanged;

               
            }
        }

    }
}
