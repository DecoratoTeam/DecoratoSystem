using Application.Dtos;
using Application.Dtos.Profile;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DecorteeSystem.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController(IProfileService profileService, IWebHostEnvironment env) : ControllerBase
    {
        [HttpGet]
        public async Task<GeneralResponseDto<ProfileResponseDto>> GetProfile(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.GetProfileAsync(userId, cancellationToken);
        }

        [HttpPut]
        public async Task<GeneralResponseDto<ProfileResponseDto>> UpdateProfile([FromBody] UpdateProfileDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.UpdateProfileAsync(userId, dto, cancellationToken);
        }

        [HttpPut("password")]
        public async Task<GeneralResponseDto<bool>> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.ChangePasswordAsync(userId, dto, cancellationToken);
        }

        [HttpPost("upload-image")]
        [Consumes("multipart/form-data")]
        public async Task<GeneralResponseDto<string>> UploadProfileImage(IFormFile image, CancellationToken cancellationToken)
        {
            if (image is null || image.Length == 0)
                return GeneralResponseDto<string>.Fail(ErrorType.BadRequest, "No image file provided");

            const long maxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
            if (image.Length > maxFileSizeBytes)
                return GeneralResponseDto<string>.Fail(ErrorType.BadRequest, "File size must not exceed 5 MB");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                return GeneralResponseDto<string>.Fail(ErrorType.BadRequest, "Only image files (jpg, jpeg, png, gif, webp) are allowed");

            var webRootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "uploads", "profile");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            var imageUrl = $"/uploads/profile/{fileName}";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.UpdateProfileImageAsync(userId, imageUrl, cancellationToken);
        }

        [HttpPut("theme")]
        public async Task<GeneralResponseDto<bool>> UpdateTheme([FromBody] UpdateThemeDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.UpdateThemeAsync(userId, dto, cancellationToken);
        }

        [HttpPut("language")]
        public async Task<GeneralResponseDto<bool>> UpdateLanguage([FromBody] UpdateLanguageDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.UpdateLanguageAsync(userId, dto, cancellationToken);
        }

        [HttpGet("posts")]
        public async Task<GeneralResponseDto<IEnumerable<ProfilePostDto>>> GetMyPosts(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.GetMyPostsAsync(userId, cancellationToken);
        }

        [HttpGet("ratings")]
        public async Task<GeneralResponseDto<IEnumerable<ProfileRatingDto>>> GetMyRatings(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await profileService.GetMyRatingsAsync(userId, cancellationToken);
        }
    }
}
