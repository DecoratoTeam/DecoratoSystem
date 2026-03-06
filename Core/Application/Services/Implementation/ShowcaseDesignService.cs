using Application.Dtos;
using Application.Dtos.ShowcaseDesign;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ShowcaseDesignService(IShowcaseDesignRepository repository) : IShowcaseDesignService
    {
        private readonly IShowcaseDesignRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetFilteredAsync(null, null, null, null, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<ShowcaseDesignResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<ShowcaseDesignResponseDto>.Fail(ErrorType.NotFound, "ShowcaseDesign not found");

            return GeneralResponseDto<ShowcaseDesignResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<ShowcaseDesignResponseDto>> CreateAsync(string userId, CreateShowcaseDesignDto dto, CancellationToken cancellationToken)
        {
            var entity = new ShowcaseDesign
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                RoomTypeId = dto.RoomTypeId,
                DesignStyleId = dto.DesignStyleId,
                UserId = userId,
                IsPopular = dto.IsPopular,
                IsTrending = dto.IsTrending
            };

            await _repository.AddAsync(entity, cancellationToken);

            var created = await _repository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);
            return GeneralResponseDto<ShowcaseDesignResponseDto>.Success(MapToResponseDto(created!));
        }

        public async Task<GeneralResponseDto<ShowcaseDesignResponseDto>> UpdateAsync(string id, UpdateShowcaseDesignDto dto, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<ShowcaseDesignResponseDto>.Fail(ErrorType.NotFound, "ShowcaseDesign not found");

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.ImageUrl = dto.ImageUrl;
            entity.RoomTypeId = dto.RoomTypeId;
            entity.DesignStyleId = dto.DesignStyleId;
            entity.IsPopular = dto.IsPopular;
            entity.IsTrending = dto.IsTrending;

            await _repository.UpdateAsync(entity, cancellationToken);
            return GeneralResponseDto<ShowcaseDesignResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "ShowcaseDesign not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetByRoomTypeAsync(string roomTypeId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByRoomTypeAsync(roomTypeId, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetByStyleAsync(string styleId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByStyleAsync(styleId, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetPopularAsync(int take, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetPopularAsync(take, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetTrendingAsync(int take, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetTrendingAsync(take, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>> GetFilteredAsync(ShowcaseDesignFilterDto filter, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetFilteredAsync(filter.RoomTypeId, filter.StyleId, filter.IsPopular, filter.IsTrending, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<ShowcaseDesignResponseDto>>.Success(dtos);
        }

        private static ShowcaseDesignResponseDto MapToResponseDto(ShowcaseDesign entity)
        {
            var avgRating = entity.Ratings?.Any() == true ? entity.Ratings.Average(r => r.Value) : 0;
            return new ShowcaseDesignResponseDto(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.ImageUrl,
                entity.RoomTypeId,
                entity.RoomType?.Name ?? string.Empty,
                entity.DesignStyleId,
                entity.DesignStyle?.Name ?? string.Empty,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.IsPopular,
                entity.IsTrending,
                entity.CreatedAt,
                avgRating);
        }
    }
}
