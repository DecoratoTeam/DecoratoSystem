using Application.Authntication;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Google.Apis.Auth;
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

        // ✅ Google Login
        public async Task<GeneralResponseDto<AuthDto>> GoogleLoginAsync(GoogleLoginDto googleLoginDto, CancellationToken cancellationToken)
        {
            // 1. Validate the Google ID Token
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(googleLoginDto.IdToken);
            }
            catch (InvalidJwtException)
            {
                return GeneralResponseDto<AuthDto>.Fail(ErrorType.InvalidCredentials, "Invalid Google token");
            }

            // 2. Extract user info from Google payload
            var email = payload.Email;
            var name = payload.Name ?? email.Split('@')[0];
            var googleId = payload.Subject;
            var profilePicture = payload.Picture;

            if (string.IsNullOrEmpty(email))
            {
                return GeneralResponseDto<AuthDto>.Fail(ErrorType.InvalidCredentials, "Google account has no email");
            }

            // 3. Check if user already exists
            var user = await _authRepository.FindUserByEmail(email, cancellationToken);

            if (user is null)
            {
                // 4. Create new user
                user = new User
                {
                    Name = name,
                    Email = email,
                    UserName = email.Split('@')[0],
                    GoogleId = googleId,
                    ProfilePicture = profilePicture,
                    Role = Role.Customer,
                    Password = GenerateRandomPassword()
                };

                await _authRepository.RegisterAsync(user, cancellationToken);
            }
            else
            {
                // 5. Link Google ID if not linked yet
                if (string.IsNullOrEmpty(user.GoogleId))
                {
                    user.GoogleId = googleId;
                }

                // Update profile picture from Google if user has none
                if (string.IsNullOrEmpty(user.ProfilePicture) && !string.IsNullOrEmpty(profilePicture))
                {
                    user.ProfilePicture = profilePicture;
                }

                await _authRepository.UpdateUserAsync(user, cancellationToken);
            }

            // 6. Generate JWT
            var (token, expiresIn) = _jwtProvider.GenerateJwtToken(user);

            var authDto = new AuthDto(user.Id, user.UserName, user.Email, token, expiresIn);

            return GeneralResponseDto<AuthDto>.Success(authDto);
        }

        private static string GenerateRandomPassword(int length = 16)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "1234567890";
            const string specials = "!@#$%^&*()-_=+";

            var random = new Random();
            var res = new char[length];

            // Ensure at least one of each required type
            res[0] = letters[random.Next(letters.Length)];
            res[1] = char.ToUpper(letters[random.Next(26)]);
            res[2] = digits[random.Next(digits.Length)];
            res[3] = specials[random.Next(specials.Length)];

            var all = letters + digits + specials;
            for (int i = 4; i < length; i++)
            {
                res[i] = all[random.Next(all.Length)];
            }

            // Shuffle
            return new string(res.OrderBy(_ => random.Next()).ToArray());
        }
    }
}
