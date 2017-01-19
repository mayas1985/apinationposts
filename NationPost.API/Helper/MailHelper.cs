using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace NationPost.API.Helper
{
    public static class MailHelper
    {
        public static void Send(string message, string fromEmail){
            MailMessage Msg = new MailMessage();
            // Sender e-mail address.
            Msg.From = new MailAddress(fromEmail);
            // Recipient e-mail address.
            Msg.To.Add("mayas1985@gmail.com,rahuldwivedi.rld@gmail.com");
            Msg.Subject = "Beta API NationPost";
            Msg.Body = message;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "relay-hosting.secureserver.net";
            smtp.Send(Msg);
        }
    }
}