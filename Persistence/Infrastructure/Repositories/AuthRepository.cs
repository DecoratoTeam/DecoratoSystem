using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthRepositor(DecorteeDbContext dbContext) : IAuthRepository
    {
        private readonly DecorteeDbContext _dbContext = dbContext;

        public async Task<PasswordVerificationResult> CheckPasswordAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            var hashedPassword = await _dbContext.Users
                .Where(x => x.Email == user.Email)
                .Select(x => x.Password)
                .FirstAsync(cancellationToken);

            return new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, password);
        }

        public async Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User?> FindUserById(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task RegisterAsync(User user, CancellationToken cancellationToken = default)
        {
            user.Password = new PasswordHasher<User>().HashPassword(user, user.Password);
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Update(user);
            return Task.CompletedTask;
        }
    }
}
