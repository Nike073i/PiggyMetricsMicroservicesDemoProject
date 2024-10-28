using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Настройка политик cookie
        /// </summary>
        /// <param name="serviceCollection">Коллекция сервисов</param>
        public static void ConfigureCookieOptions(this IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
                options.OnDeleteCookie = cookieContext =>
                    cookieContext.CookieOptions.SameSite = SameSiteMode.Unspecified;
            });
        }
    }
}
