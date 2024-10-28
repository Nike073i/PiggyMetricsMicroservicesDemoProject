using AuthService.Domain;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Services
{
    /// <summary>
    /// Сервис заполнения claims пользователя
    /// </summary>
    public class UserProfileService : IProfileService
    {
        /// <summary>
        /// Репозиторий пользователей
        /// </summary>
        private readonly IUserStore<User> _userStore;

        /// <summary>
        /// Настройки ASP.NET Identity
        /// </summary>
        private readonly IdentityOptions _identityOptions;

        public UserProfileService(IUserStore<User> userStore, IOptions<IdentityOptions> optionsAccessor)
        {
            _userStore = userStore;
            _identityOptions = optionsAccessor.Value;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string userId = context.Subject.Identity.GetSubjectId();

            User user = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            if (user == null)
            {
                return;
            }

            ClaimsIdentity userClaims = GetBaseUserClaims(user);

            context.IssuedClaims = userClaims.Claims.ToList();
        }

        private ClaimsIdentity GetBaseUserClaims(User user)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity("Identity.Application",
                _identityOptions.ClaimsIdentity.UserNameClaimType,
                _identityOptions.ClaimsIdentity.RoleClaimType);

            claimsIdentity.AddClaim(new Claim(_identityOptions.ClaimsIdentity.UserIdClaimType, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(_identityOptions.ClaimsIdentity.UserNameClaimType, user.UserName));

            return claimsIdentity;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string userId = context.Subject.Identity.GetSubjectId();

            User user = await _userStore.FindByIdAsync(userId, CancellationToken.None);

            context.IsActive = user != null && user.Deleted == null;
        }
    }
}
