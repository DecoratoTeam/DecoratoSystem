using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<int> GetPostsCountAsync(string userId, CancellationToken cancellationToken = default);
        Task<int> GetRatingsCountAsync(string userId, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    }
}
