using Application.Dtos;
using Application.Dtos.AIDesign;
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
    public class AIDesignController(IAIDesignService service) : ControllerBase
    {
        private readonly IAIDesignService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<AIDesignResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<AIDesignResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpGet("user/{userId}")]
        public async Task<GeneralResponseDto<IEnumerable<AIDesignResponseDto>>> GetByUserId(string userId, CancellationToken cancellationToken)
        {
            return await _service.GetByUserIdAsync(userId, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<AIDesignResponseDto>> Create([FromBody] CreateAIDesignDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await _service.CreateAsync(userId, dto, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<GeneralResponseDto<AIDesignResponseDto>> Update(string id, [FromBody] UpdateAIDesignDto dto, CancellationToken cancellationToken)
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
