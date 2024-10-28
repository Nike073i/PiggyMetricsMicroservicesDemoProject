using System;
using System.Threading;
using System.Threading.Tasks;
using SenderService.Domain;

namespace SenderService.Services
{
    public interface IEmailService
    {
        Task SendAsync(MessageToSend letter, CancellationToken token = default);
    }
}
