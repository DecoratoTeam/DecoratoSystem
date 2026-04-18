using Application.Dtos;
using Application.Dtos.Design;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace DecorteeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class DesignController(IDesignService service) : ControllerBase
    {
        private readonly IDesignService _service = service;

        [HttpPost("generate")]
        [ProducesResponseType(typeof(GeneralResponseDto<DesignImageResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<DesignImageResponseDto>>> Generate(
            [FromBody] GenerateDesignDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _service.GenerateFromTextAsync(dto.Prompt, cancellationToken);
            return Ok(result);
        }

        [HttpPost("transform")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(GeneralResponseDto<DesignImageResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<GeneralResponseDto<DesignImageResponseDto>>> Transform(
            [FromForm] TransformDesignDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _service.TransformAsync(dto.Image, dto.Prompt, cancellationToken);
            return Ok(result);
        }
    }
}