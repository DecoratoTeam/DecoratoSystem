using Domain.Entities;

namespace Domain.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByIdWithPostsAsync(string userId, CancellationToken cancellationToken = default);
        Task<User?> GetByIdWithRatingsAsync(string userId, CancellationToken cancellationToken = default);
    }
}