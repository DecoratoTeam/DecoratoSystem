using Application.Dtos;
using Application.Dtos.Profile;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface IProfileService
    {
        Task<GeneralResponseDto<ProfileResponseDto>> GetProfileAsync(string userId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ProfileResponseDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<string>> UploadProfileImageAsync(string userId, IFormFile image, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> UpdateThemeAsync(string userId, UpdateThemeDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> UpdateLanguageAsync(string userId, UpdateLanguageDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<UserPostDto>>> GetMyPostsAsync(string userId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<UserRatingDto>>> GetMyRatingsAsync(string userId, CancellationToken cancellationToken);
    }
}
