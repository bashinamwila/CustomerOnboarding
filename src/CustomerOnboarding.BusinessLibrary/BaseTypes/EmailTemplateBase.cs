using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.BaseTypes
{
    [Serializable]
    public class EmailTemplateBase<T>
        :BusinessBase<T>,IEmailTemplate where T : EmailTemplateBase<T>
    {
        public static readonly PropertyInfo<string> TemplateProperty=
            RegisterProperty<string>(nameof(Template));
        public string Template
        {
            get=> GetProperty(TemplateProperty);
            protected set => LoadProperty(TemplateProperty, value);
        }

        public static readonly PropertyInfo<string> TemplateNameProperty =
           RegisterProperty<string>(nameof(TemplateName));
        public string TemplateName
        {
            get => GetProperty(TemplateNameProperty);
            protected set => LoadProperty(TemplateNameProperty, value);
        }

    }
}
