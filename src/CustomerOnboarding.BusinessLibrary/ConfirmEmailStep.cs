using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.ComponentModel;
using System.Security;

namespace CustomerOnboarding.BusinessLibrary
{
    /// <summary>
    /// Manual step where the user is expected to confirm their email.
    /// Completion of this step is handled externally (e.g., through the UI).
    /// </summary>
    [Serializable]
    public class ConfirmEmailStep : StepBase<ConfirmEmailStep>
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


        public static readonly PropertyInfo<UserConfirmation> UserProperty =
           RegisterProperty<UserConfirmation>(nameof(User));

        /// <summary>
        /// Primary user/contact for the organisation.
        /// </summary>
        public UserConfirmation User
        {
            get => GetProperty(UserProperty);
            private set => LoadProperty(UserProperty, value);
        }



        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Step is complete only if both child objects are valid
            BusinessRules.AddRule(new CheckIfStepIsComplete(UserProperty, IsCompletedProperty));

        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            base.OnChildChanged(e);

            // Trigger validation whenever children change
            if (e.ChildObject is UserConfirmation)
            {
                BusinessRules.CheckRules();
            }
        }

        #endregion

        #region DataPortal Methods

        /// <summary>
        /// Creates a new step with metadata from the database.
        /// </summary>
        [CreateChild]
        private void Create(int id,int currentStepIndex,[Inject] IStepTypeDal dal,
            [Inject]IChildDataPortal<UserConfirmation>portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(id);
                Id = data.Id;
                Name = data.Name;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                StepIndex = 2;
                if (currentStepIndex == StepIndex)
                {
                    RuleSet = data.RuleSet;
                }
                else
                {
                    RuleSet = "";
                }
                
                IsCompleted = false;
                User = portal.CreateChild(RuleSet);
            }
        }

        /// <summary>
        /// Placeholder method — no insert logic needed.
        /// </summary>
        [InsertChild]
        private void Insert(UserOnboardingOrchestrator parent, [Inject]IConfirmEmailStepDal dal)
        {
            using(BypassPropertyChecks)
            {
                var dto = new ConfirmEmailStepDto
                {
                    StepId = this.Id,
                    TenantId = parent.TenantId,
                    StepIndex = this.StepIndex,
                };
                dal.Insert(dto);
                this.TimeStamp = dto.LastChanged;
            }
        }

       
        [UpdateChild]
        private async Task UpdateAsync(UserOnboardingOrchestrator parent, [Inject]IChildDataPortal<UserConfirmation>portal)
        {
           
            if(parent.CurrentStepIndex==StepIndex)
                await portal.UpdateChildAsync(User, parent);

        }

        [FetchChild]
        private void Fetch(
            string tenantId, int id,
            int currentStepIndex,
            [Inject] IConfirmEmailStepDal dal,
            [Inject] IChildDataPortalFactory portal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Fetch(tenantId, id);
                Id = data.StepId;
                Name = data.Name;
                StepIndex= data.StepIndex;
                Type = (StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
                TimeStamp = data.LastChanged;
                if(currentStepIndex== StepIndex)
                {
                    RuleSet = data.RuleSet;
                }
                else
                {
                    RuleSet = "";
                }
                User=portal.GetPortal<UserConfirmation>().FetchChild(tenantId, RuleSet);    


            }

            BusinessRules.CheckRules();
        }

        #endregion
    }
}
