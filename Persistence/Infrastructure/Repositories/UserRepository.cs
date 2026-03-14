using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository(DecorteeDbContext dbContext) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            dbContext.Users.Update(user);
            return Task.CompletedTask;
        }
    }
}
