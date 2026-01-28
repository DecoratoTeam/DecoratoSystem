using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CommentRepository(DecorteeDbContext dbContext) : GenericRepository<Comment>(dbContext), ICommentRepository
    {
        public async Task<IEnumerable<Comment>> GetByPostIdAsync(string postId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
