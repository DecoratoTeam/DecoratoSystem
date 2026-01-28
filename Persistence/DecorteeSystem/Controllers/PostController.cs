using Application.Dtos;
using Application.Dtos.Post;
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
    public class PostController(IPostService service) : ControllerBase
    {
        private readonly IPostService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<PostResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpGet("{id}/with-comments")]
        public async Task<GeneralResponseDto<PostWithCommentsDto>> GetWithComments(string id, CancellationToken cancellationToken)
        {
            return await _service.GetWithCommentsAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<PostResponseDto>> Create([FromBody] CreatePostDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await _service.CreateAsync(userId, dto, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<GeneralResponseDto<PostResponseDto>> Update(string id, [FromBody] UpdatePostDto dto, CancellationToken cancellationToken)
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
