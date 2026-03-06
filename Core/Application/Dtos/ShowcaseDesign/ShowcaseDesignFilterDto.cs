namespace Application.Dtos.ShowcaseDesign
{
    public record ShowcaseDesignFilterDto(
        string? RoomTypeId,
        string? StyleId,
        bool? IsPopular,
        bool? IsTrending);
}
