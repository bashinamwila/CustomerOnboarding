using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class ConfirmEmailStep :
        StepBase<ConfirmEmailStep>
    {
        public static readonly PropertyInfo<byte[]> TimeStampProperty =
           RegisterProperty<byte[]>(nameof(TimeStamp));
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }

        [CreateChild]
        private void Create(int id, [Inject] IStepTypeDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                StepIndex = 2;
                IsCompleted = false;

            }

           

        }

        [InsertChild]
        private void Insert(CustomerOnboardingOrchestrator parent)
        { }
        [UpdateChild]
        private void Update(CustomerOnboardingOrchestrator parent)
        { }
    }
}
