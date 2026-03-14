using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<Post?> GetWithCommentsAsync(string id, CancellationToken cancellationToken = default);
        Task<Post?> GetWithVotesAsync(string id, CancellationToken cancellationToken = default);
        Task<Post?> GetWithDetailsAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Post>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Post>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
        Task<(IEnumerable<Post> Items, int TotalCount)> GetByUserIdPaginatedAsync(string userId, int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
