using Application.Dtos.Auth;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Azure;
using DecorteeSystem.ViewModles;
using DecorteeSystem.ViewModles.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DecorteeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        private readonly IAuthService _authService = authService;
        
        [HttpPost("Register")]
        public async Task<GeneralResponseViewModle<bool>> Register([FromBody] RejesterViewModle registerView, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(registerView.Adapt<RejesterDto>(), cancellationToken);
            return result.IsSuccess 
                ? GeneralResponseViewModle<bool>.Success(result.Data) 
                : GeneralResponseViewModle<bool>.Fail(result.Error, result.Message);
        }
        
        [HttpPost("")]
        public async Task<GeneralResponseViewModle<AuthViewModle>> Login([FromBody] LoginViewModle loginView, CancellationToken cancellationToken)
        {
            var authDto = await _authService.LoginAsync(loginView.Adapt<LoginDto>(), cancellationToken);

            return authDto.IsSuccess 
                ? GeneralResponseViewModle<AuthViewModle>.Success(authDto.Data.Adapt<AuthViewModle>())
                : GeneralResponseViewModle<AuthViewModle>.Fail(authDto.Error, authDto.Message);
        }

        [HttpPost("ForgotPassword")]
        public async Task<GeneralResponseViewModle<bool>> ForgotPassword([FromBody] ForgotPasswordViewModle forgotPasswordView, CancellationToken cancellationToken)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPasswordView.Adapt<ForgotPasswordDto>(), cancellationToken);
            return result.IsSuccess
                ? GeneralResponseViewModle<bool>.Success(result.Data)
                : GeneralResponseViewModle<bool>.Fail(result.Error, result.Message);
        }

        [HttpPost("ResetPassword")]
        public async Task<GeneralResponseViewModle<bool>> ResetPassword([FromBody] ResetPasswordViewModle resetPasswordView, CancellationToken cancellationToken)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordView.Adapt<ResetPasswordDto>(), cancellationToken);
            return result.IsSuccess 
                ? GeneralResponseViewModle<bool>.Success(result.Data) 
                : GeneralResponseViewModle<bool>.Fail(result.Error, result.Message);
        }
    }
}
