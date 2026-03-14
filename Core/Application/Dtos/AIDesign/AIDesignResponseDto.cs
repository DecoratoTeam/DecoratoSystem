using System;

namespace Application.Dtos.AIDesign
{
    public record AIDesignResponseDto(
        string Id,
        string? OriginalImageUrl,
        string GeneratedImageUrl,
        string? Prompt,
        string RoomTypeId,
        string RoomTypeName,
        string DesignStyleId,
        string DesignStyleName,
        string UserId,
        string UserName,
        DateTime CreatedAt);
}
