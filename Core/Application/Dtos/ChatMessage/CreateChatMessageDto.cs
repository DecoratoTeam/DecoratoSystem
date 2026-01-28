namespace Application.Dtos.ChatMessage
{
    public record CreateChatMessageDto(string ConversationId, string Content, bool IsFromUser);
}
