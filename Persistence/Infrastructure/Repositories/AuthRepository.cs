using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthRepositor(DecorteeDbContext _dbContext) : IAuthRepository
    {
        public async Task<PasswordVerificationResult> CheckPasswordAsync(User user, string password, CancellationToken cancellationToken = default)
        {

            var hashedPassword = await _dbContext.Users.Where(x => x.Email == user.Email).Select(x => x.Password).FirstAsync(cancellationToken);
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, password);

            return result;
        }

        public async Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken = default)
        {
           var user=  await _dbContext.Users.FirstOrDefaultAsync(x=> x.Email == email, cancellationToken); 
            return user;
        }

        public async Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default)
        {
           return await _dbContext.Users.AnyAsync(u => u.Email == email,cancellationToken);
        }

        public async Task RegisterAsync(User user, CancellationToken cancellationToken = default)
        {
            user.Password=new PasswordHasher<User>().HashPassword(user,user.Password);
            await _dbContext.Users.AddAsync(user);
        }
    }
}
