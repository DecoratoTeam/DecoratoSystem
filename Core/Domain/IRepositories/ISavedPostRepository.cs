using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ISavedPostRepository : IGenericRepository<SavedPost>
    {
        Task<SavedPost?> GetByUserAndPostAsync(string userId, string postId, CancellationToken cancellationToken = default);
        Task<IEnumerable<SavedPost>> GetRecentlySavedAsync(string userId, int take, CancellationToken cancellationToken = default);
    }
}
