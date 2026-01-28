using Application.Dtos;
using Application.Dtos.ShowcaseDesign;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IShowcaseDesignService
    {
        Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<GeneralResponseDto<ShowcaseDesignResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ShowcaseDesignResponseDto>> CreateAsync(string userId, CreateShowcaseDesignDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ShowcaseDesignResponseDto>> UpdateAsync(string id, UpdateShowcaseDesignDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetByRoomTypeAsync(string roomTypeId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetByStyleAsync(string styleId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetPopularAsync(int take, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetTrendingAsync(int take, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetFilteredAsync(ShowcaseDesignFilterDto filter, CancellationToken cancellationToken);
    }
}
