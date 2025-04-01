using Csla;
using CustomerOnboarding.BusinessLibrary.BaseTypes;
using CustomerOnboarding.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Templates
{
    [Serializable]
    public class TemplateFactory :ReadOnlyBase<TemplateFactory>
    {
        public static readonly PropertyInfo<IEmailTemplate> TemplateProperty =
            RegisterProperty<IEmailTemplate>(nameof(Result));
        public IEmailTemplate Result
        {
            get=>GetProperty(TemplateProperty);
            private set=>LoadProperty(TemplateProperty, value);
        }

        [Fetch]
        private void Fetch(int id, [Inject]ApplicationContext context,
            [Inject]IEmailTemplateDal dal)
        {
            var dto=dal.Fetch(id);
            var type=Type.GetType(dto.AssemblyQualifiedName);
            var dpType=typeof(IDataPortal<>).MakeGenericType(type!);
            var portal=(IDataPortal)context.GetRequiredService(dpType);
            Result = (IEmailTemplate)portal.Create(id);

        }
    }
}
