namespace DecorteeSystem.Abastraction.Consts
{
    public static class RegexPatterns
    {
        public static string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
    }

}
