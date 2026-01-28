namespace Application.Dtos.AIDesign
{
    public record CreateAIDesignDto(
        string OriginalImageUrl,
        string GeneratedImageUrl,
        string? Prompt,
        string RoomTypeId,
        string DesignStyleId);
}
