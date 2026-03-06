namespace Application.Dtos.AIDesign
{
    public record UpdateAIDesignDto(
        string OriginalImageUrl,
        string GeneratedImageUrl,
        string? Prompt,
        string RoomTypeId,
        string DesignStyleId);
}
