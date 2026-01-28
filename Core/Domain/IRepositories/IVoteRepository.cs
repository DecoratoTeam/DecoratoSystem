using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IVoteRepository : IGenericRepository<Vote>
    {
        Task<Vote?> GetByUserAndPostAsync(string userId, string postId, CancellationToken cancellationToken = default);
    }
}
