using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Rules;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Types
{
    [Serializable]
    public class StepTypeTypeInfo :
        ReadOnlyBase<StepTypeTypeInfo>,ITypeInfo
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

        public static readonly PropertyInfo<string> FullNameProperty =
          RegisterProperty<string>(nameof(FullName));
        public string FullName
        {
            get { return GetProperty(FullNameProperty); }
            private set { LoadProperty(FullNameProperty, value); }
        }


        public static readonly PropertyInfo<string> FullTypeNameProperty =
           RegisterProperty<string>(nameof(FullTypeName));
        public string FullTypeName
        {
            get { return GetProperty(FullTypeNameProperty); }
            private set { LoadProperty(FullTypeNameProperty, value); }
        }

        public static readonly PropertyInfo<string> ComponentTypeNameProperty =
       RegisterProperty<string>(nameof(ComponentTypeName));
        public string ComponentTypeName
        {
            get { return GetProperty(ComponentTypeNameProperty); }
            private set { LoadProperty(ComponentTypeNameProperty, value); }
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new GetFullName
            {
                PrimaryProperty = FullNameProperty,
                Priority = 1
            });
            BusinessRules.AddRule(new GetTypeName
            {
                PrimaryProperty = NameProperty,
                Priority = 2
            });
        }

        [Fetch]
        private async Task FetchAsync(int id, [Inject] IStepTypeDal dal)
        {
            var data =dal.Fetch(id);
            Id = data.Id;
            Name = data.Name;
            Type=(StepType)Enum.Parse(typeof(StepType), data.Type.ToString());
            FullTypeName = data.FullTypeName;
            Name = data.Name;
            ComponentTypeName = data.ComponentTypeName;
            await BusinessRules.CheckRulesAsync();
        }

    }
}
