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
    public class SendEmailNotificationStep :
        StepBase<SendEmailNotificationStep>
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

        public override async Task ExecuteAsync()
        {
            var steps = (Steps)Parent;
            Console.WriteLine($"Email sent to {((CreateAccountStep)steps[0]).WorkEmail}");
            var portal = ApplicationContext.GetRequiredService<IDataPortal<SendEmailNotificationStepUpdater>>();
            var parent = (CustomerOnboardingOrchestrator)Parent.Parent;
            IsCompleted = true;
            var updater = await portal.CreateAsync(parent.TenantId,this);
            updater = await portal.ExecuteAsync(updater);
            
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
                StepIndex = 1;
                IsCompleted = false;

            }
        }

        [InsertChild]
        private void Insert(CustomerOnboardingOrchestrator parent) { }
        [UpdateChild]
        private void Update(CustomerOnboardingOrchestrator parent) { }

        [FetchChild]
        private void Fetch(string tenantId,int id, [Inject] ISendEmailNotificationStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.Id;
                StepIndex = data.StepIndex;
                Name = data.Name;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                IsCompleted = data.IsCompleted;
                TimeStamp = data.LastChanged;
            }
        }

    }
}
