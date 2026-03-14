namespace Application.Dtos.Profile
{
    public record ProfilePostDto(
        string Id,
        string? ImageUrl,
        string Content,
        DateTime CreatedAt
    );
}
