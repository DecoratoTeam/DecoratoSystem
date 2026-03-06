using Application.Dtos;
using Application.Dtos.DesignStyle;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IDesignStyleService
    {
        Task<GeneralResponseDto<IEnumerable<DesignStyleResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<GeneralResponseDto<DesignStyleResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<DesignStyleResponseDto>> CreateAsync(CreateDesignStyleDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<DesignStyleResponseDto>> UpdateAsync(string id, UpdateDesignStyleDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
