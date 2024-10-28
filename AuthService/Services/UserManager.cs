using System;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Configuration;
using AuthService.Domain;
using AuthService.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AuthService.Services
{
    ///<inheritdoc/>
    public class UserManager : IUserManager
    {
        private readonly IUserStore<User> _userStore;

        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly string _pepper;

        public UserManager(
            IUserStore<User> userStore,
            IOptions<PepperConfiguration> pepperOptions,
            IPasswordHasher<User> passwordHasher
        )
        {
            _userStore = userStore;
            _passwordHasher = passwordHasher;
            _pepper = pepperOptions.Value.Pepper;
        }

        ///<inheritdoc/>
        public async Task CreateAsync(UserModel userModel, CancellationToken token = default)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            User user = new User
            {
                Created = utcNow,
                Updated = utcNow,
                UserName = userModel.Username,
            };

            string normalizedName = await _userStore.GetNormalizedUserNameAsync(user, token);

            User existUser = await _userStore.FindByNameAsync(normalizedName, token);

            if (existUser != null)
            {
                throw new Exception(
                    $"Пользователь с таким именем {userModel.Username} уже существует"
                );
            }

            user.PasswordSalt = Guid.NewGuid().ToString();
            var passwordSpecie = PasswordSpecie.GetSpecie(
                userModel.Password,
                user.PasswordSalt,
                _pepper
            );
            user.PasswordHash = _passwordHasher.HashPassword(user, passwordSpecie);

            await _userStore.CreateAsync(user, token);
        }
    }
}
