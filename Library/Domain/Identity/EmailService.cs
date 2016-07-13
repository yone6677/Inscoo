using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Domain
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var credentialUserName = "webtestemail@sina.com";
            var sentFrom = "webtestemail@sina.com";
            var pwd = "web123";

            var client =
                new SmtpClient("smtp.sina.com", 25);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);
            client.Credentials = credentials;
            client.EnableSsl = false;


            var mail =
                new MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;
            return client.SendMailAsync(mail);
        }
        public void Send(IdentityMessage message)
        {
            var credentialUserName = "webtestemail@sina.com";
            var sentFrom = "webtestemail@sina.com";
            var pwd = "web123";

            var client =
                new SmtpClient("smtp.sina.com", 25);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);
            client.Credentials = credentials;
            client.EnableSsl = false;


            var mail =
                new MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;
            client.Send(mail);
        }
    }
}
