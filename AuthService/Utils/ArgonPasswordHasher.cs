using AuthService.Domain;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Utils
{
    public class ArgonPasswordHasher : IPasswordHasher<User>
    {
        public string HashPassword(User user, string password)
        {
            return Argon2.Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(
            User user,
            string hashedPassword,
            string providedPassword
        )
        {
            var result = Argon2.Verify(hashedPassword, providedPassword);

            return result ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
