using Csla;
using Csla.Core.FieldManager;
using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CheckIfStepIsComplete :BusinessRule
    {
        public CheckIfStepIsComplete(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty) :
            base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var target = (IStep)context.Target;
            var logger=context.ApplicationContext.GetRequiredService<ILogger<CheckIfStepIsComplete>>();
            // Step must be valid at the root level
            logger.LogInformation($"Checking if step {target.Name} is complete");
            var isComplete = target.IsValid;
            logger.LogInformation($"Step {target.Name} is valid: {isComplete}");

            // Find all properties on the Step that are IBusinessBase (e.g., forms, sections, sub-steps)
            var childObjects = target
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(p => typeof(IBusinessBase).IsAssignableFrom(p.PropertyType));

            foreach (var childProp in childObjects)
            {
                var child = childProp.GetValue(target);

                if (child == null)
                {
                    isComplete = false;
                    break;
                }
               

                // Now inspect each property of the child IBusinessBase object
                var childProperties = child!
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(p => p.CanWrite && p.GetIndexParameters().Length == 0 && p.Name!="TimeStamp");

                foreach (var prop in childProperties)
                {
                    var value = prop.GetValue(child);

                    if (IsDefaultValue(value, prop.PropertyType))
                    {
                        logger.LogInformation($"Property {prop.Name} of {childProp.Name} is not set");
                        isComplete = false;
                        break;
                    }
                }

                if (!isComplete)
                    break;
            }

            context.AddOutValue(AffectedProperties[1], isComplete);
        }



        private bool IsDefaultValue(object? value, Type type)
        {
            if (value == null)
                return true;

            // Special case: string
            if (type == typeof(string))
                return string.IsNullOrWhiteSpace((string)value);

            // Nullable types
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                var defaultValue = Activator.CreateInstance(underlyingType);
                return Equals(value, defaultValue);
            }

            // Regular value types (int, decimal, DateTime, etc.)
            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return Equals(value, defaultValue);
            }

            return false; // For reference types (not string), assume non-null is valid
        }


    }
}
