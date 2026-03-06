using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IAIDesignRepository : IGenericRepository<AIDesign>
    {
        Task<IEnumerable<AIDesign>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
