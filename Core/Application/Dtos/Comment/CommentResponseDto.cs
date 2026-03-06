using System;

namespace Application.Dtos.Comment
{
    public record CommentResponseDto(
        string Id,
        string PostId,
        string UserId,
        string UserName,
        string Content,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
