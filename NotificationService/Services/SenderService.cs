using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using NotificationService.AsyncClient;
using NotificationService.Domain;
using Serilog;

namespace NotificationService.Services
{
    public class SenderService
    {
        private readonly ConcurrentQueue<MessageToSend> _sendQueue =
            new ConcurrentQueue<MessageToSend>();

        private readonly NotificationQueuePublisher _notificationQueuePublisher;

        public SenderService(NotificationQueuePublisher notificationQueuePublisher)
        {
            _notificationQueuePublisher = notificationQueuePublisher;

            _ = Task.Run(SendFunc);
        }

        public void Send(MessageToSend messageToSend)
        {
            _sendQueue.Enqueue(messageToSend);
        }

        private async Task SendFunc()
        {
            while (true)
            {
                if (_sendQueue.TryDequeue(out var msg))
                {
                    try
                    {
                        SendInternal(msg);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Logger.Error(ex, "Failed to publish message");
                        _sendQueue.Enqueue(msg);
                    }
                }
                else
                {
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }
        }

        private void SendInternal(MessageToSend messageToSend)
        {
            _notificationQueuePublisher.Publish(messageToSend);
        }
    }
}
