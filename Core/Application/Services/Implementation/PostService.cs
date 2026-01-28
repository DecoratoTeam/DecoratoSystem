using Application.Dtos;
using Application.Dtos.Comment;
using Application.Dtos.Post;
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
    public class PostService(IPostRepository repository) : IPostService
    {
        private readonly IPostRepository _repository = repository;

        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllWithDetailsAsync(cancellationToken);
            var dtos = entities.Select(MapToResponseDto);
            return GeneralResponseDto<IEnumerable<PostResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<PostResponseDto>> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetWithDetailsAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<PostResponseDto>.Fail(ErrorType.NotFound, "Post not found");

            return GeneralResponseDto<PostResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<PostWithCommentsDto>> GetWithCommentsAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetWithCommentsAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<PostWithCommentsDto>.Fail(ErrorType.NotFound, "Post not found");

            var dto = new PostWithCommentsDto(
                entity.Id,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.Title,
                entity.Content,
                entity.ImageUrl,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.Votes?.Count(v => v.IsUpvote) ?? 0,
                entity.Votes?.Count(v => !v.IsUpvote) ?? 0,
                entity.Comments?.Select(c => new CommentResponseDto(
                    c.Id,
                    c.PostId,
                    c.UserId,
                    c.User?.Name ?? string.Empty,
                    c.Content,
                    c.CreatedAt,
                    c.UpdatedAt
                )).ToList() ?? new List<CommentResponseDto>()
            );

            return GeneralResponseDto<PostWithCommentsDto>.Success(dto);
        }

        public async Task<GeneralResponseDto<PostResponseDto>> CreateAsync(string userId, CreatePostDto dto, CancellationToken cancellationToken)
        {
            var entity = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                UserId = userId
            };

            await _repository.AddAsync(entity, cancellationToken);
            return GeneralResponseDto<PostResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<PostResponseDto>> UpdateAsync(string id, UpdatePostDto dto, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetWithDetailsAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<PostResponseDto>.Fail(ErrorType.NotFound, "Post not found");

            entity.Title = dto.Title;
            entity.Content = dto.Content;
            entity.ImageUrl = dto.ImageUrl;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity, cancellationToken);
            return GeneralResponseDto<PostResponseDto>.Success(MapToResponseDto(entity));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "Post not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        private static PostResponseDto MapToResponseDto(Post entity)
        {
            return new PostResponseDto(
                entity.Id,
                entity.UserId,
                entity.User?.Name ?? string.Empty,
                entity.Title,
                entity.Content,
                entity.ImageUrl,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.Votes?.Count(v => v.IsUpvote) ?? 0,
                entity.Votes?.Count(v => !v.IsUpvote) ?? 0,
                entity.Comments?.Count ?? 0);
        }
    }
}
