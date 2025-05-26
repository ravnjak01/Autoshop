namespace RS1_2024_25.API.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string toEmail, string resetLink);
    }
}
