using Application.Dtos;
using Application.Dtos.Vote;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IVoteService
    {
        Task<GeneralResponseDto<IEnumerable<VoteResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<GeneralResponseDto<VoteResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<VoteResponseDto>> CreateOrUpdateAsync(string userId, CreateVoteDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
