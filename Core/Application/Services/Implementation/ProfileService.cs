using Application.Dtos;
using Application.Dtos.Profile;
using Application.Services.Interfaces;
using Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ProfileService(
        IUserRepository userRepository,
        IPostRepository postRepository,
        IRatingRepository ratingRepository,
        IAuthRepository authRepository) : IProfileService
    {
        public async Task<GeneralResponseDto<ProfileResponseDto>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");

            var posts = await postRepository.GetByUserIdAsync(userId, cancellationToken);
            var ratings = await ratingRepository.GetByUserIdAsync(userId, cancellationToken);

            var dto = new ProfileResponseDto(
                user.Id,
                user.Name,
                user.Email,
                user.ProfilePicture,
                user.IsDarkMode,
                user.Language,
                posts.Count(),
                ratings.Count()
            );

            return GeneralResponseDto<ProfileResponseDto>.Success(dto);
        }

        public async Task<GeneralResponseDto<ProfileResponseDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");

            user.Name = dto.Name;
            user.Email = dto.Email;

            await userRepository.UpdateAsync(user, cancellationToken);

            var posts = await postRepository.GetByUserIdAsync(userId, cancellationToken);
            var ratings = await ratingRepository.GetByUserIdAsync(userId, cancellationToken);

            var response = new ProfileResponseDto(
                user.Id,
                user.Name,
                user.Email,
                user.ProfilePicture,
                user.IsDarkMode,
                user.Language,
                posts.Count(),
                ratings.Count()
            );

            return GeneralResponseDto<ProfileResponseDto>.Success(response);
        }

        public async Task<GeneralResponseDto<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User not found");

            var verificationResult = await authRepository.CheckPasswordAsync(user, dto.OldPassword, cancellationToken);
            if (verificationResult != PasswordVerificationResult.Success)
                return GeneralResponseDto<bool>.Fail(ErrorType.InvalidCredentials, "Current password is incorrect");

            user.Password = new PasswordHasher<User>().HashPassword(user, dto.NewPassword);
            await userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<string>> UpdateProfileImageAsync(string userId, string imageUrl, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return GeneralResponseDto<string>.Fail(ErrorType.NotFound, "User not found");

            user.ProfilePicture = imageUrl;
            await userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<string>.Success(imageUrl);
        }

        public async Task<GeneralResponseDto<bool>> UpdateThemeAsync(string userId, UpdateThemeDto dto, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User not found");

            user.IsDarkMode = dto.DarkMode;
            await userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<bool>> UpdateLanguageAsync(string userId, UpdateLanguageDto dto, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User not found");

            user.Language = dto.Language;
            await userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<IEnumerable<ProfilePostDto>>> GetMyPostsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var posts = await postRepository.GetByUserIdAsync(userId, cancellationToken);

            var result = posts.Select(p => new ProfilePostDto(
                p.Id,
                p.ImageUrl,
                p.Content,
                p.CreatedAt
            ));

            return GeneralResponseDto<IEnumerable<ProfilePostDto>>.Success(result);
        }

        public async Task<GeneralResponseDto<IEnumerable<ProfileRatingDto>>> GetMyRatingsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var ratings = await ratingRepository.GetByUserIdAsync(userId, cancellationToken);

            var result = ratings.Select(r => new ProfileRatingDto(
                r.ShowcaseDesignId,
                r.ShowcaseDesign?.Title,
                r.Value,
                r.Review
            ));

            return GeneralResponseDto<IEnumerable<ProfileRatingDto>>.Success(result);
        }
    }
}
