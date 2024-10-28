using System.Threading.Tasks;
using SenderService.Domain;

namespace SenderService.Services
{
    public abstract class MessageHandler
    {
        protected readonly MessageHandler _nextHandler;

        protected MessageHandler(MessageHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public abstract Task HandleMessageAsync(MessageToSend Letter);
    }
}
