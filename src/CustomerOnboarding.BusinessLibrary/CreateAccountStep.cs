using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    /// <summary>
    /// First manual onboarding step where organisation and user information is collected.
    /// </summary>
    [Serializable]
    public class CreateAccountStep : StepBase<CreateAccountStep>
    {
        #region Properties

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

        public static readonly PropertyInfo<Organisation> OrganisationProperty =
           RegisterProperty<Organisation>(nameof(Organisation));

        /// <summary>
        /// Organisation details provided by the customer.
        /// </summary>
        public Organisation Organisation
        {
            get => GetProperty(OrganisationProperty);
            private set => LoadProperty(OrganisationProperty, value);
        }

        public static readonly PropertyInfo<User> UserProperty =
            RegisterProperty<User>(nameof(User));

        /// <summary>
        /// Primary user/contact for the organisation.
        /// </summary>
        public User User
        {
            get => GetProperty(UserProperty);
            private set => LoadProperty(UserProperty, value);
        }

        #endregion

        #region Business Rules

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(OrganisationProperty, IsCompletedProperty));
            BusinessRules.AddRule(new CheckIfStepIsComplete(UserProperty, IsCompletedProperty));
        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            base.OnChildChanged(e);

            // Trigger validation whenever children change
            if (e.ChildObject is Organisation || e.ChildObject is User)
            {
                BusinessRules.CheckRules();
            }
        }

        #endregion

        #region DataPortal Methods

        /// <summary>
        /// Creates a new CreateAccountStep instance with child objects.
        /// </summary>
        [CreateChild]
        private async Task CreateAsync(
            string tenantId, int id,
            int currentStepIndex,
          [Inject] IStepTypeDal dal,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
               Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
               StepIndex = 0;
               if(currentStepIndex==StepIndex)
               {
                    RuleSet = data.RuleSet;
               }
               else
               {
                    RuleSet = "";
               }


                    IsCompleted = false;

                // Create child objects
                Organisation = await portal.GetPortal<Organisation>().CreateChildAsync(tenantId, RuleSet);
                User = await portal.GetPortal<User>().CreateChildAsync(RuleSet);

            }

            BusinessRules.CheckRules();
        }

        /// <summary>
        /// Fetches an existing CreateAccountStep and its children.
        /// </summary>
        [FetchChild]
        private async Task FetchAsync(
            string tenantId, int id,
            int currentStepIndex,
           [Inject] ICreateAccountStepDal dal,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                StepIndex = data.StepIndex;
                if (currentStepIndex == StepIndex)
                {
                    RuleSet = data.RuleSet;
                }
                else
                {
                    RuleSet = "";
                }

                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                TimeStamp = data.LastChanged;

                Organisation = await portal.GetPortal<Organisation>().FetchChildAsync(tenantId, RuleSet);
                User = await portal.GetPortal<User>().FetchChildAsync(tenantId,RuleSet);

        }

            BusinessRules.CheckRules();
        }

        /// <summary>
        /// Persists this step and its children when first added.
        /// </summary>
        [InsertChild]
        private async Task InsertAsync(
            UserOnboardingOrchestrator parent,
            [Inject] ICreateAccountStepDal dal,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new CreateAccountStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    StepIndex=this.StepIndex
                };

                dal.Insert(dto);
                TimeStamp = dto.LastChanged;
               
                await portal.GetPortal<Organisation>().UpdateChildAsync(Organisation, parent);
                await portal.GetPortal<User>().UpdateChildAsync(User, parent);
                
                
            }
        }

        /// <summary>
        /// Updates this step metadata (excluding children).
        /// </summary>
        [UpdateChild]
        private void Update(
            UserOnboardingOrchestrator parent,
            [Inject] ICreateAccountStepDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = new CreateAccountStepDto
                {
                    TenantId = parent.TenantId,
                    StepId = this.Id,
                    LastChanged = this.TimeStamp
                };

                dal.Update(dto);
                TimeStamp = dto.LastChanged;
            }
        }

        #endregion
    }
}
