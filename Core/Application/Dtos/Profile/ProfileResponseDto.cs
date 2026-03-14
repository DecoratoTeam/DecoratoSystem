namespace Application.Dtos.Profile
{
    public record ProfileResponseDto(
        string Id,
        string Name,
        string Email,
        string? ProfileImage,
        bool DarkMode,
        string Language,
        int PostsCount,
        int RatingsCount
    );
}
