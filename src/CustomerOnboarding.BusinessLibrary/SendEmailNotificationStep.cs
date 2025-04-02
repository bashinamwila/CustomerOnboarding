using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Services;
using CustomerOnboarding.BusinessLibrary.Services.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Templates;
using CustomerOnboarding.BusinessLibrary.Templates.Email;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    /// <summary>
    /// Represents an automatic step that simulates sending a welcome email to the user.
    /// </summary>
    [Serializable]
    public class SendEmailNotificationStep : StepBase<SendEmailNotificationStep>
    {
        #region Properties

        public static readonly PropertyInfo<byte[]> TimeStampProperty =
           RegisterProperty<byte[]>(nameof(TimeStamp));

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }

        #endregion

        #region Execution

        /// <summary>
        /// Executes the step logic — simulates sending an email and marks step as complete.
        /// </summary>
        public override async Task ExecuteAsync()
        {
            var steps = (Steps)Parent;
            var portal = ApplicationContext.GetRequiredService<IDataPortalFactory>();
            var emailSender = ApplicationContext.GetRequiredService<IEmailSender>();
            var userEmail = ((CreateAccountStep)steps[0]).User.Email;
            var userFirstName = ((CreateAccountStep)steps[0]).User.FirstName;
            var template = portal.GetPortal<TemplateFactory>().Fetch(1).Result;
            ((EmailConfirmationTemplate)template).UserFirstName=userFirstName;
            var parent = (UserOnboardingOrchestrator)Parent.Parent;
            ((EmailConfirmationTemplate)template).ConfirmationLink = $"https://localhost:7074/email-confirmed/{parent.TenantId}";
            await emailSender.SendEmailAsync(userEmail, "Welcome to our platform", template.Template);
                
            // Console.WriteLine($"📧 Email sent to {userEmail}");


           

            IsCompleted = true;

            var updater = await portal.GetPortal<SendEmailNotificationStepUpdater>().CreateAsync(parent.TenantId, this);
            updater = await portal.GetPortal<SendEmailNotificationStepUpdater>().ExecuteAsync(updater);
            TimeStamp = updater.Step.TimeStamp;
        }

        #endregion

        #region DataPortal Methods

        /// <summary>
        /// Initializes the step with metadata using the step type ID.
        /// </summary>
        [CreateChild]
        private void Create(int id,int currentStepIndex, [Inject] IStepTypeDal dal)
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

        /// <summary>
        /// Fetches step details for an existing tenant and step ID.
        /// </summary>
        [FetchChild]
        private void Fetch(string tenantId, int id,int currentStepIndex, [Inject] ISendEmailNotificationStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.Id;
                Name = data.Name;
                StepIndex = data.StepIndex;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                IsCompleted = data.IsCompleted;
                TimeStamp = data.LastChanged;
            }
        }

        /// <summary>
        /// Placeholder insert method (step is inserted via Command in ExecuteAsync).
        /// </summary>
        [InsertChild]
        private void Insert(UserOnboardingOrchestrator parent, [Inject]ISendEmailNotificationStepDal dal)
        {
            
        }

        /// <summary>
        /// Placeholder update method (step is updated via Command in ExecuteAsync).
        /// </summary>
        [UpdateChild]
        private void Update(UserOnboardingOrchestrator parent) { }

        #endregion
    }
}
