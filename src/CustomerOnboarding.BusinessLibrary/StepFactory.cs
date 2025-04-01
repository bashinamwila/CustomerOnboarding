using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Types;
using System;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    /// <summary>
    /// A read-only factory that dynamically creates or fetches IStep objects
    /// using metadata provided by StepTypeTypeInfo.
    /// </summary>
    [Serializable]
    public class StepFactory : ReadOnlyBase<StepFactory>
    {
        /// <summary>
        /// The resulting IStep instance after the factory resolves it.
        /// </summary>
        public static readonly PropertyInfo<IStep> ResultProperty =
            RegisterProperty<IStep>(nameof(Result));

        public IStep Result
        {
            get => GetProperty(ResultProperty);
            private set => LoadProperty(ResultProperty, value);
        }

        /// <summary>
        /// Creates a new step instance using the provided step type ID.
        /// Called when initializing the workflow (e.g., CreateAsync).
        /// </summary>
        [Fetch]
        private async Task FetchAsync(
            int id,
            int currentStepIndex,
            [Inject] IDataPortalFactory portal,
            [Inject] ApplicationContext appCtx)
        {
            // Fetch type metadata from the StepTypeTypeInfo object
            var info = await portal.GetPortal<StepTypeTypeInfo>().FetchAsync(id);

            // Resolve the type from its assembly-qualified name
            var type = Type.GetType(info.FullTypeName);

            // Dynamically get the IChildDataPortal<T> for that type using reflection
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);

            // Use the application context to retrieve the data portal for that type
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);

            // Create the step instance
            Result = (IStep)await dp.CreateChildAsync(id,currentStepIndex);
        }

        /// <summary>
        /// Fetches an existing step instance for a given tenant and step type ID.
        /// </summary>
        [Fetch]
        private async Task Fetch(
            string tenantId,
            int id,int currentStepIndex,
            [Inject] IDataPortalFactory portal,
            [Inject] ApplicationContext appCtx)
        {
            // Fetch type metadata
            var info = await portal.GetPortal<StepTypeTypeInfo>().FetchAsync(id);

            // Resolve the concrete type
            var type = Type.GetType(info.FullTypeName);

            // Get the data portal for that type
            var dpType = typeof(IChildDataPortal<>).MakeGenericType(type!);
            var dp = (IChildDataPortal)appCtx.GetRequiredService(dpType);

            // Fetch the child object using tenant context
            Result = (IStep)dp.FetchChild(tenantId, id,currentStepIndex);
        }
    }
}
