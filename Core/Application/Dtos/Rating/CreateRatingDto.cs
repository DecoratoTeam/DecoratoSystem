namespace Application.Dtos.Rating
{
    public record CreateRatingDto(string ShowcaseDesignId, int Value, string? Review);
}
