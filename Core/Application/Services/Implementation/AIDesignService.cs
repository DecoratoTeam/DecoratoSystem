using Application.Dtos;
using Application.Dtos.AIDesign;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class AIDesignService(IAIDesignRepository repository) : IAIDesignService
    {
        private readonly IAIDesignRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<AIDesignResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<AIDesignResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<AIDesignResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<AIDesignResponseDto>.Fail(ErrorType.NotFound, "AIDesign not found");

            return GeneralResponseDto<AIDesignResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<AIDesignResponseDto>> CreateAsync(string userId, CreateAIDesignDto dto, CancellationToken cancellationToken)
        {
            var entity = new AIDesign
            {
                InputImageUrl = dto.OriginalImageUrl,
                ResultImageUrl = dto.GeneratedImageUrl,
                Prompt = dto.Prompt,
                RoomTypeId = dto.RoomTypeId,
                DesignStyleId = dto.DesignStyleId,
                UserId = userId
            };

            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<AIDesignResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<AIDesignResponseDto>> UpdateAsync(string id, UpdateAIDesignDto dto, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<AIDesignResponseDto>.Fail(ErrorType.NotFound, "AIDesign not found");

            entity.InputImageUrl = dto.OriginalImageUrl;
            entity.ResultImageUrl = dto.GeneratedImageUrl;
            entity.Prompt = dto.Prompt;
            entity.RoomTypeId = dto.RoomTypeId;
            entity.DesignStyleId = dto.DesignStyleId;

            await _repository.UpdateAsync(entity, cancellationToken);
            return GeneralResponseDto<AIDesignResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "AIDesign not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<IEnumerable<AIDesignResponseDto>>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByUserIdAsync(userId, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<AIDesignResponseDto>>.Success(dtos);
        }

        private static AIDesignResponseDto MapToResponseDto(AIDesign entity)
        {
            return new AIDesignResponseDto(
                entity.Id,
                entity.InputImageUrl ?? string.Empty,
                entity.ResultImageUrl,
                entity.Prompt,
                entity.RoomTypeId,
                entity.RoomType?.Name ?? string.Empty,
                entity.DesignStyleId,
                entity.DesignStyle?.Name ?? string.Empty,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.CreatedAt);
        }
    }
}
