namespace DecorteeSystem.ViewModles.Auth
{
    public record RegisterResponseViewModle(
        string Id,
        string UserName,
        string Email,
        string Name
    );
}