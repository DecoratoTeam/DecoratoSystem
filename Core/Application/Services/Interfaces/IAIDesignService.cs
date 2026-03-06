using Application.Dtos;
using Application.Dtos.AIDesign;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAIDesignService
    {
        Task<GeneralResponseDto<IEnumerable<AIDesignResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<GeneralResponseDto<AIDesignResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<AIDesignResponseDto>> CreateAsync(string userId, CreateAIDesignDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<AIDesignResponseDto>> UpdateAsync(string id, UpdateAIDesignDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<AIDesignResponseDto>>> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
    }
}
