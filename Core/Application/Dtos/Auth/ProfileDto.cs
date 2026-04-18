namespace Application.Dtos.Auth
{
    public record ProfileDto(string Id, string Name, string UserName, string Email, string? ProfilePicture, string? Bio, bool IsDarkMode, string Language);
}
