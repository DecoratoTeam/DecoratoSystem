namespace Application.Dtos.ShowcaseDesign
{
    public record UpdateShowcaseDesignDto(
        string Title,
        string? Description,
        string ImageUrl,
        string RoomTypeId,
        string DesignStyleId,
        bool IsPopular,
        bool IsTrending);
}
