namespace Application.Dtos.ShowcaseDesign
{
    public record CreateShowcaseDesignDto(
        string Title,
        string? Description,
        string ImageUrl,
        string RoomTypeId,
        string DesignStyleId,
        bool IsPopular = false,
        bool IsTrending = false);
}
