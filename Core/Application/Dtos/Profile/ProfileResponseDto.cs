namespace Application.Dtos.Profile
{
    public record ProfileResponseDto(
        string Id,
        string UserName,
        string Name,
        string Email,
        string? ProfilePicture,
        string? Bio,
        string Language,
        bool IsDarkMode,
        int PostsCount,
        int RatingsCount);
}