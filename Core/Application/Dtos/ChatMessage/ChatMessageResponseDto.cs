using System;

namespace Application.Dtos.ChatMessage
{
    public record ChatMessageResponseDto(
        string Id,
        string ConversationId,
        string UserId,
        string UserName,
        string Content,
        bool IsFromUser,
        DateTime CreatedAt);
}
