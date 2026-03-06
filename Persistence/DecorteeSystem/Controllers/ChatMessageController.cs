using Application.Dtos;
using Application.Dtos.ChatMessage;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DecorteeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController(IChatMessageService service) : ControllerBase
    {
        private readonly IChatMessageService _service = service;

        [HttpGet("conversation/{conversationId}")]
        public async Task<GeneralResponseDto<IEnumerable<ChatMessageResponseDto>>> GetByConversationId(string conversationId, CancellationToken cancellationToken)
        {
            return await _service.GetByConversationIdAsync(conversationId, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<ChatMessageResponseDto>> Create([FromBody] CreateChatMessageDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await _service.CreateAsync(userId, dto, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<GeneralResponseDto<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(id, cancellationToken);
        }
    }
}
