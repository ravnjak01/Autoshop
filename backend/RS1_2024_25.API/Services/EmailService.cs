using Microsoft.Extensions.Options;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Services;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }


    public async Task SendResetPasswordEmail(string toEmail, string resetLink, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.From),
                Subject = "Reset lozinke",
                Body = $"Kliknite na link za reset lozinke: {resetLink}",
                IsBodyHtml = false
            };
            mail.To.Add(toEmail);

            using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true
            };

            cancellationToken.ThrowIfCancellationRequested();

            // NAPOMENA: SmtpClient.SendMailAsync ne podržava Token. 
            await smtp.SendMailAsync(mail);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Slanje emaila je otkazano.");
            throw; 
        }
        catch (Exception ex)
        {
            Console.WriteLine("Email slanje nije uspjelo: " + ex.Message);
            throw new InvalidOperationException("Greška pri slanju emaila", ex);
        }
    }

}
