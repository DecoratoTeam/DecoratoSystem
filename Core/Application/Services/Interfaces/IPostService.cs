using Application.Dtos;
using Application.Dtos.Post;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetAllAsync(string? userId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<PostResponseDto>> GetByIdAsync(string id, string? userId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<PostWithCommentsDto>> GetWithCommentsAsync(string id, string? userId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<PostResponseDto>> CreateAsync(string userId, CreatePostDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<PostResponseDto>> UpdateAsync(string id, UpdatePostDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ToggleLikeResponseDto>> ToggleLikeAsync(string userId, string postId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ToggleSaveResponseDto>> ToggleSaveAsync(string userId, string postId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetSavedPostsAsync(string userId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetRecentlySavedAsync(string userId, int take, CancellationToken cancellationToken);
    }
}
