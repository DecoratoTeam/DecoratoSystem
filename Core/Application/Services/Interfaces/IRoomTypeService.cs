using Application.Dtos;
using Application.Dtos.RoomType;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IRoomTypeService
    {
        Task<GeneralResponseDto<IEnumerable<RoomTypeResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<GeneralResponseDto<RoomTypeResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<RoomTypeResponseDto>> CreateAsync(CreateRoomTypeDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<RoomTypeResponseDto>> UpdateAsync(string id, UpdateRoomTypeDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
