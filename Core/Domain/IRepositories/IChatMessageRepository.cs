using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetByConversationIdAsync(string conversationId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ChatMessage>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
