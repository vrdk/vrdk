using System.Threading.Tasks;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendPasswordResetLink(string address, string name, string title, string plainTextContent, string message);
    }
}
