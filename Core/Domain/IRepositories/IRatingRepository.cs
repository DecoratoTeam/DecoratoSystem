using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<Rating?> GetByUserAndDesignAsync(string userId, string showcaseDesignId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Rating>> GetByDesignIdAsync(string showcaseDesignId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Rating>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
