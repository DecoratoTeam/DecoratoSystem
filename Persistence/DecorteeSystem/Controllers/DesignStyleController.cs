using Application.Dtos;
using Application.Dtos.DesignStyle;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DecorteeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignStyleController(IDesignStyleService service) : ControllerBase
    {
        private readonly IDesignStyleService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<DesignStyleResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<DesignStyleResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<DesignStyleResponseDto>> Create([FromBody] CreateDesignStyleDto dto, CancellationToken cancellationToken)
        {
            return await _service.CreateAsync(dto, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<GeneralResponseDto<DesignStyleResponseDto>> Update(string id, [FromBody] UpdateDesignStyleDto dto, CancellationToken cancellationToken)
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
