using Application.Dtos;
using Application.Dtos.ChatMessage;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ChatMessageService(IChatMessageRepository repository) : IChatMessageService
    {
        private readonly IChatMessageRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<ChatMessageResponseDto>>> GetByConversationIdAsync(string conversationId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByConversationIdAsync(conversationId, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<ChatMessageResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<ChatMessageResponseDto>> CreateAsync(string userId, CreateChatMessageDto dto, CancellationToken cancellationToken)
        {
            var entity = new ChatMessage
            {
                ConversationId = dto.ConversationId,
                UserId = userId,
                Content = dto.Content,
                IsFromUser = dto.IsFromUser
            };

            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<ChatMessageResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "ChatMessage not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        private static ChatMessageResponseDto MapToResponseDto(ChatMessage entity)
        {
            return new ChatMessageResponseDto(
                entity.Id,
                entity.ConversationId,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.Content,
                entity.IsFromUser,
                entity.CreatedAt);
        }
    }
}
