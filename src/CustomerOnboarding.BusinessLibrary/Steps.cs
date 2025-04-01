using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal;
using System;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    /// <summary>
    /// Represents a list of onboarding steps (manual or automatic) in the customer onboarding workflow.
    /// Each item implements the IStep interface.
    /// </summary>
    [Serializable]
    public class Steps : BusinessListBase<Steps, IStep>
    {
        #region DataPortal Methods

        /// <summary>
        /// Creates a new (empty) list of onboarding steps.
        /// This is typically used when initializing a new orchestrator.
        /// </summary>
        [CreateChild]
        private void Create()
        {
            // Intentionally empty: steps are added externally after creation.
        }

        /// <summary>
        /// Fetches and builds a list of steps for an existing tenant.
        /// This uses StepFactory to instantiate the correct step type per ID.
        /// </summary>
        /// <param name="tenantId">The tenant's unique ID</param>
        /// <param name="portal">Factory for creating step instances</param>
        /// <param name="dal">Step data access layer</param>
        [FetchChild]
        private async Task FetchAsync(
            string tenantId,int currentStepIndex,
            [Inject] IDataPortal<StepFactory> portal,
            [Inject] IStepDal dal)
        {
            var raiseEvents = this.RaiseListChangedEvents;
            this.RaiseListChangedEvents = false;

            // Get metadata from database
            var stepMetadataList = dal.Fetch(tenantId);

            // Resolve and instantiate each step
            foreach (var stepMeta in stepMetadataList)
            {
                var factory = await portal.FetchAsync(stepMeta.TenantId, stepMeta.Id,currentStepIndex);

                this.Add(factory.Result);
            }

            this.RaiseListChangedEvents = raiseEvents;
        }

        #endregion
    }
}
