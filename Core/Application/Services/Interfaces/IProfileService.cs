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
        Task<GeneralResponseDto<ProfileResponseDto>> UpdateLanguageAsync(string userId, UpdateLanguageDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ProfileResponseDto>> UpdateThemeAsync(string userId, UpdateThemeDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ProfileResponseDto>> UploadProfileImageAsync(string userId, IFormFile file, CancellationToken cancellationToken);
        Task<GeneralResponseDto<PaginatedResultDto<UserPostDto>>> GetMyPostsAsync(string userId, int page, int pageSize, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeletePostAsync(string userId, string postId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<UserRatingDto>>> GetMyRatingsAsync(string userId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<UserRatingDto>> RatePostAsync(string userId, string postId, RatePostDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ProfileStatsDto>> GetStatsAsync(string userId, CancellationToken cancellationToken);
    }
}