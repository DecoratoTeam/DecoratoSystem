namespace Application.Dtos.Profile
{
    public record UserRatingDto(
        string Id,
        string? ShowcaseDesignId,
        string? PostId,
        string? PostTitle,
        int Value,
        string? Review,
        DateTime CreatedAt);
}