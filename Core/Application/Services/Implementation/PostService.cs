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
    public class PostService(
        IPostRepository repository,
        IVoteRepository voteRepository,
        ISavedPostRepository savedPostRepository) : IPostService
    {
        private readonly IPostRepository _repository = repository;
        private readonly IVoteRepository _voteRepository = voteRepository;
        private readonly ISavedPostRepository _savedPostRepository = savedPostRepository;

        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetAllAsync(string? userId, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllWithDetailsAsync(cancellationToken);
            var dtos = entities.Select(e => MapToResponseDto(e, userId));
            return GeneralResponseDto<IEnumerable<PostResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<PostResponseDto>> GetByIdAsync(string id, string? userId, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetWithDetailsAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<PostResponseDto>.Fail(ErrorType.NotFound, "Post not found");

            return GeneralResponseDto<PostResponseDto>.Success(MapToResponseDto(entity, userId));
        }

        public async Task<GeneralResponseDto<PostWithCommentsDto>> GetWithCommentsAsync(string id, string? userId, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetWithDetailsAsync(id, cancellationToken);
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
                )).ToList() ?? new List<CommentResponseDto>(),
                IsLiked: !string.IsNullOrEmpty(userId) && (entity.Votes?.Any(v => v.UserId == userId && v.IsUpvote) ?? false),
                IsSaved: !string.IsNullOrEmpty(userId) && (entity.SavedPosts?.Any(s => s.UserId == userId) ?? false)
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
            return GeneralResponseDto<PostResponseDto>.Success(MapToResponseDto(entity, userId));
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
            return GeneralResponseDto<PostResponseDto>.Success(MapToResponseDto(entity, null));
        }

        public async Task<GeneralResponseDto<bool>> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
                return GeneralResponseDto<bool>.Fail(ErrorType.NotFound, "Post not found");

            await _repository.DeleteAsync(entity, cancellationToken);
            return GeneralResponseDto<bool>.Success(true);
        }

        public async Task<GeneralResponseDto<ToggleLikeResponseDto>> ToggleLikeAsync(string userId, string postId, CancellationToken cancellationToken)
        {
            var post = await _repository.GetWithDetailsAsync(postId, cancellationToken);
            if (post == null)
                return GeneralResponseDto<ToggleLikeResponseDto>.Fail(ErrorType.NotFound, "Post not found");

            var existingVote = await _voteRepository.GetByUserAndPostAsync(userId, postId, cancellationToken);

            bool isLiked;
            if (existingVote != null)
            {
                if (existingVote.IsUpvote)
                {
                    // Already liked → remove like
                    await _voteRepository.DeleteAsync(existingVote, cancellationToken);
                    isLiked = false;
                }
                else
                {
                    // Had downvote → change to like
                    existingVote.IsUpvote = true;
                    await _voteRepository.UpdateAsync(existingVote, cancellationToken);
                    isLiked = true;
                }
            }
            else
            {
                // No vote → create like
                var vote = new Vote
                {
                    PostId = postId,
                    UserId = userId,
                    IsUpvote = true
                };
                await _voteRepository.AddAsync(vote, cancellationToken);
                isLiked = true;
            }

            var likeCount = (post.Votes?.Count(v => v.IsUpvote) ?? 0) + (isLiked && existingVote == null ? 1 : 0);
            if (existingVote != null && existingVote.IsUpvote && !isLiked)
                likeCount--;

            return GeneralResponseDto<ToggleLikeResponseDto>.Success(new ToggleLikeResponseDto(isLiked, likeCount));
        }

        public async Task<GeneralResponseDto<ToggleSaveResponseDto>> ToggleSaveAsync(string userId, string postId, CancellationToken cancellationToken)
        {
            var postExists = await _repository.ExistsAsync(postId, cancellationToken);
            if (!postExists)
                return GeneralResponseDto<ToggleSaveResponseDto>.Fail(ErrorType.NotFound, "Post not found");

            var existingSave = await _savedPostRepository.GetByUserAndPostAsync(userId, postId, cancellationToken);

            if (existingSave != null)
            {
                await _savedPostRepository.DeleteAsync(existingSave, cancellationToken);
                return GeneralResponseDto<ToggleSaveResponseDto>.Success(new ToggleSaveResponseDto(false));
            }

            var savedPost = new SavedPost
            {
                UserId = userId,
                PostId = postId
            };
            await _savedPostRepository.AddAsync(savedPost, cancellationToken);
            return GeneralResponseDto<ToggleSaveResponseDto>.Success(new ToggleSaveResponseDto(true));
        }

        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetSavedPostsAsync(string userId, CancellationToken cancellationToken)
        {
            var savedPosts = await _savedPostRepository.FindAsync(sp => sp.UserId == userId, cancellationToken);
            var postIds = savedPosts.Select(sp => sp.PostId).ToList();

            var allPosts = await _repository.GetAllWithDetailsAsync(cancellationToken);
            var posts = allPosts.Where(p => postIds.Contains(p.Id));

            var dtos = posts.Select(e => MapToResponseDto(e, userId));
            return GeneralResponseDto<IEnumerable<PostResponseDto>>.Success(dtos);
        }

        public async Task<GeneralResponseDto<IEnumerable<PostResponseDto>>> GetRecentlySavedAsync(string userId, int take, CancellationToken cancellationToken)
        {
            var savedPosts = await _savedPostRepository.GetRecentlySavedAsync(userId, take, cancellationToken);
            var dtos = savedPosts
                .Where(sp => sp.Post != null)
                .Select(sp => MapToResponseDto(sp.Post, userId));
            return GeneralResponseDto<IEnumerable<PostResponseDto>>.Success(dtos);
        }

        private static PostResponseDto MapToResponseDto(Post entity, string? userId)
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
                entity.Comments?.Count ?? 0,
                IsLiked: !string.IsNullOrEmpty(userId) && (entity.Votes?.Any(v => v.UserId == userId && v.IsUpvote) ?? false),
                IsSaved: !string.IsNullOrEmpty(userId) && (entity.SavedPosts?.Any(s => s.UserId == userId) ?? false));
        }
    }
}
