using AuthService.Domain;
using AuthService.Repository;
using AuthService.Services;
using AuthService.Store;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Extensions
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddMongoRepository(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IRepository, MongoRepository>();

            return builder;
        }

        public static IIdentityServerBuilder AddProfileService(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IProfileService, UserProfileService>();

            return builder;
        }

        public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IClientStore, MongoClientStore>();
            builder.Services.AddTransient<ICorsPolicyService, InMemoryCorsPolicyService>();

            return builder;
        }

        public static IIdentityServerBuilder AddIdentityApiResources(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IResourceStore, MongoResourceStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddPersistedGrants(this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<IPersistedGrantStore, MongoPersistedGrantStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddUsers(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            builder.Services.AddTransient<IUserStore<User>, MongoUserStore>();

            return builder;
        }
    }
}
