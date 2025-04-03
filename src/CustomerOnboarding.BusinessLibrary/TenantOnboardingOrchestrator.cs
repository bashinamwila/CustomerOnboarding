using Csla;
using Csla.Core;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Multitenancy;
using CustomerOnboarding.BusinessLibrary.Rules;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class TenantOnboardingOrchestrator :
        BusinessBase<TenantOnboardingOrchestrator>,IOnboardingOrchestrator
    {
        public static readonly PropertyInfo<string> TenantIdProperty =
            RegisterProperty<string>(nameof(TenantId));

        /// <summary>
        /// Unique identifier for the onboarding session (typically a GUID).
        /// </summary>
        public string TenantId
        {
            get => GetProperty(TenantIdProperty);
            private set => LoadProperty(TenantIdProperty, value);
        }

        public static readonly PropertyInfo<bool> IsCompleteProperty =
            RegisterProperty<bool>(nameof(IsComplete));

        /// <summary>
        /// Indicates whether all steps in the workflow have been completed.
        /// This is managed by the CheckIfWorkflowIsComplete business rule.
        /// </summary>
        public bool IsComplete
        {
            get => GetProperty(IsCompleteProperty);
            private set => SetProperty(IsCompleteProperty, value); // Rule sets this
        }

        public static readonly PropertyInfo<Steps> StepsProperty =
            RegisterProperty<Steps>(nameof(Steps));

        /// <summary>
        /// A collection of steps representing the onboarding process.
        /// </summary>
        public Steps Steps
        {
            get => GetProperty(StepsProperty);
            private set => LoadProperty(StepsProperty, value);
        }

        public static readonly PropertyInfo<int> CurrentStepIndexProperty =
            RegisterProperty<int>(nameof(CurrentStepIndex));

        /// <summary>
        /// Index of the currently active step in the onboarding workflow.
        /// </summary>
        public int CurrentStepIndex
        {
            get => GetProperty(CurrentStepIndexProperty);
            private set => SetProperty(CurrentStepIndexProperty, value);
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty =
            RegisterProperty<byte[]>(nameof(TimeStamp));

        /// <summary>
        /// Timestamp for concurrency control during data persistence.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }

        public Task MoveNextAsync()
        {
            throw new NotImplementedException();
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();

            // Rule: The workflow is complete (IsComplete = true) only if all steps in the Steps collection are complete.
            BusinessRules.AddRule(new CheckIfWorkflowIsComplete(StepsProperty, IsCompleteProperty));

            // Dependency: Ensure the IsComplete property is re-evaluated whenever the Steps collection or its items change.
            BusinessRules.AddRule(new Csla.Rules.CommonRules.Dependency(StepsProperty, IsCompleteProperty));
        }

        protected override void OnChildChanged(ChildChangedEventArgs e)
        {
            base.OnChildChanged(e);

            // If any step within the Steps collection changes, or if the Steps collection itself changes
            // (e.g., items added/removed, though not typical after creation here),
            // re-evaluate rules associated with the StepsProperty (like CheckIfWorkflowIsComplete).
            if (e.ChildObject is IStep || e.ChildObject is Steps)
            {
                BusinessRules.CheckRules(StepsProperty); // Check rules associated with StepsProperty
            }
        }


        [Create]
        private async Task CreateAsync([Inject]TenantInfo tenant,
            [Inject]IChildDataPortalFactory portal,
            [Inject]ILogger<TenantOnboardingOrchestrator>logger)
        {
            using (BypassPropertyChecks)
            {
                IsComplete = false;
                CurrentStepIndex = 0;
                Steps = await portal.GetPortal<Steps>().CreateChildAsync();
                logger.LogInformation($"Number of steps is {Steps.Count}");
                logger.LogInformation($"The tenant id is {tenant.Id}");
                var organisationProfileStep = await portal.GetPortal<OrganisationProfileStep>().CreateChildAsync(tenant.Id,4,CurrentStepIndex);
                logger.LogInformation($"Number of steps is {Steps.Count}");
                Steps.AddRange(new IStep [] {organisationProfileStep });
            }
            await BusinessRules.CheckRulesAsync();
        }
    }
}
