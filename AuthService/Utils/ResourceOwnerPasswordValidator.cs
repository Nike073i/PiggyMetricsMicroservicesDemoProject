using System.Threading;
using System.Threading.Tasks;
using AuthService.Configuration;
using AuthService.Domain;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AuthService.Utils
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserStore<User> _users;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly string _pepper;

        public ResourceOwnerPasswordValidator(
            IUserStore<User> users,
            IOptions<PepperConfiguration> pepperOptions,
            IPasswordHasher<User> passwordHasher
        )
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _pepper = pepperOptions.Value.Pepper;
        }

        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _users.FindByNameAsync(context.UserName, CancellationToken.None);
            if (user == null || user.Deleted.HasValue)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            var passwordSpecie = PasswordSpecie.GetSpecie(
                context.Password,
                user.PasswordSalt,
                _pepper
            );

            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                passwordSpecie
            );
            if (verificationResult == PasswordVerificationResult.Success)
            {
                var sub = user.Id.ToString("N");
                context.Result = new GrantValidationResult(
                    sub,
                    OidcConstants.AuthenticationMethods.Password
                );
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            }
        }
    }
}
