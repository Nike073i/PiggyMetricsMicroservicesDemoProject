using System.Text;
using Newtonsoft.Json;
using SenderService.AsyncServices;
using SenderService.Domain;
using Serilog;

namespace SenderService.Services
{
    public class ReceiverService
    {
        private readonly NotificationQueueConsumer _notifictionQueueConsumer;

        private MessageHandler _messageHandler;

        public ReceiverService(
            NotificationQueueConsumer notifictionQueueConsumer,
            IEmailService emailService
        )
        {
            _notifictionQueueConsumer = notifictionQueueConsumer;

            _notifictionQueueConsumer.MessageRecieved += _notifictionQueueConsumer_MessageRecieved;

            _messageHandler = new EmailMessageHandler(emailService, null); // build chain of responsobilities for other possible types of addresses
        }

        public void StartRecieve()
        {
            _notifictionQueueConsumer.StartConsume();
        }

        private void _notifictionQueueConsumer_MessageRecieved(
            object sender,
            MessageRecievedEventArgs e
        )
        {
            var json = Encoding.UTF8.GetString(e.MessageData);

            var message = JsonConvert.DeserializeObject<MessageToSend>(json);

            try
            {
                SendMessage(message);
            }
            catch (System.Exception ex)
            {
                Log.Logger.Error(ex, "Failed to send message");
                e.Requeue = true;
            }
        }

        private void SendMessage(MessageToSend message)
        {
            _messageHandler.HandleMessageAsync(message).Wait();
        }
    }
}
