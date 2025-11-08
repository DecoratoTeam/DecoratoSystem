namespace Application.Dtos.Auth
{
    public record ResetPasswordDto(string Email, string Otp, string NewPassword);
}