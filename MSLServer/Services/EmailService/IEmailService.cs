using MSLServer.Models;

namespace MSLServer.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(Email request);
    }
}