using Csla.Rules;
using Csla;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerOnboarding.Dal;

namespace CustomerOnboarding.BusinessLibrary.Types
{
    [Serializable]
    public class StatusTypeTypeInfo : ReadOnlyBase<StatusTypeTypeInfo>
    {
        public static readonly PropertyInfo<int> IdProperty =
         RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get { return GetProperty(IdProperty); }
            private set { LoadProperty(IdProperty, value); }
        }


        public static readonly PropertyInfo<string> FullTypeNameProperty =
           RegisterProperty<string>(nameof(FullTypeName));
        public string FullTypeName
        {
            get { return GetProperty(FullTypeNameProperty); }
            private set { LoadProperty(FullTypeNameProperty, value); }
        }

        public static readonly PropertyInfo<string> FullNameProperty =
          RegisterProperty<string>(nameof(FullName));
        public string FullName
        {
            get { return GetProperty(FullNameProperty); }
            private set { LoadProperty(FullNameProperty, value); }
        }

        public static readonly PropertyInfo<string> NameProperty =
         RegisterProperty<string>(nameof(Name));
        public string Name
        {
            get { return GetProperty(NameProperty); }
            private set { LoadProperty(NameProperty, value); }
        }



        private class GetName : Csla.Rules.BusinessRule
        {
            protected override void Execute(IRuleContext context)
            {
                var target = (StatusTypeTypeInfo)context.Target;
                var fullName = target.FullName;
                var lastIndexOf = fullName.LastIndexOf(".");
                var name = fullName.Substring(lastIndexOf + 1);
                context.AddOutValue(PrimaryProperty, name);
            }
        }

        private class GetFullName : Csla.Rules.BusinessRule
        {
            protected override void Execute(IRuleContext context)
            {
                var target = (StatusTypeTypeInfo)context.Target;
                var assemblyQualifiedName = target.FullTypeName;
                var indexOfComma = assemblyQualifiedName.IndexOf(",");
                var fullName = assemblyQualifiedName?.Substring(0, indexOfComma);
                context.AddOutValue(PrimaryProperty, fullName);
            }
        }
        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new GetFullName
            {
                PrimaryProperty = FullNameProperty,
                Priority = 1
            });
            BusinessRules.AddRule(new GetName
            {
                PrimaryProperty = NameProperty,
                Priority = 2
            });
        }


        [Fetch]
        private void Fetch(int id, [Inject] IStatusTypeDal dal)
        {
            var data = dal.Fetch(id);
            Id = id;
            FullTypeName = data.TypeName;
            BusinessRules.CheckRules();
        }
        [FetchChild]
        private void Fetch(TypeDto data)
        {
            Id = data.Id;
            FullTypeName = data.TypeName;
            BusinessRules.CheckRules();
        }
    }
}
