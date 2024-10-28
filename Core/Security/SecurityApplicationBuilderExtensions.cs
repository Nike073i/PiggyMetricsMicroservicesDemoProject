using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace Core.Security
{
    public static class SecurityApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDefaultCORSPolicy(this IApplicationBuilder app)
        {
            app.UseCors("default");

            return app;
        }

        public static IApplicationBuilder UseBearerAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            return app;
        }
    }
}
