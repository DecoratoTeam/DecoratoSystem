using Application.Dtos;
using Application.Dtos.RoomType;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Mapster;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class RoomTypeService(IRoomTypeRepository repository) : IRoomTypeService
    {
        private readonly IRoomTypeRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<RoomTypeResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            var dtos = entities.Adapt<IEnumerable<RoomTypeResponseDto>>();
            return GeneralResponseDto<IEnumerable<RoomTypeResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<RoomTypeResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<RoomTypeResponseDto>.Fail(ErrorType.NotFound, "RoomType not found");

            return GeneralResponseDto<RoomTypeResponseDto>.Success(entity.Adapt<RoomTypeResponseDto>());
        }

        public async Task<GeneralResponseDto<RoomTypeResponseDto>> CreateAsync(CreateRoomTypeDto dto, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByNameAsync(dto.Name, cancellationToken);
            if (existing != null)
                return GeneralResponseDto<RoomTypeResponseDto>.Fail(ErrorType.DuplicatedEmail, "RoomType with this name already exists");

            var entity = dto.Adapt<RoomType>();
            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<RoomTypeResponseDto>.Success(entity.Adapt<RoomTypeResponseDto>());
        }

        public async Task<GeneralResponseDto<RoomTypeResponseDto>> UpdateAsync(string id, UpdateRoomTypeDto dto, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<RoomTypeResponseDto>.Fail(ErrorType.NotFound, "RoomType not found");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            await _repository.UpdateAsync(entity, cancellationToken);
            return GeneralResponseDto<RoomTypeResponseDto>.Success(entity.Adapt<RoomTypeResponseDto>());
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "RoomType not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }
    }
}
