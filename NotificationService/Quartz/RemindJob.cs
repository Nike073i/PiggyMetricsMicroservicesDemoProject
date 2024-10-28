using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Services;
using Quartz;
using Serilog;

namespace NotificationService.Quartz
{
    public class RemindJob: IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RemindJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var notificationService = scope.ServiceProvider.GetService<INotificationService>();
                await notificationService.SendRemindNotifications(context.CancellationToken);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, $"Error occured in Remindjob");
            }
        }
    }
}
