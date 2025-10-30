using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IAuthRepository
    {
        Task RegisterAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> FindUserByEmail(string email, CancellationToken cancellationToken = default);
        Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default);
        Task<PasswordVerificationResult> CheckPasswordAsync(User user, string password, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    }
}
