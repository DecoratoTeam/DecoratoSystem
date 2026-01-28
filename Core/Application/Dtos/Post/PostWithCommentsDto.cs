using System;
using System.Collections.Generic;
using Application.Dtos.Comment;

namespace Application.Dtos.Post
{
    public record PostWithCommentsDto(
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
        List<CommentResponseDto> Comments);
}
