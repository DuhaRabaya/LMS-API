using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.EmailServices
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmail(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("duharabaya4@gmail.com", "eusl kvcp zwwk aazv")
            };

            return client.SendMailAsync(
                new MailMessage(from: "duharabaya4@gmail.com",
                                to: email,
                                subject,
                                htmlMessage
                                )
                { IsBodyHtml = true });
        }
    }
}
