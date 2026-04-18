using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository(DecorteeDbContext dbContext) : IUserRepository
    {
        private readonly DecorteeDbContext _dbContext = dbContext;

        public async Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<int> GetPostsCountAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Posts.CountAsync(p => p.UserId == userId, cancellationToken);
        }

        public async Task<int> GetRatingsCountAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Ratings.CountAsync(r => r.UserId == userId, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Update(user);
            await Task.CompletedTask;
        }
    }
}
