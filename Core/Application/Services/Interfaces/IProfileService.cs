using Application.Dtos;
using Application.Dtos.Profile;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IProfileService
    {
        Task<GeneralResponseDto<ProfileResponseDto>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto<ProfileResponseDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto<string>> UpdateProfileImageAsync(string userId, string imageUrl, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto<bool>> UpdateThemeAsync(string userId, UpdateThemeDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto<bool>> UpdateLanguageAsync(string userId, UpdateLanguageDto dto, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto<IEnumerable<ProfilePostDto>>> GetMyPostsAsync(string userId, CancellationToken cancellationToken = default);
        Task<GeneralResponseDto<IEnumerable<ProfileRatingDto>>> GetMyRatingsAsync(string userId, CancellationToken cancellationToken = default);
    }
}
