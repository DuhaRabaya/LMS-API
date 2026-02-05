using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.EmailServices
{
    public interface IEmailSender 
    {
        Task SendEmail(string email, string subject, string htmlMessage);
    }
}
