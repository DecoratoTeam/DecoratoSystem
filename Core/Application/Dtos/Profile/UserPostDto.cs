namespace Application.Dtos.Profile
{
    public record UserPostDto(string Id, string? Image, string Description, DateTime CreatedAt);
}
