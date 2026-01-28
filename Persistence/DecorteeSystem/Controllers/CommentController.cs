using Application.Dtos;
using Application.Dtos.Comment;
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
    public class CommentController(ICommentService service) : ControllerBase
    {
        private readonly ICommentService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<CommentResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<CommentResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpGet("post/{postId}")]
        public async Task<GeneralResponseDto<IEnumerable<CommentResponseDto>>> GetByPostId(string postId, CancellationToken cancellationToken)
        {
            return await _service.GetByPostIdAsync(postId, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<CommentResponseDto>> Create([FromBody] CreateCommentDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await _service.CreateAsync(userId, dto, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<GeneralResponseDto<CommentResponseDto>> Update(string id, [FromBody] UpdateCommentDto dto, CancellationToken cancellationToken)
        {
            return await _service.UpdateAsync(id, dto, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<GeneralResponseDto<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(id, cancellationToken);
        }
    }
}
