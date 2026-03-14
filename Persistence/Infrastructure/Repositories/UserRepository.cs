using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(DecorteeDbContext dbContext) : GenericRepository<User>(dbContext), IUserRepository
    {
        public async Task<User?> GetByIdWithPostsAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(u => u.MyPosts)
                    .ThenInclude(p => p.Votes)
                .Include(u => u.MyPosts)
                    .ThenInclude(p => p.Comments)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByIdWithRatingsAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(u => u.MyRatings)
                    .ThenInclude(r => r.Post)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }
    }
}