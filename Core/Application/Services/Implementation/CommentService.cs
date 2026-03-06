using Application.Dtos;
using Application.Dtos.Comment;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class CommentService(ICommentRepository repository) : ICommentService
    {
        private readonly ICommentRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<CommentResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<CommentResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<CommentResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<CommentResponseDto>.Fail(ErrorType.NotFound, "Comment not found");

            return GeneralResponseDto<CommentResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<IEnumerable<CommentResponseDto>>> GetByPostIdAsync(string postId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetByPostIdAsync(postId, cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<CommentResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<CommentResponseDto>> CreateAsync(string userId, CreateCommentDto dto, CancellationToken cancellationToken)
        {
            var entity = new Comment
            {
                PostId = dto.PostId,
                UserId = userId,
                Content = dto.Content
            };

            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<CommentResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<CommentResponseDto>> UpdateAsync(string id, UpdateCommentDto dto, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<CommentResponseDto>.Fail(ErrorType.NotFound, "Comment not found");

            entity.Content = dto.Content;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity, cancellationToken);
            return GeneralResponseDto<CommentResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "Comment not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        private static CommentResponseDto MapToResponseDto(Comment entity)
        {
            return new CommentResponseDto(
                entity.Id,
                entity.PostId,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.Content,
                entity.CreatedAt,
                entity.UpdatedAt);
        }
    }
}
