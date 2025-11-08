namespace DecorteeSystem.ViewModles.Auth
{
    public record ResetPasswordViewModle(string Email, string Otp, string NewPassword);
}