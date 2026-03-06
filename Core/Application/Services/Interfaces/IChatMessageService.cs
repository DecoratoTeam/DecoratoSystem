using Application.Dtos;
using Application.Dtos.ChatMessage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<GeneralResponseDto<IEnumerable<ChatMessageResponseDto>>> GetByConversationIdAsync(string conversationId, CancellationToken cancellationToken);
        Task<GeneralResponseDto<ChatMessageResponseDto>> CreateAsync(string userId, CreateChatMessageDto dto, CancellationToken cancellationToken);
        Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
