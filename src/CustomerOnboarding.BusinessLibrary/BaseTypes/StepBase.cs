using Csla;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    [Serializable]
    public abstract class StepBase<T> : BusinessBase<T>, IStep
        where T : StepBase<T>

    {

        public static readonly PropertyInfo<int> IdProperty =
            RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get => GetProperty(IdProperty);
            protected set => LoadProperty(IdProperty, value);
        }
        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get => GetProperty(NameProperty);
            protected set => LoadProperty(NameProperty, value);
        }

        public static readonly PropertyInfo<StepType> TypeProperty =
            RegisterProperty<StepType>(nameof(Type));
        public StepType Type
        {
            get => GetProperty(TypeProperty);
            protected set => LoadProperty(TypeProperty, value);
        }

        public static readonly PropertyInfo<int> StepIndexProperty =
            RegisterProperty<int>(nameof(StepIndex));
        public int StepIndex
        {
            get => GetProperty(StepIndexProperty);
            protected set => LoadProperty(StepIndexProperty, value);
        }

        public static readonly PropertyInfo<bool> IsCompletedProperty =
            RegisterProperty<bool>(nameof(IsCompleted));
        public bool IsCompleted
        {
            get => GetProperty(IsCompletedProperty);
            protected set => LoadProperty(IsCompletedProperty, value);
        }

        public virtual Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        
    }
}
