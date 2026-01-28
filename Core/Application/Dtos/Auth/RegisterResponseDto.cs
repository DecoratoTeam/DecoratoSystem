namespace Application.Dtos.Auth
{
    public record RegisterResponseDto(
        string Id, 
        string UserName, 
        string Email, 
        string Name
    );
}