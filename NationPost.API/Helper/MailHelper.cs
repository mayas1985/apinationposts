using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace NationPost.API.Helper
{
    public static class MailHelper
    {
        public static void Send(string message, string subject, string fromEmail, string toEmail){
            MailMessage Msg = new MailMessage();
            // Sender e-mail address.
            Msg.From = new MailAddress(fromEmail);
            // Recipient e-mail address.
            Msg.To.Add(toEmail);
            Msg.Subject = subject;
            Msg.Body = message;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "relay-hosting.secureserver.net";
            smtp.Send(Msg);
        }
    }
}