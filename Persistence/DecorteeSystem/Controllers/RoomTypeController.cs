using Application.Dtos;
using Application.Dtos.RoomType;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DecorteeSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController(IRoomTypeService service) : ControllerBase
    {
        private readonly IRoomTypeService _service = service;

        [HttpGet]
        public async Task<GeneralResponseDto<IEnumerable<RoomTypeResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<GeneralResponseDto<RoomTypeResponseDto>> GetById(string id, CancellationToken cancellationToken)
        {
            return await _service.GetByIdAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<GeneralResponseDto<RoomTypeResponseDto>> Create([FromBody] CreateRoomTypeDto dto, CancellationToken cancellationToken)
        {
            return await _service.CreateAsync(dto, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<GeneralResponseDto<RoomTypeResponseDto>> Update(string id, [FromBody] UpdateRoomTypeDto dto, CancellationToken cancellationToken)
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
