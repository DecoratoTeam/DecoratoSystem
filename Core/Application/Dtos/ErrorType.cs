namespace Application.Dtos
{
    public enum ErrorType
    {
        None = 0,
        DuplicatedEmail = 100,
        InvalidCredentials = 101,
        NotFound = 102,
        ServerError = 103,
        BadRequest = 104
    }
}