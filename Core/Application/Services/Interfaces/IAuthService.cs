using Application.Dtos;
using Application.Dtos.Auth;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralResponseDto<AuthDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);

        // ✅ Register returns RegisterResponseDto (without Token)
        Task<GeneralResponseDto<RegisterResponseDto>> RegisterAsync(RejesterDto rejesterDto, CancellationToken cancellationToken);

        Task<GeneralResponseDto<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken);

        Task<GeneralResponseDto<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken);
    }
}
