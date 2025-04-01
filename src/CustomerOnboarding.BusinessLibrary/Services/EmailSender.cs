using Csla;
using CustomerOnboarding.BusinessLibrary.Services.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Services
{
    [Serializable]
    public class EmailSender :IEmailSender
    {

        private readonly IDataPortalFactory portal;

        public EmailSender(IDataPortalFactory portal)=> this.portal = portal;
       

        public async Task SendEmailAsync(string email, string subject, string htmlmessage)
        {
            
            var cmd = await portal.GetPortal<EmailSenderCommand>().CreateAsync(email, subject, htmlmessage);
            cmd = await portal.GetPortal<EmailSenderCommand>().ExecuteAsync(cmd);
            
        }

        public async Task SendEmailAsync(string email, string subject, string htmlmessage, string fileName, byte[] attachment)
        {
           
            var cmd = await portal.GetPortal<EmailSenderCommand>().CreateAsync(email, subject, htmlmessage, fileName, attachment);
            cmd = await portal.GetPortal<EmailSenderCommand>().ExecuteAsync(cmd);

        }

        

        
    }
}
