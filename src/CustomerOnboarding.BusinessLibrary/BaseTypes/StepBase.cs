using Csla;
using CustomerOnboarding.Dal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    /// <summary>
    /// Abstract base class for all onboarding steps.
    /// Provides shared properties and implements the IStep interface.
    /// </summary>
    /// <typeparam name="T">The concrete step type</typeparam>
    [Serializable]
    public abstract class StepBase<T> : BusinessBase<T>, IStep
        where T : StepBase<T>
    {
        #region Common Step Properties

        public static readonly PropertyInfo<int> IdProperty =
            RegisterProperty<int>(nameof(Id));

        /// <summary>
        /// Step identifier (matches entry in the StepType table).
        /// </summary>
        public int Id
        {
            get => GetProperty(IdProperty);
            protected set => LoadProperty(IdProperty, value);
        }

        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(nameof(Name));

        /// <summary>
        /// Friendly name of the step (e.g., "Create Account").
        /// </summary>
        public string Name
        {
            get => GetProperty(NameProperty);
            protected set => LoadProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<StepType> TypeProperty =
            RegisterProperty<StepType>(nameof(Type));

        /// <summary>
        /// Indicates whether the step is Manual or Automatic.
        /// </summary>
        public StepType Type
        {
            get => GetProperty(TypeProperty);
            protected set => LoadProperty(TypeProperty, value);
        }

        public static readonly PropertyInfo<int> StepIndexProperty =
            RegisterProperty<int>(nameof(StepIndex));

        /// <summary>
        /// Indicates the position/order of the step in the onboarding workflow.
        /// </summary>
        public int StepIndex
        {
            get => GetProperty(StepIndexProperty);
            protected set => LoadProperty(StepIndexProperty, value);
        }

        public static readonly PropertyInfo<bool> IsCompletedProperty =
            RegisterProperty<bool>(nameof(IsCompleted));

        /// <summary>
        /// Indicates whether this step has been completed.
        /// </summary>
        public bool IsCompleted
        {
            get => GetProperty(IsCompletedProperty);
            protected set => LoadProperty(IsCompletedProperty, value);
        }

        #endregion

        #region Execution

        /// <summary>
        /// Executes the logic of the step. Must be overridden by automatic steps.
        /// Manual steps typically do not override this.
        /// </summary>
        public virtual Task ExecuteAsync()
        {
            throw new NotImplementedException("Only automatic steps should override ExecuteAsync().");
        }

        #endregion

        #region Rules

        /// <summary>
        /// Hook for child classes to add business rules.
        /// </summary>
        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
        }

        #endregion
    }
}
