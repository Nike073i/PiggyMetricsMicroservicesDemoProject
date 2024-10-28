namespace AuthService.Utils
{
    public static class PasswordSpecie
    {
        public static string GetSpecie(string password, string salt, string pepper) =>
            string.Concat(password, salt, pepper);
    }
}
