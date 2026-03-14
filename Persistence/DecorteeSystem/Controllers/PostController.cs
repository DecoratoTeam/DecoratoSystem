using Application.Dtos;
using Application.Dtos.Post;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(GetUserId(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<PostResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, GetUserId(), cancellationToken);
        }

        [HttpGet("{id}/with-comments")]
        public async Task<GeneralResponseDto<PostWithCommentsDto>> GetWithComments(string id, CancellationToken cancellationToken)
        {
            return await _service.GetWithCommentsAsync(id, GetUserId(), cancellationToken);
        }

        [HttpPost]
        [Authorize]
        public async Task<GeneralResponseDto<PostResponseDto>> Create([FromBody] CreatePostDto dto, CancellationToken cancellationToken)
        {
            var userId = GetUserId()!;
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

        [HttpPost("{id}/like")]
        [Authorize]
        public async Task<GeneralResponseDto<ToggleLikeResponseDto>> ToggleLike(string id, CancellationToken cancellationToken)
        {
            var userId = GetUserId()!;
            return await _service.ToggleLikeAsync(userId, id, cancellationToken);
        }

        [HttpPost("{id}/save")]
        [Authorize]
        public async Task<GeneralResponseDto<ToggleSaveResponseDto>> ToggleSave(string id, CancellationToken cancellationToken)
        {
            var userId = GetUserId()!;
            return await _service.ToggleSaveAsync(userId, id, cancellationToken);
        }

        [HttpGet("saved")]
        [Authorize]
        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetSavedPosts(CancellationToken cancellationToken)
        {
            var userId = GetUserId()!;
            return await _service.GetSavedPostsAsync(userId, cancellationToken);
        }

        [HttpGet("recently-saved")]
        [Authorize]
        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetRecentlySaved([FromQuery] int take = 10, CancellationToken cancellationToken = default)
        {
            var userId = GetUserId()!;
            return await _service.GetRecentlySavedAsync(userId, take, cancellationToken);
        }
    }
}
