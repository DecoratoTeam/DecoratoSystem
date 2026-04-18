using Application.Dtos;
using Application.Dtos.Profile;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Implementation
{
    public class ProfileService(
        IUserRepository userRepository,
        IPostRepository postRepository,
        IRatingRepository ratingRepository,
        IWebHostEnvironment webHostEnvironment) : IProfileService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IRatingRepository _ratingRepository = ratingRepository;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<GeneralResponseDto<ProfileResponseDto>> GetProfileAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");
            }

            var postsCount = await _userRepository.GetPostsCountAsync(userId, cancellationToken);
            var ratingsCount = await _userRepository.GetRatingsCountAsync(userId, cancellationToken);

            return GeneralResponseDto<ProfileResponseDto>.Success(new ProfileResponseDto(
                user.Name,
                user.Email,
                user.ProfilePicture,
                user.IsDarkMode,
                user.Language,
                postsCount,
                ratingsCount));
        }

        public async Task<GeneralResponseDto<ProfileResponseDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");
            }

            var userByEmail = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
            if (userByEmail is not null && userByEmail.Id != userId)
            {
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.DuplicatedEmail, "Another user with the same email already exists");
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                user.UserName = dto.Email.Split('@')[0];
            }

            await _userRepository.UpdateAsync(user, cancellationToken);
            return await GetProfileAsync(userId, cancellationToken);
        }

        public async Task<GeneralResponseDto<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User not found");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verifyResult = passwordHasher.VerifyHashedPassword(user, user.Password, dto.OldPassword);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.InvalidCredentials, "Old password is incorrect");
            }

            user.Password = passwordHasher.HashPassword(user, dto.NewPassword);
            await _userRepository.UpdateAsync(user, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<string>> UploadProfileImageAsync(string userId, IFormFile image, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GeneralResponseDto<string>.Fail(ErrorType.NotFound, "User not found");
            }

            if (image is null || image.Length == 0)
            {
                return GeneralResponseDto<string>.Fail(ErrorType.InvalidCredentials, "Image file is required");
            }

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension))
            {
                return GeneralResponseDto<string>.Fail(ErrorType.InvalidCredentials, "Only jpg, jpeg, png, webp are allowed");
            }

            var webRoot = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRoot))
            {
                webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var directory = Path.Combine(webRoot, "uploads", "profile");
            Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(directory, fileName);

            await using (var stream = File.Create(fullPath))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }

            var imageUrl = $"/uploads/profile/{fileName}";
            user.ProfilePicture = imageUrl;
            await _userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<string>.Success(imageUrl);
        }

        public async Task<GeneralResponseDto<bool>> UpdateThemeAsync(string userId, UpdateThemeDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User not found");
            }

            user.IsDarkMode = dto.DarkMode;
            await _userRepository.UpdateAsync(user, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<bool>> UpdateLanguageAsync(string userId, UpdateLanguageDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User not found");
            }

            user.Language = dto.Language;
            await _userRepository.UpdateAsync(user, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<IEnumerable<UserPostDto>>> GetMyPostsAsync(string userId, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetByUserIdAsync(userId, cancellationToken);
            var result = posts.Select(p => new UserPostDto(p.Id, p.ImageUrl, p.Content, p.CreatedAt));
            return GeneralResponseDto<IEnumerable<UserPostDto>>.Success(result);
        }

        public async Task<GeneralResponseDto<IEnumerable<UserRatingDto>>> GetMyRatingsAsync(string userId, CancellationToken cancellationToken)
        {
            var ratings = await _ratingRepository.GetByUserIdAsync(userId, cancellationToken);
            var result = ratings.Select(r => new UserRatingDto(
                r.ShowcaseDesignId,
                r.ShowcaseDesign?.Title ?? string.Empty,
                r.ShowcaseDesign?.ImageUrl,
                r.Value));

            return GeneralResponseDto<IEnumerable<UserRatingDto>>.Success(result);
        }
    }
}
