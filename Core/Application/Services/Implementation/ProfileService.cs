using Application.Dtos;
using Application.Dtos.Profile;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Implementation
{
    public class ProfileService(
        IUserRepository userRepository,
        IPostRepository postRepository,
        IRatingRepository ratingRepository,
        IAuthRepository authRepository) : IProfileService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IRatingRepository _ratingRepository = ratingRepository;
        private readonly IAuthRepository _authRepository = authRepository;

        public async Task<GeneralResponseDto<ProfileResponseDto>> GetProfileAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithRatingsAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");

            var posts = await _postRepository.GetByUserIdAsync(userId, cancellationToken);
            var postsCount = posts.Count();
            var ratingsCount = user.MyRatings?.Count ?? 0;

            return GeneralResponseDto<ProfileResponseDto>.Success(MapToProfileResponse(user, postsCount, ratingsCount));
        }

        public async Task<GeneralResponseDto<ProfileResponseDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");

            user.Name = dto.Name;
            user.Bio = dto.Bio;

            // Update email if provided and different
            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                var existingUser = await _authRepository.FindUserByEmail(dto.Email, cancellationToken);
                if (existingUser != null && existingUser.Id != userId)
                    return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.DuplicatedEmail, "This email is already in use");

                user.Email = dto.Email;
            }

            await _userRepository.UpdateAsync(user, cancellationToken);
            return GeneralResponseDto<ProfileResponseDto>.Success(MapToProfileResponse(user));
        }

        public async Task<GeneralResponseDto<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "User not found");

            var passwordHasher = new PasswordHasher<User>();
            var verifyResult = passwordHasher.VerifyHashedPassword(user, user.Password, dto.CurrentPassword);

            if (verifyResult == PasswordVerificationResult.Failed)
                return GeneralResponseDto<bool>.Fail(ErrorType.InvalidCredentials, "Current password is incorrect");

            user.Password = passwordHasher.HashPassword(user, dto.NewPassword);
            await _userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<ProfileResponseDto>> UpdateLanguageAsync(string userId, UpdateLanguageDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");

            user.Language = dto.Language;
            await _userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<ProfileResponseDto>.Success(MapToProfileResponse(user));
        }

        public async Task<GeneralResponseDto<ProfileResponseDto>> UpdateThemeAsync(string userId, UpdateThemeDto dto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");

            user.IsDarkMode = dto.IsDarkMode;
            await _userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<ProfileResponseDto>.Success(MapToProfileResponse(user));
        }

        public async Task<GeneralResponseDto<ProfileResponseDto>> UploadProfileImageAsync(string userId, IFormFile file, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.NotFound, "User not found");

            if (file == null || file.Length == 0)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.InvalidCredentials, "No file uploaded");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.InvalidCredentials, "Invalid file type. Allowed: jpg, jpeg, png, webp");

            const long maxSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxSize)
                return GeneralResponseDto<ProfileResponseDto>.Fail(ErrorType.InvalidCredentials, "File size exceeds 5 MB limit");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile");
            Directory.CreateDirectory(uploadsFolder);

            // Delete old image if exists
            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePicture.TrimStart('/'));
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);
            }

            var fileName = $"{userId}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            user.ProfilePicture = $"/uploads/profile/{fileName}";
            await _userRepository.UpdateAsync(user, cancellationToken);

            return GeneralResponseDto<ProfileResponseDto>.Success(MapToProfileResponse(user));
        }

        public async Task<GeneralResponseDto<PaginatedResultDto<UserPostDto>>> GetMyPostsAsync(
            string userId, int page, int pageSize, CancellationToken cancellationToken)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 50) pageSize = 50;

            var (posts, totalCount) = await _postRepository.GetByUserIdPaginatedAsync(userId, page, pageSize, cancellationToken);

            var postDtos = posts.Select(p => new UserPostDto(
                p.Id,
                p.Title,
                p.Content,
                p.ImageUrl,
                p.CreatedAt,
                p.UpdatedAt,
                p.ViewCount,
                p.Votes?.Count(v => v.IsUpvote) ?? 0,
                p.Votes?.Count(v => !v.IsUpvote) ?? 0,
                p.Comments?.Count ?? 0));

            var result = new PaginatedResultDto<UserPostDto>
            {
                Items = postDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return GeneralResponseDto<PaginatedResultDto<UserPostDto>>.Success(result);
        }

        public async Task<GeneralResponseDto<bool>> DeletePostAsync(string userId, string postId, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(postId, cancellationToken);
            if (post == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "Post not found");

            if (post.UserId != userId)
                return GeneralResponseDto<bool>.Fail(ErrorType.InvalidCredentials, "You can only delete your own posts");

            await _postRepository.DeleteAsync(post, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<IEnumerable<UserRatingDto>>> GetMyRatingsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithRatingsAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<IEnumerable<UserRatingDto>>.Fail(ErrorType.NotFound, "User not found");

            var dtos = user.MyRatings.Select(r => new UserRatingDto(
                r.Id,
                r.ShowcaseDesignId,
                r.PostId,
                r.Post?.Title,
                r.Value,
                r.Review,
                r.CreatedAt));

            return GeneralResponseDto<IEnumerable<UserRatingDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<UserRatingDto>> RatePostAsync(string userId, string postId, RatePostDto dto, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(postId, cancellationToken);
            if (post == null)
                return GeneralResponseDto<UserRatingDto>.Fail(ErrorType.NotFound, "Post not found");

            // Check if user already rated this post
            var existingRatings = await _ratingRepository.FindAsync(
                r => r.UserId == userId && r.PostId == postId, cancellationToken);
            var existing = existingRatings.FirstOrDefault();

            if (existing != null)
            {
                existing.Value = dto.Value;
                existing.Review = dto.Review;
                await _ratingRepository.UpdateAsync(existing, cancellationToken);

                return GeneralResponseDto<UserRatingDto>.Success(new UserRatingDto(
                    existing.Id, existing.ShowcaseDesignId, existing.PostId,
                    post.Title, existing.Value, existing.Review, existing.CreatedAt));
            }

            var rating = new Rating
            {
                UserId = userId,
                PostId = postId,
                Value = dto.Value,
                Review = dto.Review
            };

            await _ratingRepository.AddAsync(rating, cancellationToken);

            return GeneralResponseDto<UserRatingDto>.Success(new UserRatingDto(
                rating.Id, rating.ShowcaseDesignId, rating.PostId,
                post.Title, rating.Value, rating.Review, rating.CreatedAt));
        }

        public async Task<GeneralResponseDto<ProfileStatsDto>> GetStatsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdWithRatingsAsync(userId, cancellationToken);
            if (user == null)
                return GeneralResponseDto<ProfileStatsDto>.Fail(ErrorType.NotFound, "User not found");

            var posts = await _postRepository.GetByUserIdAsync(userId, cancellationToken);
            var totalPosts = posts.Count();
            var totalRatings = user.MyRatings?.Count ?? 0;
            var averageRating = user.MyRatings?.Any() == true
                ? Math.Round(user.MyRatings.Average(r => r.Value), 2)
                : 0.0;

            return GeneralResponseDto<ProfileStatsDto>.Success(
                new ProfileStatsDto(totalPosts, totalRatings, averageRating));
        }

        // Overload for quick calls without counts (used by update methods)
        private static ProfileResponseDto MapToProfileResponse(User user, int postsCount = 0, int ratingsCount = 0)
        {
            return new ProfileResponseDto(
                user.Id,
                user.UserName,
                user.Name,
                user.Email,
                user.ProfilePicture,
                user.Bio,
                user.Language,
                user.IsDarkMode,
                postsCount,
                ratingsCount);
        }
    }
}