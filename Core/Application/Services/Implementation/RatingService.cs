using Application.Dtos;
using Application.Dtos.Rating;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class RatingService(IRatingRepository repository) : IRatingService
    {
        private readonly IRatingRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<RatingResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<RatingResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<RatingResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<RatingResponseDto>.Fail(ErrorType.NotFound, "Rating not found");

            return GeneralResponseDto<RatingResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<IEnumerable<RatingResponseDto>>> GetByDesignIdAsync(string showcaseDesignId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByDesignIdAsync(showcaseDesignId, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<RatingResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<RatingResponseDto>> CreateOrUpdateAsync(string userId, CreateRatingDto dto, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByUserAndDesignAsync(userId, dto.ShowcaseDesignId, cancellationToken);

            if (existing != null)
            {
                existing.Value = dto.Value;
                existing.Review = dto.Review;
                await _repository.UpdateAsync(existing, cancellationToken);
                return GeneralResponseDto<RatingResponseDto>.Success(MapToResponseDto(existing));
            }

            var entity = new Rating
            {
                ShowcaseDesignId = dto.ShowcaseDesignId,
                UserId = userId,
                Value = dto.Value,
                Review = dto.Review
            };

            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<RatingResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "Rating not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        private static RatingResponseDto MapToResponseDto(Rating entity)
        {
            return new RatingResponseDto(
                entity.Id,
                entity.ShowcaseDesignId,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.Value,
                entity.Review,
                entity.CreatedAt);
        }
    }
}
