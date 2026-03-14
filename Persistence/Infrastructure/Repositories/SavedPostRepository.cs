using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SavedPostRepository(DecorteeDbContext dbContext) : GenericRepository<SavedPost>(dbContext), ISavedPostRepository
    {
        public async Task<SavedPost?> GetByUserAndPostAsync(string userId, string postId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(sp => sp.UserId == userId && sp.PostId == postId, cancellationToken);
        }

        public async Task<IEnumerable<SavedPost>> GetRecentlySavedAsync(string userId, int take, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(sp => sp.UserId == userId)
                .OrderByDescending(sp => sp.CreatedAt)
                .Take(take)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.User)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.Votes)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.Comments)
                .Include(sp => sp.Post)
                    .ThenInclude(p => p.SavedPosts)
                .ToListAsync(cancellationToken);
        }
    }
}
