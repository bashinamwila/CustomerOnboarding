using Csla;
using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.BusinessLibrary.Attributes;
using System;
using System.Linq;
using System.Reflection;


namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class CheckIfStepIsComplete : BusinessRule
    {
        public CheckIfStepIsComplete(Csla.Core.IPropertyInfo primaryProperty,
            Csla.Core.IPropertyInfo affectedProperty)
            : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);
        }

        protected override void Execute(IRuleContext context)
        {
            var target = (IStep)context.Target;
            var isComplete = target.IsValid;

            var childObjects = target.GetType()
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

                if (HasDefaultValueInBusinessBase((IBusinessBase)child))
                {
                    isComplete = false;
                    break;
                }
            }

            context.AddOutValue(AffectedProperties[1], isComplete);
        }

        /// <summary>
        /// Recursively checks if any property is default, unless it's explicitly allowed to be.
        /// </summary>
        private bool HasDefaultValueInBusinessBase(IBusinessBase businessObject)
        {
            var properties = businessObject.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(p => p.CanWrite && p.GetIndexParameters().Length == 0);

            foreach (var prop in properties)
            {
                // Skip known exclusions
                if (prop.Name == "TimeStamp" || prop.Name.StartsWith("Old", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Skip properties explicitly marked as allowed to be default
                if (prop.GetCustomAttribute<DefaultValueAllowedAttribute>() != null)
                    continue;

                var value = prop.GetValue(businessObject);

                if (typeof(IBusinessBase).IsAssignableFrom(prop.PropertyType))
                {
                    if (value is IBusinessBase nestedChild)
                    {
                        if (HasDefaultValueInBusinessBase(nestedChild))
                            return true;
                    }
                }
                else
                {
                    if (IsDefaultValue(value, prop.PropertyType))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if the given value is considered default (except bool).
        /// </summary>
        private bool IsDefaultValue(object? value, Type type)
        {
            if (value == null)
                return true;

            if (type == typeof(string))
                return string.IsNullOrWhiteSpace((string)value);

            if (type == typeof(bool) || Nullable.GetUnderlyingType(type) == typeof(bool))
                return false;

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                var defaultValue = Activator.CreateInstance(underlyingType);
                return Equals(value, defaultValue);
            }

            if (type.IsValueType)
            {
                var defaultValue = Activator.CreateInstance(type);
                return Equals(value, defaultValue);
            }

            return false; // reference types are valid if non-null
        }
    }
}
