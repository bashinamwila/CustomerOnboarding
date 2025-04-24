using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace CustomerOnboarding.BusinessLibrary.Attributes
    {
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class DefaultValueAllowedAttribute : Attribute
        {
            public string? Reason { get; }

            public DefaultValueAllowedAttribute(string? reason = null)
            {
                Reason = reason;
            }
        }
    }


