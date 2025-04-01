using CustomerOnboarding.Dal;
using CustomerOnboarding.Dal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.DalMock
{
    public class EmailTemplateDal : IEmailTemplateDal
    {
        public EmailTemplateDto Fetch(int id)
        {
            var result = (from r in MockDb.EmailTemplates
                          where r.Id == id
                          select new EmailTemplateDto
                          {
                              Id = r.Id,
                              TemplateName = r.TemplateName,
                              AssemblyQualifiedName = r.AssemblyQualifiedName,

                          }).FirstOrDefault();
            if (result == null)
                throw new DataNotFoundException("EmailTemplate");
            return result;
        }
    }
}
