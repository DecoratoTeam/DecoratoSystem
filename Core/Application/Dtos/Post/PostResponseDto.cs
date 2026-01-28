using System;

namespace Application.Dtos.Post
{
    public record PostResponseDto(
        string Id,
        string UserId,
        string UserName,
        string Title,
        string Content,
        string? ImageUrl,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        int UpvoteCount,
        int DownvoteCount,
        int CommentCount);
}
