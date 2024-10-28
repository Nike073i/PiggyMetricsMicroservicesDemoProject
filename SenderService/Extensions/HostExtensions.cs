using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SenderService.Services;

namespace SenderService.Extensions
{
    public static class HostExtension
    {
        public static IHost RunReceiveService(this IHost host)
        {
            var service = host.Services.GetService<ReceiverService>();

            service.StartRecieve();

            return host;
        }
    }
}
