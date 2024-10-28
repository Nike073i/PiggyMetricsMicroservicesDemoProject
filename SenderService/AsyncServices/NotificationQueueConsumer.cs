using System;
using System.Threading;
using System.Threading.Tasks;
using Core.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace SenderService.AsyncServices
{
    public class NotificationQueueConsumer : BaseRabbitMqTransmitter
    {
        private string _consumerTag;

        private Task _startTask;

        private readonly object _locker = new object();

        public NotificationQueueConsumer(
            RabbitMqChannelFactory channelFactory,
            IRabbitTopology topology
        )
            : base(channelFactory, topology) { }

        public Task StartConsume(CancellationToken cancellationToken = default)
        {
            lock (_locker)
            {
                if (_startTask == null)
                {
                    _startTask = Task.Run(
                        async () =>
                            await StartConsumeInternal(cancellationToken).ConfigureAwait(false),
                        cancellationToken
                    );
                }
            }

            return _startTask;
        }

        private async Task StartConsumeInternal(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!string.IsNullOrEmpty(_consumerTag))
                {
                    return;
                }

                bool waitBeforeNextTry = false;
                try
                {
                    var channel = GetChannel();

                    Topology.EstablishTopology(channel);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;

                    _consumerTag = channel.BasicConsume(Topology.RoutingKey, false, consumer);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Failed to start consume from RabbitMq queue");
                    waitBeforeNextTry = true;
                }

                if (waitBeforeNextTry)
                {
                    await Task.Delay(5000, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var consumer = (EventingBasicConsumer)sender;

            var data = e.Body.ToArray();
            var args = new MessageRecievedEventArgs(data);
            MessageRecieved?.Invoke(this, args);

            if (args.Requeue)
            {
                consumer.Model.BasicReject(e.DeliveryTag, true);
            }
            else
            {
                consumer.Model.BasicAck(e.DeliveryTag, false);
            }
        }
    }
}
