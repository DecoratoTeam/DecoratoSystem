using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class VoteRepository(DecorteeDbContext dbContext) : GenericRepository<Vote>(dbContext), IVoteRepository
    {
        public async Task<Vote?> GetByUserAndPostAsync(string userId, string postId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId, cancellationToken);
        }
    }
}
