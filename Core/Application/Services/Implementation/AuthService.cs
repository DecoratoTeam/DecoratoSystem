using Application.Authntication;
using Application.Dtos;
using Application.Dtos.Auth;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class AuthService (IAuthRepository _authRepository,IjwtProvider _jwtProvider) : IAuthService
    {
        public async Task<GeneralResponseDto<AuthDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var user = await _authRepository.FindUserByEmail(loginDto.Email, cancellationToken);
            //todo validate on user if its null;

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
            
            var IsEmailExist= await _authRepository.IsEmailExists(rejesterDto.Email,cancellationToken);
            if (IsEmailExist)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.DuplicatedEmail, "Another user with the same email is already exists");

            }
                await _authRepository.RegisterAsync(rejesterDto.Adapt<User>(), cancellationToken);

                return GeneralResponseDto<bool>.Success(true);
            }


    }
    
}
