using Application.Dtos;
using Application.Dtos.Vote;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class VoteService(IVoteRepository repository) : IVoteService
    {
        private readonly IVoteRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<VoteResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<VoteResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<VoteResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<VoteResponseDto>.Fail(ErrorType.NotFound, "Vote not found");

            return GeneralResponseDto<VoteResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<VoteResponseDto>> CreateOrUpdateAsync(string userId, CreateVoteDto dto, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByUserAndPostAsync(userId, dto.PostId, cancellationToken);

            if (existing != null)
            {
                existing.IsUpvote = dto.IsUpvote;
                await _repository.UpdateAsync(existing, cancellationToken);
                return GeneralResponseDto<VoteResponseDto>.Success(MapToResponseDto(existing));
            }

            var entity = new Vote
            {
                PostId = dto.PostId,
                UserId = userId,
                IsUpvote = dto.IsUpvote
            };

            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<VoteResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "Vote not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        private static VoteResponseDto MapToResponseDto(Vote entity)
        {
            return new VoteResponseDto(
                entity.Id,
                entity.PostId,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.IsUpvote,
                entity.CreatedAt);
        }
    }
}
