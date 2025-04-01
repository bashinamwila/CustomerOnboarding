using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.BusinessLibrary.Services.BaseTypes
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlmessage);
        public Task SendEmailAsync(string email, string subject, string htmlmessage,string fileName,byte[] attachment);
    }
}
