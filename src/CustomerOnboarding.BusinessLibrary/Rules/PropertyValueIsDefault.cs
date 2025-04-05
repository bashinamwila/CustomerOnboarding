using Csla.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class PropertyValueIsDefault :BusinessRule
    {
        public PropertyValueIsDefault(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties.Add(primaryProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            
            if (context.InputPropertyValues.TryGetValue(PrimaryProperty, out var value) && IsDefault(value))
            {
                context.AddWarningResult($"{PrimaryProperty.FriendlyName} is set to it's default value.");
            }
        }

        private bool IsDefault<T>(T value)=> EqualityComparer<T>.Default.Equals(value, default(T));
    }
}
