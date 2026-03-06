using Application.Dtos;
using Application.Dtos.ShowcaseDesign;
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
    public class ShowcaseDesignController(IShowcaseDesignService service) : ControllerBase
    {
        private readonly IShowcaseDesignService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<ShowcaseDesignResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpGet("by-room-type/{roomTypeId}")]
        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetByRoomType(string roomTypeId, CancellationToken cancellationToken)
        {
            return await _service.GetByRoomTypeAsync(roomTypeId, cancellationToken);
        }

        [HttpGet("by-style/{styleId}")]
        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetByStyle(string styleId, CancellationToken cancellationToken)
        {
            return await _service.GetByStyleAsync(styleId, cancellationToken);
        }

        [HttpGet("popular")]
        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetPopular([FromQuery] int take = 10, CancellationToken cancellationToken = default)
        {
            return await _service.GetPopularAsync(take, cancellationToken);
        }

        [HttpGet("trending")]
        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetTrending([FromQuery] int take = 10, CancellationToken cancellationToken = default)
        {
            return await _service.GetTrendingAsync(take, cancellationToken);
        }

        [HttpGet("filter")]
        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetFiltered([FromQuery] ShowcaseDesignFilterDto filter, CancellationToken cancellationToken)
        {
            return await _service.GetFilteredAsync(filter, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<ShowcaseDesignResponseDto>> Create([FromBody] CreateShowcaseDesignDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            return await _service.CreateAsync(userId, dto, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<GeneralResponseDto<ShowcaseDesignResponseDto>> Update(string id, [FromBody] UpdateShowcaseDesignDto dto, CancellationToken cancellationToken)
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
