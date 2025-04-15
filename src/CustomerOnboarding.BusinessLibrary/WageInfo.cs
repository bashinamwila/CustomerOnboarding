using Csla.Rules;
using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal.Dtos;
using CustomerOnboarding.Dal;
using CustomerOnboarding.BusinessLibrary.BaseTypes;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class WageInfo : ReadOnlyBase<WageInfo>, IEarningInfo
    {
        public static readonly PropertyInfo<string> IdProperty =
            RegisterProperty<string>(nameof(Id));
        public string Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }
        public static readonly PropertyInfo<string> NameProperty =
            RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get { return GetProperty(NameProperty); }
            private set { LoadProperty(NameProperty, value); }
        }

        public static readonly PropertyInfo<bool> IsSystemDefinedProperty = RegisterProperty<bool>(nameof(IsSystemDefined));

        public bool IsSystemDefined
        {
            get { return GetProperty(IsSystemDefinedProperty); }
            private set { LoadProperty(IsSystemDefinedProperty, value); }
        }

        public static readonly PropertyInfo<int> TypeProperty = RegisterProperty<int>(nameof(Type));
        public int Type
        {
            get { return GetProperty(TypeProperty); }
            private set { LoadProperty(TypeProperty, value); }
        }

        public static readonly PropertyInfo<string> TypeNameProperty =
            RegisterProperty<string>(nameof(TypeName));
        public string TypeName
        {

            get => GetProperty(TypeNameProperty);
            private set => LoadProperty(TypeNameProperty, value);
        }

        public static readonly PropertyInfo<string> FormularProperty = RegisterProperty<string>(nameof(Formular));
        public string Formular
        {
            get { return GetProperty(FormularProperty); }
            private set { LoadProperty(FormularProperty, value); }
        }


        /*

        public static readonly PropertyInfo<VariableList> VariablesProperty =
           RegisterProperty<VariableList>(nameof(Variables));
        public VariableList Variables
        {
            get => GetProperty(VariablesProperty);
            private set => LoadProperty(VariablesProperty, value);
        }

        */

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            // BusinessRules.AddRule(new GetTypeName(TypeProperty, TypeNameProperty));
        }



        [FetchChild]
        private void Fetch(WageDto data)
        {
            Id = data.Id;
            Name = data.Name;
            Type = data.Type;
            Formular = data.Formular;
            IsSystemDefined = data.IsSystemDefined;
            BusinessRules.CheckRules();
        }

        /*
        [Fetch]
        private void Fetch(string id, [Inject] IWageDal dal,
            [Inject] IChildDataPortal<VariableList> portal)
        {
            var data = dal.Fetch(id);

            Id = data.Id;
            Name = data.Name;
            Type = data.Type;
            Formular = data.Formular;
            IsSystemDefined = data.IsSystemDefined;
           // Variables = portal.FetchChild(id);
            // WageType = portal.Fetch(Type, Id).Result;

        }

        */
    }
}
