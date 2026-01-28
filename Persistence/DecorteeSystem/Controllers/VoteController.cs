using Application.Dtos;
using Application.Dtos.Vote;
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
    public class VoteController(IVoteService service) : ControllerBase
    {
        private readonly IVoteService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<VoteResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<VoteResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<VoteResponseDto>> CreateOrUpdate([FromBody] CreateVoteDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await _service.CreateOrUpdateAsync(userId, dto, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<GeneralResponseDto<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            return await _service.DeleteAsync(id, cancellationToken);
        }
    }
}
