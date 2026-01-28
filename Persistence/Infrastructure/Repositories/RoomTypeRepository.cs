using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoomTypeRepository(DecorteeDbContext dbContext) : GenericRepository<RoomType>(dbContext), IRoomTypeRepository
    {
        public async Task<RoomType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }
    }
}
