using Csla.Rules;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Rules
{
    public class FullNameRule : Csla.Rules.BusinessRule
    {

#pragma warning disable CSLA0017


        public FullNameRule(Csla.Core.IPropertyInfo
            primaryProperty, Csla.Core.IPropertyInfo affectedProperty)
        : base(primaryProperty)
        {
            InputProperties.AddRange(new[] { primaryProperty, affectedProperty });
            AffectedProperties.Add(affectedProperty);


        }
        protected override void Execute(IRuleContext context)
        {

            string firstName = "";
            string lastName = "";

            if (context.Target is IFullName target)
            {

                firstName = target.FirstName;
                lastName = target.LastName;
                LoadProperty(context.Target, AffectedProperties[1], firstName + " " + lastName);
            }
            else
            {

                if (context.Target is IFullNameInfo targetInfo)
                {
                    firstName = targetInfo.FirstName;
                    lastName = targetInfo.LastName;
                    LoadProperty(context.Target, AffectedProperties[1], firstName + " " + lastName);
                }
            }

#pragma warning restore CSLA0017  



        }
    }
}
