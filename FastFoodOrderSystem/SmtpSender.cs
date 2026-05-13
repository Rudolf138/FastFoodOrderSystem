using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FastFoodOrderSystem
{
    public class SmtpSettings
    {
        public string? Host { get; set; }
        public int Port { get; set; } = 587;
        public string? SenderEmail { get; set; }
        public string? SenderPassword { get; set; }
    }

    public static class SmtpSender
    {
        public static async Task SendEmailAsync(SmtpSettings settings, string toEmail, string subject, string body)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(settings.SenderEmail) || string.IsNullOrWhiteSpace(settings.Host))
                throw new ArgumentException("SMTP settings incomplete");

            using var msg = new MailMessage();
            msg.From = new MailAddress(settings.SenderEmail!);
            msg.To.Add(new MailAddress(toEmail));
            msg.Subject = subject;
            msg.Body = body;

            using var client = new SmtpClient(settings.Host, settings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(settings.SenderEmail!, settings.SenderPassword ?? string.Empty),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            };

            await client.SendMailAsync(msg).ConfigureAwait(false);
        }
    }
}
