using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DesignStyleRepository(DecorteeDbContext dbContext) : GenericRepository<DesignStyle>(dbContext), IDesignStyleRepository
    {
        public async Task<DesignStyle?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        }
    }
}
