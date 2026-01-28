using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ChatMessageRepository(DecorteeDbContext dbContext) : GenericRepository<ChatMessage>(dbContext), IChatMessageRepository
    {
        public async Task<IEnumerable<ChatMessage>> GetByConversationIdAsync(string conversationId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.User)
                .Where(c => c.ConversationId == conversationId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ChatMessage>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
