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
        public Task<PasswordVerificationResult> CheckPasswordAsync(User user, string password, CancellationToken cancellationToken = default)
        {

            throw new NotImplementedException();
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
