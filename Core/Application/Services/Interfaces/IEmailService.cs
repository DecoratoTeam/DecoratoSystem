using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendOtpEmailAsync(string toEmail, string userName, string otp, CancellationToken cancellationToken = default);
    }
}