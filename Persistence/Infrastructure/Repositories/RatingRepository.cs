using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RatingRepository(DecorteeDbContext dbContext) : GenericRepository<Rating>(dbContext), IRatingRepository
    {
        public async Task<Rating?> GetByUserAndDesignAsync(string userId, string showcaseDesignId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.UserId == userId && r.ShowcaseDesignId == showcaseDesignId, cancellationToken);
        }

        public async Task<IEnumerable<Rating>> GetByDesignIdAsync(string showcaseDesignId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => r.ShowcaseDesignId == showcaseDesignId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Rating>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(r => r.ShowcaseDesign)
                .Include(r => r.Post)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
