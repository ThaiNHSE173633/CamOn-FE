using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace CamOn_FE.Services
{
    public class EmailSender : IEmailSender
     {
        public Task SendEmailAsync(string email, string subject, string confirmLink)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("norep@gmail.com"),
                Subject = subject,
                Body = confirmLink,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //Credentials = new NetworkCredential("camon229235@gmail.com", "vphzypfebzfwymdi")
                Credentials = new NetworkCredential("cookez.mail@gmail.com", "azqhastrbxuvogsp")
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
