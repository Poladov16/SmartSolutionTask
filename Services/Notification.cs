using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SmartSTask.Services
{
    public static class Notification
    {
        public static async Task SendMail(string to, string title, string description)
        {
            try
            {
                // MailMessage mail = new MailMessage();
                // SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                // NetworkCredential NetworkCred = new NetworkCredential("poladov16@mail.ru", "dtjz31x40b293");
                // mail.From = new MailAddress("poladov16@mail.ru");
                // mail.To.Add(to);
                // mail.Subject = "You have new Task";
                // mail.Body = description;

                // SmtpServer.Port = 587;
                // SmtpServer.UseDefaultCredentials = false;
                // SmtpServer.Credentials = CredentialCache.DefaultNetworkCredentials;// NetworkCred;
                // SmtpServer.EnableSsl = true;

                // SmtpServer.Send(mail);
                // return true;
                var message = new MailMessage();
                message.To.Add(to);

                message.Subject = title; 
                message.Body = description;

                using (var smtpClient = new SmtpClient())
                {
                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                //return false;
            }
        }
       
    }
    public enum Status
    {
        Done = 1,    // 
        InProgress = 2,   // 
        Cancel = 3,         // 
    }
}
