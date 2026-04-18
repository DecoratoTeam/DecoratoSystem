using Application.Authntication;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IjwtProvider _jwtProvider;
        private readonly IEmailService _emailService;

        public AuthService(IAuthRepository authRepository, IjwtProvider jwtProvider, IEmailService emailService)
        {
            _authRepository = authRepository;
            _jwtProvider = jwtProvider;
            _emailService = emailService;
        }

        public async Task<GeneralResponseDto<AuthDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var user = await _authRepository.FindUserByEmail(loginDto.Email, cancellationToken);

            if (user is not null)
            {
                var isValidPassword = await _authRepository.CheckPasswordAsync(user, loginDto.Password, cancellationToken);
                if (isValidPassword == PasswordVerificationResult.Success)
                {
                    var (token, expiresIn) = _jwtProvider.GenerateJwtToken(user);

                    var authDto = new AuthDto(user.Id, user.UserName, user.Email, token, expiresIn);

                    return GeneralResponseDto<AuthDto>.Success(authDto);
                }
            }

            return GeneralResponseDto<AuthDto>.Fail(ErrorType.InvalidCredentials, "Invalid email/password");
        }

        public async Task<GeneralResponseDto<AuthDto>> GoogleLoginAsync(GoogleLoginDto googleLoginDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(googleLoginDto.Email) || string.IsNullOrWhiteSpace(googleLoginDto.GoogleId))
            {
                return GeneralResponseDto<AuthDto>.Fail(ErrorType.InvalidCredentials, "Google login data is invalid");
            }

            var user = await _authRepository.FindUserByEmail(googleLoginDto.Email, cancellationToken);

            if (user is null)
            {
                user = new User
                {
                    Name = string.IsNullOrWhiteSpace(googleLoginDto.Name) ? googleLoginDto.Email.Split('@')[0] : googleLoginDto.Name,
                    UserName = googleLoginDto.Email.Split('@')[0],
                    Email = googleLoginDto.Email,
                    GoogleId = googleLoginDto.GoogleId,
                    Password = Guid.NewGuid().ToString("N")
                };

                await _authRepository.RegisterAsync(user, cancellationToken);
            }
            else
            {
                user.GoogleId = googleLoginDto.GoogleId;
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    user.UserName = user.Email.Split('@')[0];
                }
                await _authRepository.UpdateUserAsync(user, cancellationToken);
            }

            var (token, expiresIn) = _jwtProvider.GenerateJwtToken(user);
            return GeneralResponseDto<AuthDto>.Success(new AuthDto(user.Id, user.UserName, user.Email, token, expiresIn));
        }

        public async Task<GeneralResponseDto<ProfileDto>> GetProfileAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _authRepository.FindUserById(userId, cancellationToken);
            if (user is null)
            {
                return GeneralResponseDto<ProfileDto>.Fail(ErrorType.NotFound, "User not found");
            }

            var profile = new ProfileDto(
                user.Id,
                user.Name,
                user.UserName,
                user.Email,
                user.ProfilePicture,
                user.Bio,
                user.IsDarkMode,
                user.Language);

            return GeneralResponseDto<ProfileDto>.Success(profile);
        }

        // ✅ Register without Token
        public async Task<GeneralResponseDto<RegisterResponseDto>> RegisterAsync(RejesterDto rejesterDto, CancellationToken cancellationToken)
        {
            var IsEmailExist = await _authRepository.IsEmailExists(rejesterDto.Email, cancellationToken);
            if (IsEmailExist)
            {
                return GeneralResponseDto<RegisterResponseDto>.Fail(ErrorType.DuplicatedEmail, "Another user with the same email already exists");
            }

            var user = rejesterDto.Adapt<User>();

            // Generate UserName from Email (take part before @)
            user.UserName = rejesterDto.Email.Split('@')[0];

            await _authRepository.RegisterAsync(user, cancellationToken);

            // ✅ Return user data WITHOUT token
            var response = new RegisterResponseDto(user.Id, user.UserName, user.Email, user.Name);

            return GeneralResponseDto<RegisterResponseDto>.Success(response);
        }

        public async Task<GeneralResponseDto<bool>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken)
        {
            var user = await _authRepository.FindUserByEmail(forgotPasswordDto.Email, cancellationToken);

            if (user == null)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User with this email does not exist");
            }

            // Generate 4-digit OTP
            var random = new Random();
            var otp = random.Next(1000, 9999).ToString();

            // Save OTP to user table with expiry (10 minutes)
            user.Otp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);
            await _authRepository.UpdateUserAsync(user, cancellationToken);

            // Send email with OTP
            var emailSent = await _emailService.SendOtpEmailAsync(user.Email, user.UserName, otp, cancellationToken);

            if (!emailSent)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.ServerError, "Failed to send email. Please try again later.");
            }

            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<bool>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken)
        {
            var user = await _authRepository.FindUserByEmail(resetPasswordDto.Email, cancellationToken);

            if (user == null)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User with this email does not exist");
            }

            // Validate OTP
            if (string.IsNullOrEmpty(user.Otp) || user.Otp != resetPasswordDto.Otp)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.InvalidCredentials, "Invalid OTP");
            }

            // Check if OTP has expired
            if (user.OtpExpiry == null || user.OtpExpiry < DateTime.UtcNow)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.InvalidCredentials, "OTP has expired. Please request a new one.");
            }

            // Hash and update password
            user.Password = new PasswordHasher<User>().HashPassword(user, resetPasswordDto.NewPassword);

            // Clear OTP after successful password reset
            user.Otp = null;
            user.OtpExpiry = null;

            await _authRepository.UpdateUserAsync(user, cancellationToken);

            return GeneralResponseDto<bool>.Success(true);
        }
    }
}
