using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PostRepository(DecorteeDbContext dbContext) 
        : GenericRepository<Post>(dbContext), IPostRepository
    {
        public async Task<Post?> GetWithCommentsAsync(string id, 
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Post?> GetWithVotesAsync(string id, 
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Post?> GetWithDetailsAsync(string id, 
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetByUserIdAsync(string userId, 
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Votes)
                .Include(p => p.Comments)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetAllWithDetailsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Votes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
