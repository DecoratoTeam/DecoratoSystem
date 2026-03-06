using System;

namespace Application.Dtos.Vote
{
    public record VoteResponseDto(
        string Id,
        string PostId,
        string UserId,
        string UserName,
        bool IsUpvote,
        DateTime CreatedAt);
}
