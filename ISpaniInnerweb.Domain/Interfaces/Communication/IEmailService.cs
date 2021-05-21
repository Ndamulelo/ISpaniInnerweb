namespace ISpaniInnerweb.Domain.Interfaces.Services.Communication
{
    public interface IEmailService
    {
        void SendMail(string subject, string message, string to);
    }
}
