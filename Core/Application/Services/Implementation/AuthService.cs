using Application.Authntication;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            var error = GeneralResponseDto<AuthDto>.Fail(ErrorType.InvalidCredentials, "Invalid email/password");
            return error;
        }

        public async Task<GeneralResponseDto<bool>> RegisterAsync(RejesterDto rejesterDto, CancellationToken cancellationToken)
        {
            var IsEmailExist = await _authRepository.IsEmailExists(rejesterDto.Email, cancellationToken);
            if (IsEmailExist)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.DuplicatedEmail, "Another user with the same email is already exists");
            }
            
            // Create user with default values for missing fields
            var user = new User
            {
                Name = rejesterDto.Name,
                Email = rejesterDto.Email,
                Password = rejesterDto.Password,
                UserName = rejesterDto.Email.Split('@')[0], // Use email prefix as username
                Phone = string.Empty, // Default empty phone
                Role = Role.Customer // Default role as Customer
            };
            
            await _authRepository.RegisterAsync(user, cancellationToken);

            return GeneralResponseDto<bool>.Success(true);
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
