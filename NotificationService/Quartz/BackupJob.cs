using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Services;
using Quartz;
using Serilog;

namespace NotificationService.Quartz
{
    public class BackupJob: IJob
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BackupJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var notificationService = scope.ServiceProvider.GetService<INotificationService>();
                await notificationService.SendBackupNotifications(context.CancellationToken);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, $"Error occured in Backupjob");
            }
        }
    }
}
