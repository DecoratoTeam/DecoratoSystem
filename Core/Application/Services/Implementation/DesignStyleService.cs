using Application.Dtos;
using Application.Dtos.DesignStyle;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Mapster;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class DesignStyleService(IDesignStyleRepository repository) : IDesignStyleService
    {
        private readonly IDesignStyleRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<DesignStyleResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            var dtos = entities.Adapt<IEnumerable<DesignStyleResponseDto>>();
            return GeneralResponseDto<IEnumerable<DesignStyleResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<DesignStyleResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<DesignStyleResponseDto>.Fail(ErrorType.NotFound, "DesignStyle not found");

            return GeneralResponseDto<DesignStyleResponseDto>.Success(entity.Adapt<DesignStyleResponseDto>());
        }

        public async Task<GeneralResponseDto<DesignStyleResponseDto>> CreateAsync(CreateDesignStyleDto dto, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByNameAsync(dto.Name, cancellationToken);
            if (existing != null)
                return GeneralResponseDto<DesignStyleResponseDto>.Fail(ErrorType.DuplicatedEmail, "DesignStyle with this name already exists");

            var entity = dto.Adapt<DesignStyle>();
            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<DesignStyleResponseDto>.Success(entity.Adapt<DesignStyleResponseDto>());
        }

        public async Task<GeneralResponseDto<DesignStyleResponseDto>> UpdateAsync(string id, UpdateDesignStyleDto dto, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<DesignStyleResponseDto>.Fail(ErrorType.NotFound, "DesignStyle not found");

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            await _repository.UpdateAsync(entity, cancellationToken);
            return GeneralResponseDto<DesignStyleResponseDto>.Success(entity.Adapt<DesignStyleResponseDto>());
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "DesignStyle not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }
    }
}
