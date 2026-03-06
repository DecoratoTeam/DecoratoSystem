using Application.Dtos;
using Application.Dtos.Rating;
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
    public class RatingController(IRatingService service) : ControllerBase
    {
        private readonly IRatingService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<RatingResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<RatingResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpGet("design/{showcaseDesignId}")]
        public async Task<GeneralResponseDto<IEnumerable<RatingResponseDto>>> GetByDesignId(string showcaseDesignId, CancellationToken cancellationToken)
        {
            return await _service.GetByDesignIdAsync(showcaseDesignId, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<RatingResponseDto>> CreateOrUpdate([FromBody] CreateRatingDto dto, CancellationToken cancellationToken)
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
