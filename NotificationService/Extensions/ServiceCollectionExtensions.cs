using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Configurations;
using NotificationService.Quartz;
using Quartz;

namespace NotificationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddQuartzNotify(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(q =>
            {
                var quartzConfig = configuration.GetSection("Quartz")
                    .Get<QuartzConfiguration>();

                q.UseMicrosoftDependencyInjectionJobFactory();

                q.ScheduleJob<RemindJob>(t =>
                    t.WithIdentity("remind")
                        .StartNow()
                        .WithCronSchedule(quartzConfig.RemindCronPeriod)
                );

                q.ScheduleJob<BackupJob>(t =>
                    t.WithIdentity("backup")
                        .StartNow()
                        .WithCronSchedule(quartzConfig.BackupCronPeriod)
                );
            });

            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
            return services;
        }

    }
}
