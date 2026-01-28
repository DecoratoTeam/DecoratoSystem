using Application.Dtos;
using Application.Dtos.Comment;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICommentService
    {
        Task<GeneralResponseDto<IEnumerable<CommentResponseDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<GeneralResponseDto<CommentResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<CommentResponseDto>>> GetByPostIdAsync(string postId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<CommentResponseDto>> CreateAsync(string userId, CreateCommentDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<CommentResponseDto>> UpdateAsync(string id, UpdateCommentDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
