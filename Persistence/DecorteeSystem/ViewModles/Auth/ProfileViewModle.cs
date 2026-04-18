namespace DecorteeSystem.ViewModles.Auth
{
    public record ProfileViewModle(string Id, string Name, string UserName, string Email, string? ProfilePicture, string? Bio, bool IsDarkMode, string Language);
}
