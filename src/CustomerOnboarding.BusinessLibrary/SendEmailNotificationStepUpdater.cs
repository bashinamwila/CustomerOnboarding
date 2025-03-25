using Csla;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class SendEmailNotificationStepUpdater :
        CommandBase<SendEmailNotificationStepUpdater>
    {
        public static readonly PropertyInfo<string> TenantIdProperty =
            RegisterProperty<string>(nameof(TenantId));
        public string TenantId
        {
            get => ReadProperty(TenantIdProperty);
            private set=>LoadProperty(TenantIdProperty, value);
        }

        public static readonly PropertyInfo<SendEmailNotificationStep>StepProperty=
            RegisterProperty<SendEmailNotificationStep>(nameof(Step));
        public SendEmailNotificationStep Step
        {
            get => ReadProperty(StepProperty);
            private set => LoadProperty(StepProperty, value);
        }

        [Create]
        private void Create(string tenantId, SendEmailNotificationStep step)
        {
            TenantId = tenantId;
            Step = step;
            
        }

        [Execute]
        private void Execute([Inject]ISendEmailNotificationStepDal dal)
        {
            var data = new SendEmailNotificationStepDto
            {
                TenantId = TenantId,
                Id = Step.Id,
                IsCompleted = Step.IsCompleted
            };
             dal.Insert(data);
        }
       
    }
}
