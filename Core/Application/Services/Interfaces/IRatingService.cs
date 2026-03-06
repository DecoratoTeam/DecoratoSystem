using Application.Dtos;
using Application.Dtos.Rating;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IRatingService
    {
        Task<GeneralResponseDto<IEnumerable<RatingResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<GeneralResponseDto<RatingResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<RatingResponseDto>>> GetByDesignIdAsync(string showcaseDesignId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<RatingResponseDto>> CreateOrUpdateAsync(string userId, CreateRatingDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
