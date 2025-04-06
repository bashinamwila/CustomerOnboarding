using Csla;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary
{
    [Serializable]
    public class StatutoryRegistrations :
        BusinessBase<StatutoryRegistrations>
    {


        // TPIN Property
        public static readonly PropertyInfo<string> TPINProperty = RegisterProperty<string>(nameof(TPIN));
        public string TPIN
        {
            get => GetProperty(TPINProperty);
            set => SetProperty(TPINProperty, value);
        }

        // NAPSAAccountNumber Property
        public static readonly PropertyInfo<string> NAPSAAccountNumberProperty = RegisterProperty<string>(nameof(NAPSAAccountNumber));
        [Display(Name = "NAPSA A/C #")]
        public string NAPSAAccountNumber
        {
            get => GetProperty(NAPSAAccountNumberProperty);
            set => SetProperty(NAPSAAccountNumberProperty, value);
        }

        // NHIMAAccountNumber Property
        public static readonly PropertyInfo<string> NHIMAAccountNumberProperty = RegisterProperty<string>(nameof(NHIMAAccountNumber));
        [Display(Name = "NHIMA A/C #")]
        public string NHIMAAccountNumber
        {
            get => GetProperty(NHIMAAccountNumberProperty);
            set => SetProperty(NHIMAAccountNumberProperty, value);
        }

        public static readonly PropertyInfo<byte[]> TimeStampProperty =
            RegisterProperty<byte[]>(nameof(TimeStamp));

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public byte[] TimeStamp
        {
            get => GetProperty(TimeStampProperty);
            set => SetProperty(TimeStampProperty, value);
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            // Add validation rules
            BusinessRules.RuleSet = "Compliance Info";
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(TPINProperty) { MessageText = "TPIN is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(NAPSAAccountNumberProperty) { MessageText = "NAPSA Account # is required" });
            BusinessRules.AddRule(new BusinessLibrary.Rules.Required(NHIMAAccountNumberProperty) { MessageText = "NHIMA Account # is required" });

        }

        [CreateChild]
        private async Task CreateAsync(string ruleSet)
        {
            BusinessRules.RuleSet = ruleSet;
            await BusinessRules.CheckRulesAsync();
        }
    }
}
