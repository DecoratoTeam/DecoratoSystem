namespace Application.Dtos.Profile
{
    public record UserRatingDto(string ShowcaseDesignId, string ShowcaseDesignTitle, string? ShowcaseDesignImageUrl, int RatingValue);
}
