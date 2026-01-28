using System;

namespace Application.Dtos.ShowcaseDesign
{
    public record ShowcaseDesignResponseDto(
        string Id,
        string Title,
        string? Description,
        string ImageUrl,
        string RoomTypeId,
        string RoomTypeName,
        string DesignStyleId,
        string DesignStyleName,
        string UserId,
        string UserName,
        bool IsPopular,
        bool IsTrending,
        DateTime CreatedAt,
        double AverageRating);
}
