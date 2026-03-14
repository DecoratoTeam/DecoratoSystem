using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AIDesignRepository(DecorteeDbContext dbContext) : GenericRepository<AIDesign>(dbContext), IAIDesignRepository
    {
        public async Task<IEnumerable<AIDesign>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.RoomType)
                .Include(a => a.DesignStyle)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
