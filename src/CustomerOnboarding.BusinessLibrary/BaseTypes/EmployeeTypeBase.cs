using Csla.Core;
using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    [Serializable]
    public abstract class EmployeeTypeBase<T> : BusinessBase<T>, IEmployeeType, ITerm
        where T : BusinessBase<T>
    {
        public static readonly PropertyInfo<int> IdProperty =
           RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            protected set { LoadProperty(IdProperty, value); }
        }

        public static readonly PropertyInfo<DateSplitter> ContractStartDateProperty =
            RegisterProperty<DateSplitter>(nameof(ContractStartDate));
        [Display(Name = "Contract Start Date")]
        [Required(ErrorMessage = "Contract start date is required")]
        public DateSplitter ContractStartDate
        {
            get { return GetProperty(ContractStartDateProperty); }
            set { SetProperty(ContractStartDateProperty, value); }
        }

        public static readonly PropertyInfo<DateTime> ContractExpiryDateProperty =
            RegisterProperty<DateTime>(nameof(ContractExpiryDate));
        [Display(Name = "Contract Expiry Date")]
        public DateTime ContractExpiryDate
        {
            get { return GetProperty(ContractExpiryDateProperty); }
            protected set { LoadProperty(ContractExpiryDateProperty, value); }
        }

        public static readonly PropertyInfo<int?> ContractDurationProperty =
           RegisterProperty<int?>(nameof(ContractDuration));
        [Required(ErrorMessage = "Contract duration is required")]
        [Display(Name = "Contract Duration")]
        public int? ContractDuration
        {
            get { return GetProperty(ContractDurationProperty); }
            set { SetProperty(ContractDurationProperty, value); }
        }

        public static readonly PropertyInfo<int?> IntervalProperty =
          RegisterProperty<int?>(nameof(Interval));
        [Required(ErrorMessage = "Contract interval is required")]
        public int? Interval
        {
            get { return GetProperty(IntervalProperty); }
            set { SetProperty(IntervalProperty, value); }
        }




        public static readonly PropertyInfo<byte[]> TimeStampProperty =
            RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get { return GetProperty(TimeStampProperty); }
            set { SetProperty(TimeStampProperty, value); }
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(
                 new BusinessLibrary.Rules.CalculateContractExpiryDate(ContractStartDateProperty, ContractExpiryDateProperty));
            BusinessRules.AddRule(
               new BusinessLibrary.Rules.CalculateContractExpiryDate(ContractDurationProperty, ContractExpiryDateProperty));
            BusinessRules.AddRule(
              new BusinessLibrary.Rules.CalculateContractExpiryDate(IntervalProperty, ContractExpiryDateProperty));

            BusinessRules.AddRule(new BusinessLibrary.Rules.DateRequired(ContractStartDateProperty));


        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            if (e.ChildObject is DateSplitter)
                BusinessRules.CheckRules(ContractStartDateProperty);
            base.OnChildChanged(e);
        }

        [CreateChild]
        protected void Create(int id,
            [Inject] IChildDataPortal<DateSplitter> portal)
        {
            using (BypassPropertyChecks)
            {
                this.Id = id;
                this.ContractStartDate = portal.CreateChild();


            }
            BusinessRules.CheckRules();
        }
    }
}
