using Application.Dtos.Auth;
using Application.Services.Interfaces;
using DecorteeSystem.ViewModles;
using DecorteeSystem.ViewModles.Auth;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace DecorteeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Register")]
        [ProducesResponseType(typeof(GeneralResponseViewModle<RegisterResponseViewModle>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseViewModle<RegisterResponseViewModle>>> Register(
            [FromBody] RejesterViewModle registerView,
            CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(
                registerView.Adapt<RejesterDto>(),
                cancellationToken);

            return Ok(result.IsSuccess
                ? GeneralResponseViewModle<RegisterResponseViewModle>.Success(result.Data.Adapt<RegisterResponseViewModle>())
                : GeneralResponseViewModle<RegisterResponseViewModle>.Fail(result.Error, result.Message));
        }

        // ✅ Login - Returns 200 OK only (removed 400 Bad Request)
        [HttpPost("Login")]
        [ProducesResponseType(typeof(GeneralResponseViewModle<AuthViewModle>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseViewModle<AuthViewModle>>> Login(
            [FromBody] LoginViewModle loginView,
            CancellationToken cancellationToken)
        {
            var authDto = await _authService.LoginAsync(loginView.Adapt<LoginDto>(), cancellationToken);

            // ✅ Always return 200 OK (even for errors)
            return Ok(authDto.IsSuccess
                ? GeneralResponseViewModle<AuthViewModle>.Success(authDto.Data.Adapt<AuthViewModle>())
                : GeneralResponseViewModle<AuthViewModle>.Fail(authDto.Error, authDto.Message));
        }

        [HttpPost("ForgotPassword")]
        [ProducesResponseType(typeof(GeneralResponseViewModle<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseViewModle<bool>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeneralResponseViewModle<bool>>> ForgotPassword(
            [FromBody] ForgotPasswordViewModle forgotPasswordView,
            CancellationToken cancellationToken)
        {
            var result = await _authService.ForgotPasswordAsync(
                forgotPasswordView.Adapt<ForgotPasswordDto>(),
                cancellationToken);

            if (result.IsSuccess)
                return Ok(GeneralResponseViewModle<bool>.Success(result.Data));

            return BadRequest(GeneralResponseViewModle<bool>.Fail(result.Error, result.Message));
        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(GeneralResponseViewModle<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseViewModle<bool>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeneralResponseViewModle<bool>>> ResetPassword(
            [FromBody] ResetPasswordViewModle resetPasswordView,
            CancellationToken cancellationToken)
        {
            var result = await _authService.ResetPasswordAsync(
                resetPasswordView.Adapt<ResetPasswordDto>(),
                cancellationToken);

            if (result.IsSuccess)
                return Ok(GeneralResponseViewModle<bool>.Success(result.Data));

            return BadRequest(GeneralResponseViewModle<bool>.Fail(result.Error, result.Message));
        }
    }
}
