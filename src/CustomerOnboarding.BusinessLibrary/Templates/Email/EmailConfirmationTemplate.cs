using CustomerOnboarding.BusinessLibrary.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csla;
using Csla.Rules;
using Microsoft.AspNetCore.Hosting;
using CustomerOnboarding.BusinessLibrary.Services;
using CustomerOnboarding.Dal;

namespace CustomerOnboarding.BusinessLibrary.Templates.Email
{
    [Serializable]
    public class EmailConfirmationTemplate :
        EmailTemplateBase<EmailConfirmationTemplate>
    {
        public static readonly PropertyInfo<string>UserFirstNameProperty=
            RegisterProperty<string>(nameof(UserFirstName));
        public string UserFirstName
        {
            get => GetProperty(UserFirstNameProperty);
            set => SetProperty(UserFirstNameProperty, value);
        }

        public static readonly PropertyInfo<string> ConfirmationLinkProperty =
            RegisterProperty<string>(nameof(ConfirmationLink));
        public string ConfirmationLink
        {
            get => GetProperty(ConfirmationLinkProperty);
            set => SetProperty(ConfirmationLinkProperty, value);
        }

        private class GetTemplate : BusinessRule
        {
            public Csla.Core.IPropertyInfo SecondaryProperty { get; private set; }
            public GetTemplate(Csla.Core.IPropertyInfo primaryProperty,
                Csla.Core.IPropertyInfo secondaryProperty,
                Csla.Core.IPropertyInfo affectedProperty) : base(primaryProperty)
            {
                InputProperties.AddRange(new[] {primaryProperty,secondaryProperty, affectedProperty});
                SecondaryProperty = secondaryProperty;
                AffectedProperties.Add(affectedProperty);
            }
            override protected void Execute(IRuleContext context)
            {
                var env = context.ApplicationContext.GetRequiredService<IWebHostEnvironment>();
                var prop1 = context.GetInputValue<string>(PrimaryProperty);
                var prop2 = context.GetInputValue<string>(SecondaryProperty);
                var target = (IEmailTemplate)context.Target;
                if(!string.IsNullOrEmpty(prop1) && 
                    !string.IsNullOrEmpty(prop2))
                {
                    var userFirstName=PrimaryProperty.Name=="UserFirstName"?prop1:prop2;
                    var confirmationLink=SecondaryProperty.Name=="ConfirmationLink"?prop2:prop1;

                    var templatePath = Path.Combine(env.WebRootPath, "templates", $"{target.TemplateName}.html");
                    var template = File.ReadAllText(templatePath);
                    template = template.Replace("{{UserFirstName}}", userFirstName);
                    template = template.Replace("{{ConfirmationLink}}", confirmationLink);
                    template = template.Replace("{{CurrentYear}}", DateTime.UtcNow.Year.ToString());
                    context.AddOutValue(AffectedProperties[1],template);
                }

            }
        }

        protected override void AddBusinessRules()
        {
            base.AddBusinessRules();
            BusinessRules.AddRule(new GetTemplate(UserFirstNameProperty, ConfirmationLinkProperty, TemplateProperty));
            BusinessRules.AddRule(new GetTemplate(ConfirmationLinkProperty,UserFirstNameProperty, TemplateProperty));
        }

        [Create]
        private void Create(int id, [Inject]IEmailTemplateDal dal)
        {
            using (BypassPropertyChecks)
            {
                var dto = dal.Fetch(id);
                TemplateName=dto.TemplateName;
            }
        }

    }
}
