using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using Csla;

namespace CustomerOnboarding.BusinessLibrary.Services
{
    [Serializable]
    public class EmailSenderCommand :
        CommandBase<EmailSenderCommand>
    {
        public static readonly PropertyInfo<string> EmailProperty =
            RegisterProperty<string>(nameof(Email));
        public string Email
        {
            get => ReadProperty(EmailProperty);
            private set => LoadProperty(EmailProperty, value);
        }

        public static readonly PropertyInfo<string> SubjectProperty =
            RegisterProperty<string>(nameof(Subject));
        public string Subject
        {
            get => ReadProperty(SubjectProperty);
            private set => LoadProperty(SubjectProperty, value);
        }

        public static readonly PropertyInfo<string> HtmlMessageProperty =
            RegisterProperty<string>(nameof(HtmlMessage));
        public string HtmlMessage
        {
            get => ReadProperty(HtmlMessageProperty);
            private set => LoadProperty(HtmlMessageProperty, value);
        }

        public static readonly PropertyInfo<string> FileNameProperty =
           RegisterProperty<string>(nameof(FileName));
        public string FileName
        {
            get => ReadProperty(FileNameProperty);
            private set => LoadProperty(FileNameProperty, value);
        }

        public static readonly PropertyInfo<byte[]> AttachmentProperty =
            RegisterProperty<byte[]>(nameof(Attachment));
        public byte[] Attachment
        {
            get => ReadProperty(AttachmentProperty);
            private set => LoadProperty(AttachmentProperty, value);
        }

        [Create]
        private void Create(string email, string subject, string htmlMessage)
        {
            this.Email = email;
            this.Subject = subject;
            this.HtmlMessage = htmlMessage;
        }

        [Create]
        private void Create(string email, string subject, string htmlMessage, string fileName,
            byte[] attachment)
        {
            this.Email = email;
            this.Subject = subject;
            this.HtmlMessage = htmlMessage;
            this.FileName = fileName;
            this.Attachment = attachment;
        }

        [Execute]
        private async void Execute()
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync("localhost", 25, MailKit.Security.SecureSocketOptions.None);
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Test", "test@example.com"));
                message.To.Add(new MailboxAddress(Email, Email));
                message.Subject = Subject;
                var builder = new BodyBuilder();

                builder.HtmlBody = HtmlMessage;
                if (Attachment != null)
                {
                    builder.Attachments.Add(FileName, Attachment);
                }
                message.Body = builder.ToMessageBody();

                await client.SendAsync(message);
            }
        }
    }

}

