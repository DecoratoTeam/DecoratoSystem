namespace Application.Dtos.Profile
{
    public record ProfileRatingDto(
        string ShowcaseDesignId,
        string? ShowcaseDesignTitle,
        int RatingValue,
        string? Review
    );
}
