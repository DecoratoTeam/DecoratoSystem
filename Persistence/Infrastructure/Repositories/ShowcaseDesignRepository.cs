using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ShowcaseDesignRepository(DecorteeDbContext dbContext) : GenericRepository<ShowcaseDesign>(dbContext), IShowcaseDesignRepository
    {
        public async Task<IEnumerable<ShowcaseDesign>> GetByRoomTypeAsync(string roomTypeId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.RoomType)
                .Include(s => s.DesignStyle)
                .Include(s => s.User)
                .Where(s => s.RoomTypeId == roomTypeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ShowcaseDesign>> GetByStyleAsync(string styleId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.RoomType)
                .Include(s => s.DesignStyle)
                .Include(s => s.User)
                .Where(s => s.DesignStyleId == styleId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ShowcaseDesign>> GetPopularAsync(int take = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.RoomType)
                .Include(s => s.DesignStyle)
                .Include(s => s.User)
                .Where(s => s.IsPopular)
                .OrderByDescending(s => s.CreatedAt)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ShowcaseDesign>> GetTrendingAsync(int take = 10, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.RoomType)
                .Include(s => s.DesignStyle)
                .Include(s => s.User)
                .Where(s => s.IsTrending)
                .OrderByDescending(s => s.CreatedAt)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ShowcaseDesign>> GetFilteredAsync(string? roomTypeId, string? styleId, bool? isPopular, bool? isTrending, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(s => s.RoomType)
                .Include(s => s.DesignStyle)
                .Include(s => s.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(roomTypeId))
                query = query.Where(s => s.RoomTypeId == roomTypeId);

            if (!string.IsNullOrEmpty(styleId))
                query = query.Where(s => s.DesignStyleId == styleId);

            if (isPopular.HasValue)
                query = query.Where(s => s.IsPopular == isPopular.Value);

            if (isTrending.HasValue)
                query = query.Where(s => s.IsTrending == isTrending.Value);

            return await query.OrderByDescending(s => s.CreatedAt).ToListAsync(cancellationToken);
        }

        public async Task<ShowcaseDesign?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(s => s.RoomType)
                .Include(s => s.DesignStyle)
                .Include(s => s.User)
                .Include(s => s.Ratings)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}
