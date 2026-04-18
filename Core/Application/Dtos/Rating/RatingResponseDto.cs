using System;

namespace Application.Dtos.Rating
{
    public record RatingResponseDto(
        string Id,
        string ShowcaseDesignId,
        string ShowcaseDesignTitle,
        string ShowcaseDesignImageUrl,
        string UserId,
        string UserName,
        int Value,
        string? Review,
        DateTime CreatedAt);
}
